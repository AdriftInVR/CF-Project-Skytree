using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public bool type;
    void Awake()
    {
        this.stats = new Stats(100, 100, 10, 10, 5, 1, 10, 1.0f, 1.0f);
    }
    public override void InitTurn()
    {
     StartCoroutine(this.IA());   
     StartCoroutine(AnimAttack());
    }

    IEnumerator IA(){
        yield return new WaitForSeconds(1.0f);
        Action action = this.actions[Random.Range(0, this.actions.Length)];
        action.SetEmmiterAndReceiver(this, this.combatManager.GetOppositeFighter());
        this.combatManager.OnFighterAction(action);
        Debug.Log("El enemigo hizo : " + action.actionName);
    }

    //Animacion de ataque del  enemigo 
    IEnumerator AnimAttack(){
        float mov = 0.9f;
        if (!type) mov *= -4;
        transform.position = new Vector3(transform.position.x + mov, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(0.2f);
        transform.position = new Vector3(transform.position.x - mov, transform.position.y, transform.position.z);
    }
    
}
