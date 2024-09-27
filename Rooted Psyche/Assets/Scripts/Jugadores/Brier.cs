using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brier : Fighter{
    [Header("UI")]
    public PlayerSkillPanel skillPanel;
    
    private Animator animator; // Referencia al Animator

    void Awake()
    {
        this.stats = new Stats(100, 100, 10, 10, 50, 1, 10, 1.0f, 1.0f);
        this.animator = GetComponent<Animator>(); // Obtén el Animator
    }

    public override void InitTurn(){

        this.skillPanel.Show();
        for (int i = 0; i < this.skills.Length; i++){
            this.skillPanel.ConfigureButtons(i, this.skills[i].skillName);
        }
    }

    public void ExecuteSkill(int index){
        this.skillPanel.Hide();
        Skill skill = this.skills[index];

        skill.SetEmmiterAndReciver(this, this.combatManager.GetOppositeFighter());

        // Llama a la animación de ataque
        animator.SetTrigger("Attack");

        // Después de ejecutar la animación, realiza la habilidad
        this.combatManager.OnFighterSkill(skill);

        Debug.Log("Ejecutando habilidad: " + skill.skillName);
    }
}
