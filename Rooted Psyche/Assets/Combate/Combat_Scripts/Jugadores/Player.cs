using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player: Fighter{
    [Header("UI")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI specialText;
    public GameObject ActionWheel;
    public Action currentAction;
    public int lastCube;
    private PlayerController PC;
    private Rigidbody RB;

    [SerializeField]
    public List<Action> SoloActions = new List<Action>();
    
    [SerializeField]
    public List<Action> DuoActions = new List<Action>();

    void Awake()
    {
        // HP, MaxHP, Atk, Def, Spd, Lv, SP
        UpdateHealthUI(); // Inicializa la UI de vida con los valores actuales
        UpdateSpecialUI(); // Inicializa la UI de vida con los valores actuales
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

    public new void ModifySpecial(int cost) {
        base.ModifySpecial(cost);
        UpdateSpecialUI(); // Actualiza la UI de vida
    }

    // Método para actualizar solo la vida en la interfaz
    void UpdateHealthUI() {
        healthText.text = this.stats.health.ToString();
    }
    
    void UpdateSpecialUI() {
        specialText.text = this.stats.special.ToString();
    }

    protected override void GetActions()
    {
        foreach(Transform child in ActionParent)
        {
            if(child.gameObject.activeSelf)
            {
                Action act = child.gameObject.GetComponent<Action>();
                switch(act.actType)
                {
                    case ActionType.SoloAction:
                        SoloActions.Add(act);
                        break;
                    case ActionType.DuoAction:
                        DuoActions.Add(act);
                        break;
                    default:
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
