using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatStatus{
    WAITING_FOR_FIGHTER,
    FIGTHER_ACTION,
    CHECK_FOR_VICTORY,
    NEXT_TURN
}


public class CombatManager : MonoBehaviour{
    private Fighter[] fighters;
    private int currentFighterIndex;
    private int turno = 1; // Variable para depuración no hace nada en el estado del juego
    private bool isCombatActive = false;
    private CombatStatus combatStatus;
    private Action currentFigtherAction;

    public int selectedEnemyIndex = 0;  // Índice del enemigo seleccionado

    public int EnemySelect, PlayerSelect;

    public GameObject enemies, players;

    public GameObject targetArrowPrefab; // Prefab de la flecha para indicar el objetivo
    private GameObject currentTargetArrow; // Instancia de la flecha


    void Start(){

        Debug.Log("El combate ha iniciado");
        // Busca a los peleadores en la escena
        this.fighters = FindObjectsOfType<Fighter>();
        foreach (var fighter in this.fighters)
        {
            fighter.combatManager = this;
        }
        // Establece el estado del combate
        this.combatStatus = CombatStatus.NEXT_TURN;
        // Asigna el turno al primer peleador basado en la velocidad
        SortFightersBySpeed(); 
        this.currentFighterIndex = -1;
        this.isCombatActive = true;
        StartCoroutine(CombatLoop());
    }

void SortFightersBySpeed(){

    // Ordenar los Personajes por velocidad
    System.Array.Sort(this.fighters, (fighter1, fighter2) => {

        int speedComparison = fighter2.stats.speed.CompareTo(fighter1.stats.speed);

        // Si las velocidades son iguales, decidir aleatoriamente
        if (speedComparison == 0)
        {
            return Random.Range(0, 2) == 0 ? 1 : -1; // Devuelve 1 o -1 aleatoriamente
        }

        return speedComparison; // Retorna la comparación de velocidad
    });
}


    IEnumerator CombatLoop(){
        
        while (this.isCombatActive){

            switch (this.combatStatus){

                case CombatStatus.WAITING_FOR_FIGHTER:
                yield return null;
                break;
                
                case CombatStatus.FIGTHER_ACTION:
                    //Aqui deberia estar el actualizar el turno en la interfaz de usuario
                    yield return null;
                
                    // Ejecutar la habilidad del personaje
                    currentFigtherAction.Run();

                    // Espera la animación del personaje
                    yield return new WaitForSeconds(currentFigtherAction.animationDuration);
                    this.combatStatus = CombatStatus.CHECK_FOR_VICTORY;

                    currentFigtherAction = null;
                    break;

                case CombatStatus.CHECK_FOR_VICTORY:
                // Verificar si todos los personajes de un equipo están muertos
                if (IsTeamDefeated(Team.Player)) {
                        Debug.Log("El equipo del jugador ha sido derrotado. El combate ha terminado.");
                        this.isCombatActive = false;
                    } else if (IsTeamDefeated(Team.Enemy)) {
                        Debug.Log("El equipo enemigo ha sido derrotado. ¡Victoria!");
                        this.isCombatActive = false;
                    } else {
                        this.combatStatus = CombatStatus.NEXT_TURN;
                    }

                    yield return null;

                    break;
                case CombatStatus.NEXT_TURN:
                yield return new WaitForSeconds(1f);

                // Buscar al siguiente peleador vivo
                do {
                    this.currentFighterIndex = (this.currentFighterIndex + 1) % this.fighters.Length;
                } while (!this.fighters[this.currentFighterIndex].isAlive);

                var currentTurn = this.fighters[this.currentFighterIndex];
                Debug.Log("El turno es de " + currentTurn.fighterName + " en el turno " + turno);
                turno++; // No hace nada fin de depuración
                currentTurn.InitTurn();

                this.combatStatus = CombatStatus.WAITING_FOR_FIGHTER;
                break;

            }
            
        }
    }

    // Método para obtener el peleador opuesto al actual en el turno - IA
    public Fighter GetOppositeFighter() {
        foreach (var fighter in this.fighters) {
            if (fighter.team != this.fighters[this.currentFighterIndex].team && fighter.isAlive) {
                return fighter;
            }
        }
        return null; // Retorna null si no hay enemigos vivos
    }

