using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealthModType{
    STAT_BASED, FIXED
}
public class HealthModAction : Action
{
    [Header("Health Mod")]

    public int amount;
    public HealthModType modType;

    protected override void OnRun(){
        int amount = this.GetModification();
        this.receiver.ModifyHealth(amount);
    }

    public int GetModification(){
        switch (this.modType){
            case HealthModType.STAT_BASED:
            // En caso de que el ataque sea basado en stats se obtienen los stats actuales del emisor y receptor
            Stats emitterStats = this.emitter.GetCurrentStats();
            Stats receiverStats = this.receiver.GetCurrentStats();
            //Formula de daño basado en stats
            float amount = (emitterStats.level * emitterStats.attack) / (receiverStats.level * receiverStats.defense);
                return Mathf.FloorToInt(this.amount);
            case HealthModType.FIXED:
                return this.amount;
        }
        throw new System.InvalidOperationException("HelthModAction::GetDamage(). Unrecheable!");
    }
}
