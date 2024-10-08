using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Brier : Fighter {
    [Header("UI")]
    public PlayerActionPanel actionPanel;
    public TextMeshProUGUI healthText;
    
    private Animator animator;

    void Awake() {
        this.stats = new Stats(100, 100, 10, 10, 50, 1, 10, 1.0f, 1.0f);
        this.animator = GetComponent<Animator>();
        UpdateHealthUI();
    }

    private void Update() {
        UpdateHealthUI();
    }

    public override void InitTurn() {
        // Mostrar el panel de acciones
        this.actionPanel.Show();
        for (int i = 0; i < this.actions.Length; i++) {
            this.actionPanel.ConfigureButtons(i, this.actions[i].actionName);
        }
    }

    // Método llamado cuando se elige una acción
    public void ExecuteAction(int index) {
        this.actionPanel.Hide();
        Action action = this.actions[index];

        // Iniciar el proceso de selección de objetivo, delegando al CombatManager
        this.combatManager.PlayerTurn(this, action);

        // Llama a la animación de ataque (la animación puede empezar luego de seleccionar el objetivo)
        animator.SetTrigger("Attack");
    }

    public new void ModifyHealth(int amount) {
        base.ModifyHealth(amount);
        UpdateHealthUI();
    }

    void UpdateHealthUI() {
        healthText.text = this.stats.health.ToString();
    }
}

