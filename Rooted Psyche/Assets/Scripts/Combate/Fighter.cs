using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    public string fighterName;
    public CombatManager combatManager;
    public Stats stats;
/*
    Estadisticas iniciales de los personajes en el UI de los jugadores
    protected virtual void Start()
    {
        this.statusPanel.SetStats(this.stats);
    }
*/
    public void ModifyHealth(int amount)
    {
        // Se asegura de que la vida no sea menor a 0 ni mayor a la vida maxima
        this.stats.health = (int)Mathf.Clamp(this.stats.health + amount, 0f, this.stats.MaxHealth);
        /*
        Aqui deberia ir la barra de vida que se actualiza en la interfaz de usuario
        this.statusPanel.SetHealth(this.stats.health);
        */
        
    }
    public abstract void InitTurn();

}
