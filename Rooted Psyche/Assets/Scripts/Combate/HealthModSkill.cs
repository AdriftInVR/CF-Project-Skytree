using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealthModType{
    STAT_BASED, FIXED
}
public class HealthModSkill : Skill
{
    [Header("Health Mod")]

    public int amount;
    public HealthModType modType;

    protected override void OnRun(){
        int amount = this.GetModification();
        this.reciver.ModifyHealth(amount);
    }

    public int GetModification(){
        switch (this.modType){
            case HealthModType.STAT_BASED:
            Stats emitterStats = this.emitter.GetCurrentStats();
            Stats receiverStats = this.reciver.GetCurrentStats();
            //Formula de daño basado en stats
            float amount = (emitterStats.level * emitterStats.attack) / (receiverStats.level * receiverStats.defense) * emitterStats.AttackMultiplier * emitterStats.specialAttackMultiplier;
                return Mathf.FloorToInt(this.amount);
            case HealthModType.FIXED:
                return this.amount;
        }
        throw new System.InvalidOperationException("HelthModSkill::GetDamage(). Unrecheable!");
    }
}
