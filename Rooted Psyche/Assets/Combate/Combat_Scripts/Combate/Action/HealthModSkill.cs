using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModAction : Action
{
    protected override IEnumerator OnRun(){
        VFX();
        ShowDamageText("Attacker");
        complete = true;
        yield return null;
    }

    
    void VFX(){
        GameObject go = Instantiate(this.effectPrefab, this.receiver.transform.position, Quaternion.identity);
    }
}
