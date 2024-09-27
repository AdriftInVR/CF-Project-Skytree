using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [Header("Base Skill")]
    public string skillName;
    public float animationDuration;

    public bool selfInflicted;

    public GameObject effectPrefab;

    protected Fighter emitter;
    protected Fighter reciver;

    private void Animate(){
        var go = Instantiate(this.effectPrefab, this.reciver.transform.position, Quaternion.identity);
        Destroy(go, this.animationDuration);
    }

    public void Run(){
        if (this.selfInflicted){
            this.reciver = this.emitter;
        }
        this.Animate();
        this.OnRun();
    }

    public void SetEmmiterAndReciver(Fighter _emitter, Fighter _reciver){
        this.emitter = _emitter;
        this.reciver = _reciver;
    }
    protected abstract void OnRun();
}
