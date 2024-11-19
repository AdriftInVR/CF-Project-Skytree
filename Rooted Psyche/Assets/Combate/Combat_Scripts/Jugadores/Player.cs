using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player: Fighter{
    [Header("UI")]
    public TextMeshProUGUI healthText;
    public GameObject ActionWheel;
    public Action currentAction;
    public int lastCube;
    public Transform ActionParent;
    private PlayerController PC;
    private Rigidbody RB;
    public List<Action> SoloActions = new List<Action>();
    public List<Action> DuoActions = new List<Action>();

    void Awake()
    {
        this.stats = new Stats(100, 100, 40, 10, 1, 5, 10);
        // HP, MaxHP, Atk, Def, Spd, Lv, SP
        UpdateHealthUI(); // Inicializa la UI de vida con los valores actuales
        GetActions();
        PC = GetComponent<PlayerController>();
        RB = GetComponent<Rigidbody>();
    }

    private void Update() {
        UpdateHealthUI();
    }

    public override void InitTurn()
    {
        CombatManager.playerTurn = true;
        PC.canJump = true;
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

    private void GetActions()
    {
        foreach(Transform child in ActionParent)
        {
            if(child.gameObject.activeSelf)
            {
                Action act = child.gameObject.GetComponent<Action>();
                switch(act.actType)
                {
                    default:
                    case ActionType.SoloAction:
                        SoloActions.Add(act);
                        break;
                    case ActionType.DuoAction:
                        DuoActions.Add(act);
                        break;
                }
            }
        }
    }

    public override IEnumerator Die()
    {
        //TODO: DeathAnimation
        yield return new WaitForSeconds(1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            switch(other.gameObject.name)
            {
                default:
                case "Hitbox":
                    combatManager.currentFigtherAction.ShowDamageText("Attacker");
                    combatManager.currentFigtherAction.interrupted = true;
                    PC.canJump = false;
                    break;
                case "Hurtbox": 
                    combatManager.currentFigtherAction.ShowDamageText("Counter");
                    combatManager.currentFigtherAction.interrupted = true;
                    RB.velocity = Vector3.up*30f;
                    break;
            }
        }
    }
}
