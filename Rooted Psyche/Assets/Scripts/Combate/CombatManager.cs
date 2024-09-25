using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private Fighter[] fighters;
    private int currentFighterIndex;
    private int turno = 1;
    private bool isCombatActive = false;

    void Start()
    {
        Debug.Log("El combate ha iniciado");
        this.fighters = FindObjectsOfType<Fighter>();
        SortFightersBySpeed(); 
        this.currentFighterIndex = 0;
        this.isCombatActive = true;
        StartCoroutine(CombatLoop());
    }

void SortFightersBySpeed()
{
    // Ordenar los Personajes por velocidad
    System.Array.Sort(this.fighters, (fighter1, fighter2) => 
    {
        int speedComparison = fighter2.stats.speed.CompareTo(fighter1.stats.speed);

        // Si las velocidades son iguales, decidir aleatoriamente
        if (speedComparison == 0)
        {
            return Random.Range(0, 2) == 0 ? 1 : -1; // Devuelve 1 o -1 aleatoriamente
        }

        return speedComparison; // Retorna la comparación de velocidad
    });
}


    IEnumerator CombatLoop()
    {
        while (this.isCombatActive)
        {
            yield return new WaitForSeconds(3f);
            var currentTurn = this.fighters[this.currentFighterIndex];
            Debug.Log("El turno es de " + currentTurn.fighterName + "En el turno " + turno);
            turno++;
            currentTurn.InitTurn();
            this.currentFighterIndex = (this.currentFighterIndex + 1) % this.fighters.Length;
            
            // this.EndTurn(); // Puedes implementar la lógica para terminar el turno aquí
        }
    }
}
