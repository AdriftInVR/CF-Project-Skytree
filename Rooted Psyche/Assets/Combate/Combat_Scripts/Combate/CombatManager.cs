using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum CombatStatus{
    WAITING_FOR_FIGHTER,
    FIGTHER_ACTION,
    CHECK_FOR_VICTORY,
    NEXT_TURN,
    RUNNING
}


public class CombatManager : MonoBehaviour{
    private Fighter[] fighters;

    private GameObject[] enemies;
    private GameObject[] players;
    private int currentFighterIndex;
    private int lastEnemyIndex;
    private int lastPlayerIndex;
    private int index;
    private int turno = 1; // Variable para depuración no hace nada en el estado del juego
    public static bool isCombatActive = false;
    
    private bool selectorPositioned = true;
    public static bool playerTurn = true;
    public static bool menuOpen = false;
    public static CombatStatus combatStatus;
    private Action currentFigtherAction;
    public static Vector2 direction;
    public int selectedEnemyIndex = 0;  // Índice del enemigo seleccionado
    public int EnemySelect, PlayerSelect;
    public GameObject[] targetArrows; // Prefab de la flecha para indicar el objetivo
    private GameObject currentTargetArrow; // Instancia de la flecha
    private GameObject Wheel;
    private PlayerInput myInput;
    public static InputAction confirmButton;
    public static InputAction cancelButton;
    public static bool confirm {get; private set;}
    
    public static bool cancel {get; private set;}
    void Start(){
        // Busca a los peleadores en la escena
        fighters = FindObjectsOfType<Fighter>();
        foreach (var fighter in fighters)
        {
            fighter.combatManager = this;
        }
        // Establece el estado del combate
        combatStatus = CombatStatus.NEXT_TURN;
        // Asigna el turno al primer peleador basado en la velocidad
        SortFightersBySpeed(); 
        currentFighterIndex = -1;
        isCombatActive = true;
        StartCoroutine(CombatLoop());
        myInput = GetComponent<PlayerInput>();
        confirmButton = myInput.actions["Timber"];
        cancelButton = myInput.actions["Cancel"];
    }

    void FixedUpdate()
    {
        confirm = confirmButton.WasPressedThisFrame();
        cancel = cancelButton.WasPressedThisFrame();
        if (RunHandle.running)
        {
            combatStatus = CombatStatus.RUNNING;
        }
    }