    public void OnFighterAction(Action action){
        this.currentFigtherAction = action;
        this.combatStatus = CombatStatus.FIGTHER_ACTION;
    }
/*
    public void PlayerTurn(Fighter player, Action action) {
        
        var enemies = System.Array.FindAll(this.fighters, f => f.team == Team.Enemy && f.isAlive);

        if (enemies.Length == 0) return;  // Si no hay enemigos vivos, no hacer nada

        StartCoroutine(HandleTargetSelection(player, action, enemies));
    }*/

public bool IsTeamDefeated(Team team) {
    // Busca en todos los peleadores y verifica si algún miembro del equipo sigue vivo
    foreach (var fighter in this.fighters) {
        if (fighter.team == team && fighter.isAlive) {
            return false; // Si algún miembro del equipo está vivo, el equipo no está derrotado
        }
    }
    return true; // Si todos los miembros están muertos, el equipo está derrotado
}

    // Método para seleccionar un objetivo enemigo con las teclas de flecha y Enter
IEnumerator HandleTargetSelection(Fighter player, Action action, Fighter[] enemies) {
    selectedEnemyIndex = 0;  // Inicia en el primer enemigo
    HighlightTarget(enemies[selectedEnemyIndex]);  // Resalta el primer objetivo seleccionado

    // Instanciar la flecha sobre el enemigo seleccionado
    if (currentTargetArrow == null) {
        currentTargetArrow = Instantiate(targetArrowPrefab);
    }

    // Posicionar la flecha sobre el enemigo seleccionado
    MoveArrowToTarget(enemies[selectedEnemyIndex]);

    while (true) {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            selectedEnemyIndex = (selectedEnemyIndex - 1 + enemies.Length) % enemies.Length;  // Mover hacia arriba
            HighlightTarget(enemies[selectedEnemyIndex]);
            MoveArrowToTarget(enemies[selectedEnemyIndex]);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            selectedEnemyIndex = (selectedEnemyIndex + 1) % enemies.Length;  // Mover hacia abajo
            HighlightTarget(enemies[selectedEnemyIndex]);
            MoveArrowToTarget(enemies[selectedEnemyIndex]);
        } else if (Input.GetKeyDown(KeyCode.Return)) {  // Seleccionar con Enter
            action.SetEmmiterAndReceiver(player, enemies[selectedEnemyIndex]);

            // Añadir lógica para activar las animaciones correctas según el tipo de acción
            if (action.isTeamAction) {
                // Si es una acción de equipo (curación), activar la animación de curación
                player.GetComponent<Animator>().SetTrigger("Heal");
            } else {
                // Si es una acción de ataque, activar la animación de ataque
                player.GetComponent<Animator>().SetTrigger("Attack");
            }

            OnFighterAction(action);

            // Destruir la flecha al confirmar la selección
            Destroy(currentTargetArrow);
            currentTargetArrow = null;
            break;
        }

        yield return null;
    }
}

// Actualizar el método para decidir los objetivos
public void PlayerTurn(Fighter player, Action action) {
    Fighter[] possibleTargets;
    
    if (action.isTeamAction) {
        // Si es una acción de equipo, permite seleccionar entre aliados
        possibleTargets = System.Array.FindAll(this.fighters, f => f.team == player.team && f.isAlive);
    } else {
        // Si es una acción normal, seleccionar enemigos
        possibleTargets = System.Array.FindAll(this.fighters, f => f.team != player.team && f.isAlive);
    }

    if (possibleTargets.Length == 0) return;  // Si no hay objetivos vivos, no hacer nada

    StartCoroutine(HandleTargetSelection(player, action, possibleTargets));
}



    void MoveArrowToTarget(Fighter target) {
        if (currentTargetArrow != null) {
        // Actualiza la posición de la flecha para que siga al enemigo
        currentTargetArrow.transform.position = target.transform.position + new Vector3(-3f, 6.5f, 0); // Ajusta el valor '2.0f' para elevar la flecha sobre el enemigo
        }
    }



    // Método para resaltar el objetivo actual (puedes personalizar el resaltado)
    void HighlightTarget(Fighter target) {
        Debug.Log("Objetivo seleccionado: " + target.fighterName);
        //  Añadir efectos visuales, como cambiar el color del enemigo, para que el jugador vea qué enemigo está seleccionado o agregar un pregab de flecha.
    }


}
