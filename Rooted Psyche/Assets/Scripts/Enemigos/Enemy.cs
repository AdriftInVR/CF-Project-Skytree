using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    void Awake()
    {
        this.stats = new Stats(100, 100, 10, 10, 5, 1, 10, 1.0f, 1.0f);
    }
    public override void InitTurn()
    {
     StartCoroutine(this.IA());   
    }

    IEnumerator IA(){
        yield return new WaitForSeconds(1.0f);
        Skill skill = this.skills[Random.Range(0, this.skills.Length)];
        skill.SetEmmiterAndReciver(this, this.combatManager.GetOppositeFighter());
        this.combatManager.OnFighterSkill(skill);
        Debug.Log("El enemigo hizo : " + skill.skillName);
    }
}
