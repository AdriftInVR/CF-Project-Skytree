using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player: Fighter{
    [Header("UI")]
    public PlayerActionPanel actionPanel;
    public TextMeshProUGUI healthText;
    public GameObject ActionWheel;
    public Action currentAction;
    public Action[] SoloActions;
    public Action[] DuoActions; 

    void Awake()
    {
        this.stats = new Stats(100, 100, 10, 10, 1, 1, 10);
        // HP, MaxHP, Atk, Def, Spd, Lv, SP
        UpdateHealthUI(); // Inicializa la UI de vida con los valores actuales
    }

    private void Update() {
        UpdateHealthUI();
    }

    public override void InitTurn()
    {
        CombatManager.playerTurn = true;
        WheelSelection.lockedRotation = false;
        StartCoroutine(combatManager.PlayerTurn(this));
    }

    // Método llamado cuando se elige una acción
    public void SoloAction(int index)
    {
        currentAction = SoloActions[index];
    }


    public void DuoAction(int index){
        currentAction = DuoActions[index];
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

    public override IEnumerator Die()
    {
        
        //TODO: DeathAnimation
        yield return new WaitForSeconds(1f);
    }
}
