using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Brier : Fighter {
    [Header("UI")]
    public PlayerActionPanel actionPanel;
    public TextMeshProUGUI healthText;
    public GameObject ActionWheel;

    void Awake()
    {
        _ActionWheel = ActionWheel;
        this.stats = new Stats(100, 100, 10, 10, 50, 1, 10, 1.0f, 1.0f);
        // HP, MaxHP, Atk, Def, Spd, Lv, SP, Multiplier, ATKMult
        UpdateHealthUI(); // Inicializa la UI de vida con los valores actuales
    }

    private void Update() {
        UpdateHealthUI();
    }

    public override void InitTurn()
    {
        CombatManager.playerTurn = true;
        WheelSelection.lockedRotation = false;
        Vector3 offset = this.transform.position + new Vector3(0f,17.5f,7.5f);
        _ActionWheel = Instantiate(ActionWheel, offset, ActionWheel.transform.rotation);
    }

    // Método llamado cuando se elige una acción
    public void Act(int index)
    {
        StartCoroutine(Action(index));
    }

    IEnumerator Action(int index){
        this.actionPanel.Hide();
        Action action = this.actions[index];
        this.combatManager.PlayerTurn(this, action);
        yield return null;
        //animator.SetTrigger("esto lo cambiamos por la animacion de pensar- solo agregas el trigger en el animator");
    }

    // Actualiza la interfaz cuando la vida cambie
    public new void ModifyHealth(int amount) {
        base.ModifyHealth(amount);
        UpdateHealthUI();
    }

    void UpdateHealthUI() {
        healthText.text = this.stats.health.ToString();
    }

    public override IEnumerator Die()
    {
        //TODO: DeathAnimation
        yield return new WaitForSeconds(1f);
    }
}

