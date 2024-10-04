using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timber: Fighter{
    [Header("UI")]
    public PlayerActionPanel actionPanel;
    public TextMeshProUGUI healthText;
    
    private Animator animator; // Referencia al Animator

    void Awake()
    {
        this.stats = new Stats(1000, 1000, 10, 10, 50, 1, 10, 1.0f, 1.0f);
        this.animator = GetComponent<Animator>(); // Obtén el Animator
        UpdateHealthUI(); // Inicializa la UI de vida con los valores actuales
    }

    private void Update() {
        UpdateHealthUI();
    }

    public override void InitTurn(){

        this.actionPanel.Show();
        for (int i = 0; i < this.actions.Length; i++){
            this.actionPanel.ConfigureButtons(i, this.actions[i].actionName);
        }
    }

    public void ExecuteAction(int index){
        this.actionPanel.Hide();
        Action action = this.actions[index];

        action.SetEmmiterAndReciver(this, this.combatManager.GetOppositeFighter());

        // Llama a la animación de ataque
        animator.SetTrigger("Attack");

        // Después de ejecutar la animación, realiza la habilidad
        this.combatManager.OnFighterAction(action);

        Debug.Log("Ejecutando habilidad: " + action.actionName);
    }
    // Actualiza la interfaz cuando la vida cambie
    public new void ModifyHealth(int amount) {
        base.ModifyHealth(amount);
        UpdateHealthUI(); // Actualiza la UI de vida
    }


    // Método para actualizar solo la vida en la interfaz
    void UpdateHealthUI() {
        healthText.text = this.stats.health.ToString();
    }

}
