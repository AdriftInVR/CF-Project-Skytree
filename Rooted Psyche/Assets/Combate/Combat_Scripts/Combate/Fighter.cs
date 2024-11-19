using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public GameObject[] modifyIndicators;
    protected Action[] actions;
    public bool isAlive{
        get => this.stats.health > 0;
    }

    protected bool isDying = false;

   // Estadisticas iniciales de los personajes en el UI de los jugadores
    protected virtual void Start(){
        this.actions = this.GetComponentsInChildren<Action>();
    }

    void Update()
    {
        if (!isAlive && !isDying) {
            StartCoroutine(Die());
        }
    }

    public void ModifyHealth(int amount){
        // Se asegura de que la vida no sea menor a 0 ni mayor a la vida maxima
        this.stats.health = (int)Mathf.Clamp(this.stats.health + amount, 0f, this.stats.MaxHealth);
        GameObject go;
        switch(amount)
        {
            case > 0:
                go = Instantiate(modifyIndicators[0], transform.position + modifyIndicators[0].transform.position, Quaternion.identity);
                go.GetComponent<TextMeshPro>().text = amount.ToString();
                break;
            default:
                go = Instantiate(modifyIndicators[1], transform.position + modifyIndicators[1].transform.position, Quaternion.identity);
                go.GetComponent<TextMeshPro>().text = amount.ToString().Substring(1);
                break;
        }   
    }

    public Stats GetCurrentStats(){
        return this.stats;
    }
    public abstract void InitTurn();
    
    public abstract IEnumerator Die();
}
