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
                // cambiar logica para que se verifique que ambos jugadores han perdido la vida - Se puede cambiar a implementar equipos y que se haga un check de los equipos
                    foreach (var fighter in this.fighters){

                        if (fighter.isAlive == false){

                            this.isCombatActive = false;

                            Debug.Log("El combate ha terminado");

                        }

                        else

                        {
                            this.combatStatus = CombatStatus.NEXT_TURN;

                        }

                    }

                    yield return null;

                    break;
                case CombatStatus.NEXT_TURN:

                    yield return new WaitForSeconds(1f);
                    this.currentFighterIndex = (this.currentFighterIndex + 1) % this.fighters.Length;

                    var currentTurn = this.fighters[this.currentFighterIndex];

                    Debug.Log("El turno es de " + currentTurn.fighterName + "En el turno " + turno);

                    turno++; // Aumenta el turno solo con fines de depuración no hace nada en el estado del juego

                    currentTurn.InitTurn();

                    

                    this.combatStatus = CombatStatus.WAITING_FOR_FIGHTER;

                    break;

            }
            
        }
    }

    // Método para obtener el peleador opuesto al actual en el turno - IA
    public Fighter GetOppositeFighter(){
        if (this.currentFighterIndex == 0){
            return this.fighters[1];
        } else {
            return this.fighters[0];
        }
    }

    public void OnFighterAction(Action action){
        this.currentFigtherAction = action;
        this.combatStatus = CombatStatus.FIGTHER_ACTION;
    }
}
