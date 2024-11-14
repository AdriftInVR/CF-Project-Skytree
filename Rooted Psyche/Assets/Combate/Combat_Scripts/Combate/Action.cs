using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    [Header("Base Action")]
    public string actionName;
    public float animationDuration;
    public bool isTeamAction; // Cambiado de selfInflicted a isTeamAction
    public bool selfInflicted;
    public GameObject effectPrefab;
    protected Fighter emitter;
    protected Fighter receiver;

    private void VFX(){
        GameObject go = Instantiate(this.effectPrefab, this.receiver.transform.position, Quaternion.identity);
    }

    public IEnumerator Run(float Duration){
        if (this.isTeamAction){
            // Si es una acción de equipo, puedes aplicarla a aliados o a ti mismo
            this.receiver = this.receiver ?? this.emitter; // Si no hay receptor asignado, usar el emisor
        }
        if (this.selfInflicted){
            this.receiver = this.emitter;
        }
        // Espera la animación del personaje
        yield return new WaitForSeconds(Duration);
        this.VFX();
        this.OnRun();
        yield return new WaitForSeconds(1f);
        CombatManager.combatStatus = CombatStatus.CHECK_FOR_VICTORY;
    }

    public void SetEmitterAndReceiver(Fighter _emitter, Fighter _receiver){
        this.emitter = _emitter;
        this.receiver = _receiver;
    }

    protected abstract void OnRun();
}
