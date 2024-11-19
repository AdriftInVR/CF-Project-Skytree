using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public GameObject DefeatEffect;
    void Awake()
    {
        this.fighterName = gameObject.name;
        this.stats = new Stats(100, 100, 10, 10, 5, 1, 10);
        // HP, MaxHP, Atk, Def, Spd, Lv, SP, Multiplier, ATKMult
    }
    public override void InitTurn()
    {
        CombatManager.playerTurn = false;
        PlayerController.locked = false;
        StartCoroutine(IA());
    }

    IEnumerator IA(){
        yield return new WaitForSeconds(1f);
        Action action = actions[Random.Range(0, actions.Length)];
        if(action.isTeamAction)
        {
            action.SetEmitterAndReceiver(this, combatManager.GetTeamFighter(Team.Enemy));
        }
        else
        {
            action.SetEmitterAndReceiver(this, combatManager.GetOppositeFighter(Team.Enemy));
        }
        combatManager.OnFighterAction(action);
        Debug.Log("El enemigo hizo " + action.actionName);
    }

    public override IEnumerator Die()
    {
        isDying = true;
        Destroy(gameObject,0.8f);
        yield return new WaitForSeconds(0.5f);
        GameObject explode = Instantiate(DefeatEffect, transform.position, Quaternion.identity);
    }
}
