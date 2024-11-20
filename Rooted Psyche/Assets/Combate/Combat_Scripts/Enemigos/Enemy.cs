using System.Collections;
using UnityEngine;

public class Enemy : Fighter
{
    public GameObject DefeatEffect;

    [Header("Enemy Stats")] 
    public Stats baseStats; // Configurable en el inspector

    private void Awake()
    {
        this.fighterName = gameObject.name;

        // Clona los stats base para evitar modificar los originales.
        this.stats = baseStats.Clone();
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
        Action action = actions[Random.Range(0, actions.Length)];
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

    public override IEnumerator Die()
    {
        Destroy(gameObject, 1.2f);
        yield return new WaitForSeconds(1f);
        Instantiate(DefeatEffect, transform.position, Quaternion.identity);
    }
}