    void SortFightersBySpeed(){

        // Ordenar los Personajes por velocidad
        System.Array.Sort(fighters, (fighter1, fighter2) => {

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
        
        while (isCombatActive){

            switch (combatStatus){

                case CombatStatus.WAITING_FOR_FIGHTER:
                    yield return null;
                    break;
                
                case CombatStatus.FIGTHER_ACTION:
                    //Aqui deberia estar el actualizar el turno en la interfaz de usuario
                    yield return null;
                
                    // Ejecutar la habilidad del personaje
                    StartCoroutine(currentFigtherAction.Run(currentFigtherAction.animationDuration));
                    currentFigtherAction = null;
                    combatStatus = CombatStatus.WAITING_FOR_FIGHTER;
                    break;

                case CombatStatus.CHECK_FOR_VICTORY:
                    yield return new WaitForSeconds(0.5f);
                    CheckWinLoss();
                    break;
                case CombatStatus.NEXT_TURN:

                    // Buscar al siguiente peleador vivo
                    do {
                        currentFighterIndex = (currentFighterIndex + 1) % fighters.Length;
                    } while (!fighters[currentFighterIndex].isAlive);

                    var currentTurn = fighters[currentFighterIndex];
                    Debug.Log("Turno " + (int)(turno/fighters.Length+1) + " / Es el turno de " + currentTurn.fighterName + ".");
                    turno++; // No hace nada fin de depuración
                    currentTurn.InitTurn();

                    combatStatus = CombatStatus.WAITING_FOR_FIGHTER;
                    yield return null;
                    break;
                case CombatStatus.RUNNING:
                    Destroy(Wheel);
                    yield return null;
                    break;
            }
            
        }
        Debug.Log("El combate ha terminado.");
    }

    void CheckWinLoss()
    {
        bool end = false;
        bool allPlayersDefeated = true;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // Filtrar los personajes del equipo del jugador que aún están vivos y verificar si todos están derrotados
        foreach (var fighter in fighters)
        {
        if (fighter.team == Team.Player && fighter.isAlive)
        {
            allPlayersDefeated = false;
                }
        }
        if (allPlayersDefeated) {
            end = true;
            Debug.Log("El equipo del jugador ha sido derrotado.");
        }
        if (enemies.Length == 0) {
            end = true;
            Debug.Log("El equipo enemigo ha sido derrotado.");
        }
        if (!end) {
            combatStatus = CombatStatus.NEXT_TURN;
        }
        else
        {
            PlayerController.locked = true;
            isCombatActive = false;
            // TODO: Victory Sequence
            SceneManager.LoadScene("Exploracion");
        }
    }

    // Método para obtener el peleador opuesto al actual en el turno - IA
    public Fighter GetOppositeFighter() {
    // Filtrar los personajes del equipo del jugador que aún están vivos
    var playerTeam = System.Array.FindAll(this.fighters, fighter => fighter.team == Team.Player && fighter.isAlive);

    // Si hay personajes del equipo del jugador vivos, selecciona uno aleatoriamente
    if (playerTeam.Length > 0) {
        return playerTeam[Random.Range(0, playerTeam.Length)];
    }

    // Si no hay personajes vivos en el equipo del jugador, retorna null
    return null;
    }

    public void OnFighterAction(Action action){
        currentFigtherAction = action;
        combatStatus = CombatStatus.FIGTHER_ACTION;
    }
    /*
        public void PlayerTurn(Fighter player, Action action) {
            
            var enemies = System.Array.FindAll(fighters, f => f.team == Team.Enemy && f.isAlive);

            if (enemies.Length == 0) return;  // Si no hay enemigos vivos, no hacer nada

            StartCoroutine(HandleTargetSelection(player, action, enemies));
        }*/


    // Método para seleccionar un objetivo enemigo con las teclas de flecha y Enter
    IEnumerator HandleTargetSelection(Player player, Action action, Fighter[] targets) {
        PlayerController.locked = true;
        selectedEnemyIndex = 0;  // Inicia en el primer enemigo
        Vector3 arrowScale = new Vector3(0,0,0);
        selectedEnemyIndex = Mathf.Clamp(0, targets.Length-1, selectedEnemyIndex);
        if (targets[0].team == Team.Player)
        {
            arrowScale = new Vector3(7f,7f,7f);
            selectedEnemyIndex = lastPlayerIndex;
        }
        else
        {
            arrowScale = new Vector3(-7f,7f,7f);
            selectedEnemyIndex = lastEnemyIndex;
        }

        if (targets.Length-1 < selectedEnemyIndex)
        {
            selectedEnemyIndex = 0;
        }

        GameObject currentTargetArrow = Instantiate(targetArrows[index]);
        currentTargetArrow.transform.localScale = arrowScale;

        HighlightTarget(targets[selectedEnemyIndex]);  // Resalta el primer objetivo seleccionado
        SetArrowToTarget(currentTargetArrow, targets[selectedEnemyIndex]);

        // Posicionar la flecha sobre el enemigo seleccionado
        int oldIndex = selectedEnemyIndex; 
        while (!cancelButton.WasPressedThisFrame()) {
            oldIndex = selectedEnemyIndex;
            if (Mathf.Abs(direction.y)>0 && selectorPositioned) {
                selectedEnemyIndex = (selectedEnemyIndex + (int)Mathf.Sign(direction.y) + targets.Length) % targets.Length;  // Mover hacia arriba
                HighlightTarget(targets[selectedEnemyIndex]);
                if (oldIndex != selectedEnemyIndex)
                {
                    StartCoroutine(MoveArrowToTarget(currentTargetArrow, targets[selectedEnemyIndex]));
                }
            }
            if (confirmButton.WasPressedThisFrame()) {  // Seleccionar con Enter
                action.SetEmmiterAndReceiver(player, targets[selectedEnemyIndex]);

                // Añadir lógica para activar las animaciones correctas según el tipo de acción
                if (action.isTeamAction) {
                    // Si es una acción de equipo (curación), activar la animación de curación
                    player.anim.SetTrigger("Heal");
                } else {
                    // Si es una acción de ataque, activar la animación de ataque
                    player.anim.SetTrigger("Attack");
                }

                if (targets[selectedEnemyIndex].team == Team.Player)
                {
                    lastPlayerIndex = selectedEnemyIndex;
                }
                else
                {
                    lastEnemyIndex = selectedEnemyIndex;
                }

                OnFighterAction(action);

                // Destruir la flecha al confirmar la selección
                Destroy(currentTargetArrow);
                Destroy(Wheel);
                player.currentAction = null;
                break;
            }
            yield return null;
        }
        if(cancelButton.WasPressedThisFrame())
        {
            Destroy(currentTargetArrow);
            player.currentAction = null;
            StartCoroutine(PlayerTurn(player));
            PanelHandler.ReopenActive();
            combatStatus = CombatStatus.WAITING_FOR_FIGHTER;
        }
    }

    // Actualizar el método para decidir los objetivos
    public IEnumerator PlayerTurn(Player player) {
        WheelSelection.lastCube = player.lastCube;
        //TODO: if not multiplayer...
        confirmButton = myInput.actions[player.fighterName];
        Vector3 offset = player.transform.position + new Vector3(0f,12.5f,7.5f);
        if(!Wheel)
        {
            PlayerController.locked = true;
            yield return new WaitForSeconds(0.5f);
            Wheel = Instantiate(player.ActionWheel, offset, player.ActionWheel.transform.rotation);
        }
        index = player.fighterName == "Timber" ? 0 : 1;
        Fighter[] possibleTargets;
        while (player.currentAction == null)
        {
            yield return null;
        }
        WheelSelection.lockedRotation = true;
        PanelHandler.ClosePanel();
        if (player.currentAction.isTeamAction) {
            // Si es una acción de equipo, permite seleccionar entre aliados
            possibleTargets = System.Array.FindAll(fighters, f => f.team == player.team && f.isAlive);
        } else {
            // Si es una acción normal, seleccionar enemigos
            possibleTargets = System.Array.FindAll(fighters, f => f.team != player.team && f.isAlive);
        }
        StartCoroutine(HandleTargetSelection(player, player.currentAction, possibleTargets));
        yield return null;
    }

    void SetArrowToTarget(GameObject arrow, Fighter target) {
        Vector3 offset = new Vector3(0f,0f,0f);
        if (target.team == Team.Player)
        {
            offset.x = 5f;
            offset.y = 5f;
        }
        else
        {
            offset.x = -8f;
            offset.y = 9f;
        }
        arrow.transform.position = target.transform.position + offset;
        selectorPositioned = true;
    }

    IEnumerator MoveArrowToTarget(GameObject arrow, Fighter target) {
        Vector3 offset = new Vector3(0,9f,0);
        if (target.team == Team.Player)
        {
            offset.x += 5f;
        }
        else
        {
            offset.x -= 8f;
        }
        Vector3 targetPos = target.transform.position + offset;
        selectorPositioned = false;
        while (!selectorPositioned && arrow != null){
            arrow.transform.position = Vector3.Lerp(arrow.transform.position, targetPos,0.25f);
            if (arrow.transform.position == targetPos)
            {
                break;
            }
            yield return null;
        }
        selectorPositioned = true;
        yield return null;
    }

    // Método para resaltar el objetivo actual (puedes personalizar el resaltado)
    void HighlightTarget(Fighter target) {
        Debug.Log("Objetivo seleccionado: " + target.fighterName);
        //  Añadir efectos visuales, como cambiar el color del enemigo, para que el jugador vea qué enemigo está seleccionado o agregar un pregab de flecha.
    }

    void OnDirection()
    {
        direction = myInput.actions["Direction"].ReadValue<Vector2>();
    }
}
