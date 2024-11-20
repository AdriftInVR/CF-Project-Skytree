using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    [Header("Action Info")]
    public HealthModType modType;
    public float amount;
    public int cost;
    public float mult;

    public IEnumerator Run(){     
        this.complete = false;
        if (this.isTeamAction){
            // Si es una acción de equipo, puedes aplicarla a aliados o a ti mismo
            this.receiver = this.receiver ?? this.emitter; // Si no hay receptor asignado, usar el emisor
        }
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
                break;
            case "Counter":
                attacker = this.emitter.GetCurrentStats();
                defender = this.receiver.GetCurrentStats();
                this.damaged = this.emitter;
                break;
        }
        amount = GetModification();
        this.damaged.ModifyHealth(Mathf.FloorToInt(amount));
    }

    public int GetModification(){
        switch (this.modType){
            case HealthModType.STAT_BASED:
            //Formula de daño basado en stats
            amount = this.attacker.level * this.attacker.attack / this.defender.defense * this.mult;
                return Mathf.FloorToInt(amount*-1);
            case HealthModType.FIXED:
                return Mathf.FloorToInt(this.amount);
        }
        throw new System.InvalidOperationException("HealthModAction::GetDamage(). Unreachable!");
    }

    public void SetEmitterAndReceiver(Fighter _emitter, Fighter _receiver){
        this.emitter = _emitter;
        this.receiver = _receiver;
    }

    protected abstract IEnumerator OnRun();
}
