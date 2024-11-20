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
    public Stats stats; // Configurable en el inspectory
    public CombatManager combatManager;
    public Animator anim;
    public Transform ActionParent;
    public GameObject[] modifyIndicators;
    protected List<Action> actions = new List<Action>();
    public bool isAlive{
        get => this.stats.health > 0;
    }
    protected bool isDying = false;

   // Estadisticas iniciales de los personajes en el UI de los jugadores

    void Update()
    {
        if (!isAlive && !isDying) {
            StartCoroutine(Die());
        }
    }

    public void ModifyHealth(int amount){
        // Se asegura de que la vida no sea menor a 0 ni mayor a la vida maxima
        this.stats.health = (int)Mathf.Clamp(this.stats.health + amount, 0f, this.stats.maxHealth);
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

    public void ModifySpecial(int cost)
    {
        // Se asegura de que los puntos especiales no sean menor a 0 ni mayor al maximo
        this.stats.special = (int)Mathf.Clamp(this.stats.special - cost, 0f, this.stats.maxSpecial);
        if (cost<0)
        {
            GameObject go = Instantiate(modifyIndicators[2], transform.position + modifyIndicators[0].transform.position, Quaternion.identity);
            go.GetComponent<TextMeshPro>().text = cost.ToString().Substring(1);
        }
    }

    public Stats GetCurrentStats(){
        return this.stats;
    }

    protected abstract void GetActions();

    public abstract void InitTurn();
    
    public abstract IEnumerator Die();
}
