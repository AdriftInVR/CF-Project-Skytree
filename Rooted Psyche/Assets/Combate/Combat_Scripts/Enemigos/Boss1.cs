using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Fighter
{
    public GameObject DefeatEffect;
    public bool type;
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
        this.stats = new Stats(250, 250, 100, 10, 100, 1, 10);
        // HP, MaxHP, Atk, Def, Spd, Lv, SP
    }
    public override void InitTurn()
    {
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
