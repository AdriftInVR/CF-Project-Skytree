using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timber: Fighter{
    [Header("UI")]
    public PlayerActionPanel actionPanel;
    public TextMeshProUGUI healthText;

    void Awake()
    {
        this.stats = new Stats(100, 100, 10, 10, 1, 1, 10, 1.0f, 1.0f);
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

        this.combatManager.PlayerTurn(this, action);
        
         //animator.SetTrigger("esto lo cambiamos por la animacion de pensar- solo agregas el trigger en el animator");
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
