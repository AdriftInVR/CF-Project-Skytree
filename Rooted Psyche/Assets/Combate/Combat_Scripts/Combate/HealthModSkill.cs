using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum HealthModType{
    STAT_BASED, FIXED
}
public class HealthModAction : Action
{
    [Header("Health Mod")]

    public int amount;
    public GameObject damageFloat;
    public HealthModType modType;

    protected override void OnRun(){
        int amount = this.GetModification();
        Debug.Log("Amount " + amount);
        this.receiver.ModifyHealth(amount);
        ShowDamageText(amount);
    }

    public int GetModification(){
        switch (this.modType){
            case HealthModType.STAT_BASED:
            // En caso de que el ataque sea basado en stats se obtienen los stats actuales del emisor y receptor
            Stats emitterStats = this.emitter.GetCurrentStats();
            Stats receiverStats = this.receiver.GetCurrentStats();
            //Formula de daño basado en stats
            float amount = (emitterStats.level * emitterStats.attack) / (receiverStats.level * receiverStats.defense);
                return Mathf.FloorToInt(amount*-1);
            case HealthModType.FIXED:
                return this.amount;
        }
        throw new System.InvalidOperationException("HelthModAction::GetDamage(). Unrecheable!");
    }

        void ShowDamageText(int damageAmount) {
            var go = Instantiate(damageFloat, this.receiver.transform.position + damageFloat.transform.position, Quaternion.identity);
            switch(damageAmount)
            {
                case > 0:
                    go.GetComponent<TextMeshPro>().text = damageAmount.ToString();
                    break;
                default:
                    go.GetComponent<TextMeshPro>().text = damageAmount.ToString().Substring(1);
                    break;
            }
            
        }
    
/*        void ShowDamageText(int damageAmount) {
        Debug.Log("Amount que llego" + damageAmount);
        // Aqui se Instancia el prefab en la posición del receptor par que el texto salga a flote
        GameObject damageTextObj = Instantiate(damageFloat, this.receiver.transform.position, Quaternion.identity);
        DamageText damageText = damageTextObj.GetComponent<DamageText>();
        
        if (damageText != null) {
            damageText.SetDamage(damageAmount);  // Asigna el valor de daño
            damageText.ShowAndDestroy();         // Llama al método para mostrar y destruir el texto
        }
    }*/
}
