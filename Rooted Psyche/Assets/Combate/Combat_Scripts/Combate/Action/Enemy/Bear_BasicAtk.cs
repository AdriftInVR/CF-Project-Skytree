using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear_BasicAtk : Action
{
    //CombatManager.combatStatus = CombatStatus.CHECK_FOR_VICTORY;
    
    Vector3 startPos;
    Vector3 offset;
    GameObject body;
    bool positioned;

    void Awake()
    {
        body = transform.parent.transform.parent.gameObject;
        startPos = body.transform.position;
        offset = new Vector3(25f,0f,0f);
    }

    protected override IEnumerator OnRun(){
        this.positioned = false;
        this.attacked = false;
        this.interrupted = false;
        StartCoroutine(PositionSelf());
        while(!positioned)
        {
            yield return null;
        }
        StartCoroutine(Strike());
        while(!attacked && !interrupted)
        {
            yield return null;
        }
        StartCoroutine(BackToInit());
        while(positioned)
        {
            yield return null;
        }
        this.complete = true;
        yield return null;
    }

    IEnumerator PositionSelf(){
        Vector3 targetPos = this.receiver.transform.position + offset;
        targetPos.y = startPos.y;
        //body.position = targetPos;
        while(!positioned)
        {
            body.transform.position = Vector3.Lerp(body.transform.position, targetPos, 0.3f);
            if (body.transform.position == targetPos)
            {
                positioned = true;
                break;
            }
            yield return null;
        }
        yield return null;
    }

    IEnumerator Strike(){
        body.GetComponent<Rigidbody>().AddForce(body.transform.forward*20f, ForceMode.Impulse);
        while(!interrupted && body.transform.position.x > this.receiver.transform.position.x)
        {
            yield return null;
        }
        body.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    IEnumerator BackToInit(){
        while(positioned)
        {
            body.transform.position = Vector3.Lerp(body.transform.position, startPos, 0.5f);
            if (body.transform.position == startPos)
            {
                positioned = false;
                break;
            }
            yield return null;
        }
        yield return null;
    }
}
