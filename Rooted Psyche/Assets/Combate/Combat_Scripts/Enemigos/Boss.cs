using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Fighter
{
    public GameObject DefeatEffect;

    void Awake()
    {
        RunHandle.canRun = false;
        GetActions();
    }
    public override void InitTurn()
    {
        CombatManager.playerTurn = false;
        PlayerController.locked = false;
        StartCoroutine(this.IA());   
    }

    IEnumerator IA(){
        yield return new WaitForSeconds(1f);
        Action action = this.actions[Random.Range(0, this.actions.Count)];
        action.SetEmitterAndReceiver(this, this.combatManager.GetOppositeFighter(Team.Enemy));
        this.combatManager.OnFighterAction(action);
        Debug.Log("El enemigo hizo " + action.actionName);
    }

    protected override void GetActions()
    {
        foreach(Transform child in ActionParent)
        {
            if(child.gameObject.activeSelf)
            {
                Action act = child.gameObject.GetComponent<Action>();
                if (act.actType == ActionType.EnemyAction)
                {
                    actions.Add(act);
                }
            }
        }
    }

    public override IEnumerator Die()
    {
        isDying = true;
        Destroy(gameObject, 4.5f);
        Vector3 offset = new Vector3(0,0,-1);
        GameObject explode = Instantiate(DefeatEffect, transform.position+offset, Quaternion.identity);
        yield return null;
    }

    
}
