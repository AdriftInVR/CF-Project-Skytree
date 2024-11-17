using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Fighter
{
    public GameObject DefeatEffect;
    //     1. health;
    //     2. MaxHealth;
    //     3. attack;
    //     4. defense;
    //     5. speed;
    //     6. level;
    //     7. Mana;
    //     8. AttackMultiplier; (float)
    //     9. specialAttackMultiplier; (float)
    void Awake()
    {
        RunHandle.canRun = false;
        this.stats = new Stats(250, 250, 100, 10, 100, 1, 10);
        // HP, MaxHP, Atk, Def, Spd, Lv, SP
    }
    public override void InitTurn()
    {
        CombatManager.playerTurn = false;
        PlayerController.locked = false;
        StartCoroutine(this.IA());   
    }

    IEnumerator IA(){
        yield return new WaitForSeconds(1f);
        Action action = this.actions[Random.Range(0, this.actions.Length)];
        action.SetEmitterAndReceiver(this, this.combatManager.GetOppositeFighter(Team.Enemy));
        this.combatManager.OnFighterAction(action);
        Debug.Log("El enemigo hizo " + action.actionName);
    }

    public override IEnumerator Die()
    {
        Destroy(gameObject,10f);
        yield return new WaitForSeconds(1f);
        GameObject explode = Instantiate(DefeatEffect, transform.position, Quaternion.identity);
    }

    
}
