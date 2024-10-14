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

    private void Animate(){
        GameObject go = Instantiate(this.effectPrefab, this.receiver.transform.position, Quaternion.identity);
    }

    public void Run(){
        if (this.isTeamAction){
            // Si es una acción de equipo, puedes aplicarla a aliados o a ti mismo
            this.receiver = this.receiver ?? this.emitter; // Si no hay receptor asignado, usar el emisor
        }
        if (this.selfInflicted){
            this.receiver = this.emitter;
        }
        this.Animate();
        this.OnRun();
    }

    public void SetEmmiterAndReceiver(Fighter _emitter, Fighter _receiver){
        this.emitter = _emitter;
        this.receiver = _receiver;
    }

    protected abstract void OnRun();
}
