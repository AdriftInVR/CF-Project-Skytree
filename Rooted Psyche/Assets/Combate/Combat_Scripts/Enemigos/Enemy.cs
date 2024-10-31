using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public GameObject DefeatEffect;
    public bool type;
    void Awake()
    {
        this.stats = new Stats(100, 100, 10, 10, 5, 1, 10);
        // HP, MaxHP, Atk, Def, Spd, Lv, SP, Multiplier, ATKMult
    }
    public override void InitTurn()
    {
        CombatManager.playerTurn = false;
        PlayerController.locked = false;
        StartCoroutine(this.IA());   
        StartCoroutine(AnimAttack());
    }

    IEnumerator IA(){
        yield return new WaitForSeconds(1f);
        Action action = this.actions[Random.Range(0, this.actions.Length)];
        action.SetEmmiterAndReceiver(this, this.combatManager.GetOppositeFighter());
        this.combatManager.OnFighterAction(action);
        Debug.Log("El enemigo hizo " + action.actionName);
    }

    //Animacion de ataque del  enemigo 
    IEnumerator AnimAttack(){
        float mov = 0.9f;
        if (!type)
            mov *= -4;
        transform.position = new Vector3(transform.position.x + mov, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(0.2f);
        transform.position = new Vector3(transform.position.x - mov, transform.position.y, transform.position.z);
        Debug.Log("El enemigo se movio");
    }

    public override IEnumerator Die()
    {
        Destroy(gameObject,1.2f);
        yield return new WaitForSeconds(1f);
        GameObject explode = Instantiate(DefeatEffect, transform.position, Quaternion.identity);
    }
}
