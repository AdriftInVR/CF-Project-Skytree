using System.Collections;
using UnityEngine;

public class Enemy : Fighter
{
    public GameObject DefeatEffect;

    void Awake()
    {        
        GetActions();
    }

    public override void InitTurn()
    {
        CombatManager.playerTurn = false;
        PlayerController.locked = false;
        StartCoroutine(IA());
    }

    IEnumerator IA()
    {
        yield return new WaitForSeconds(1f);
        Action action = actions[Random.Range(0, actions.Count)];
        if (action.isTeamAction)
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
        Destroy(gameObject,0.6f);
        yield return new WaitForSeconds(0.3f);
        GameObject explode = Instantiate(DefeatEffect, transform.position, Quaternion.identity);
    }
}
