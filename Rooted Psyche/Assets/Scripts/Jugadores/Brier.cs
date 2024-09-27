using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Brier : Fighter{
    [Header("UI")]
    public PlayerSkillPanel skillPanel;
    public TextMeshProUGUI healthText;
    
    private Animator animator; // Referencia al Animator

    void Awake()
    {
        this.stats = new Stats(100, 100, 10, 10, 50, 1, 10, 1.0f, 1.0f);
        this.animator = GetComponent<Animator>(); // Obtén el Animator
        UpdateHealthUI(); // Inicializa la UI de vida con los valores actuales
    }

    private void Update() {
        UpdateHealthUI();
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
    // Actualiza la interfaz cuando la vida cambie
    public void ModifyHealth(int amount) {
        base.ModifyHealth(amount);
        UpdateHealthUI(); // Actualiza la UI de vida
    }


    // Método para actualizar solo la vida en la interfaz
    void UpdateHealthUI() {
        healthText.text = this.stats.health.ToString();
    }



}
