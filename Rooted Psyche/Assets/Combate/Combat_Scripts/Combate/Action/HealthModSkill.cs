using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class HealthModAction : Action
{
    protected override IEnumerator OnRun(){
        emitter.ModifySpecial(cost); 
        VFX();
        ShowDamageText("Attacker");
        complete = true;
        yield return null;
    }

    
    void VFX(){
        GameObject go = Instantiate(this.effectPrefab, this.receiver.transform.position, Quaternion.identity);
        CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
    }
}
