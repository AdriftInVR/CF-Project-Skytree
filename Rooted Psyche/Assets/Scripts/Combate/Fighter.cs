using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour{
    
    public string fighterName;
    public CombatManager combatManager;
    public Stats stats;

    protected Skill[] skills;

    public bool isAlive{
        get => this.stats.health > 0;
    }
    

   // Estadisticas iniciales de los personajes en el UI de los jugadores
    protected virtual void Start(){
        //this.statusPanel.SetStats(this.stats);
        this.skills = this.GetComponentsInChildren<Skill>();
    }

    public void ModifyHealth(int amount){
        // Se asegura de que la vida no sea menor a 0 ni mayor a la vida maxima
        this.stats.health = (int)Mathf.Clamp(this.stats.health + amount, 0f, this.stats.MaxHealth);
        if (!isAlive) {
            Die();
        }
    }

    void Die(){
        Destroy(gameObject);
    }

    public Stats GetCurrentStats(){
        return this.stats;
    }
    public abstract void InitTurn();

}
