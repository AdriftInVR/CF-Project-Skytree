using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum HealthModType{
    STAT_BASED, FIXED
}

public enum ActionType{
    SoloAction, DuoAction, EnemyAction
}

public abstract class Action : MonoBehaviour
{
    [Header("Base Action")]
    public string actionName;
    public bool isTeamAction; // Cambiado de selfInflicted a isTeamAction
    public bool selfInflicted;
    public GameObject effectPrefab;
    protected Fighter emitter;
    protected Fighter receiver;
    protected Fighter damaged;
    public ActionType actType;
    public bool interrupted;
    protected bool attacked;
    protected bool complete;
    private Stats attacker;
    private Stats defender;
    
    [Header("Health Mod")]
    public GameObject indicator;
    public HealthModType modType;
    public int amount;
    public int counterAmount;

    public IEnumerator Run(){      
        this.complete = false;
        if (this.isTeamAction){
            // Si es una acción de equipo, puedes aplicarla a aliados o a ti mismo
            this.receiver = this.receiver ?? this.emitter; // Si no hay receptor asignado, usar el emisor
        }
        this.complete = false;
        if (this.selfInflicted){
            this.receiver = this.emitter;
        }
        StartCoroutine(OnRun());
        // Espera la animación del personaje
        do {
            yield return null;
        } while (!this.complete);
        CombatManager.combatStatus = CombatStatus.CHECK_FOR_VICTORY;
    }
    
    public void ShowDamageText(string who) {
        switch(who)
        {
            default:
            case "Attacker":
                attacker = this.emitter.GetCurrentStats();
                defender = this.receiver.GetCurrentStats();
                this.damaged = this.receiver;
                amount = GetModification();
                break;
            case "Counter":
                attacker = this.emitter.GetCurrentStats();
                defender = this.receiver.GetCurrentStats();
                this.damaged = this.emitter;
                amount = GetModification();
                break;
        }
        var go = Instantiate(indicator, this.damaged.transform.position + indicator.transform.position, Quaternion.identity);
        switch(amount)
        {
            case > 0:
                go.GetComponent<TextMeshPro>().text = amount.ToString();
                break;
            default:
                //go.GetComponent<TextMeshPro>().text = damageAmount.ToString().Substring(1);
                go.GetComponent<TextMeshPro>().text = amount.ToString().Substring(1);
                break;
        }   
        this.damaged.ModifyHealth(amount);
    }

    public int GetModification(){
        switch (this.modType){
            case HealthModType.STAT_BASED:
            //Formula de daño basado en stats
            float amount = (attacker.level * attacker.attack) / (defender.level * defender.defense);
                return Mathf.FloorToInt(amount*-1);
            case HealthModType.FIXED:
                return this.amount;
        }
        throw new System.InvalidOperationException("HealthModAction::GetDamage(). Unreachable!");
    }

    public void SetEmitterAndReceiver(Fighter _emitter, Fighter _receiver){
        this.emitter = _emitter;
        this.receiver = _receiver;
    }

    protected abstract IEnumerator OnRun();
}
