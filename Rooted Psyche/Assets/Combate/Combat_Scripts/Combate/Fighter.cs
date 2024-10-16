using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum Team {
        Player,
        Enemy
    }
public abstract class Fighter : MonoBehaviour{

    public Team team;
    
    public string fighterName;
    public CombatManager combatManager;
    public Stats stats;

    public Animator anim;

    protected Action[] actions;

    public bool isAlive{
        get => this.stats.health > 0;
    }

   // Estadisticas iniciales de los personajes en el UI de los jugadores
    protected virtual void Start(){
        this.actions = this.GetComponentsInChildren<Action>();
    }

    public void ModifyHealth(int amount){
        // Se asegura de que la vida no sea menor a 0 ni mayor a la vida maxima
        this.stats.health = (int)Mathf.Clamp(this.stats.health + amount, 0f, this.stats.MaxHealth);
        if (!isAlive) {
            if (team == Team.Player)
            {
                Quaternion dead = new Quaternion(0,0,90,0);
                this.transform.rotation = dead;
            }
            else
            {
                Explode();
            }
        }
    }

    void Explode(){
        Destroy(gameObject);
    }

    public Stats GetCurrentStats(){
        return this.stats;
    }
    public abstract void InitTurn();
}
