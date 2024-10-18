using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlatePosition : MonoBehaviour
{
    private bool slatePositioned = true;
    
    [SerializeField]
    private GameObject[] SP;

    // Update is called once per frame
    void LateUpdate()
    {
        if(slatePositioned)
        {
            if(CombatManager.playerTurn)
            {
                StartCoroutine(MoveSlate(-488));
            }
            else
            {
                StartCoroutine(MoveSlate(-561));
            }
        }
    }

    IEnumerator MoveSlate(int y) {
        foreach(GameObject element in SP)
        {
            element.SetActive(CombatManager.playerTurn);
        }
        slatePositioned = false;
        Vector3 targetPos = new Vector3(this.transform.localPosition.x,y, this.transform.localPosition.z);
        while (!slatePositioned){
            transform.localPosition = Vector3.Lerp(this.transform.localPosition, targetPos, 0.1f);
            if (Mathf.Abs(transform.localPosition.y - targetPos.y)<0.001f || transform.localPosition == targetPos)
            {
                transform.localPosition = targetPos;
                break;
            }
            yield return null;
        }
        slatePositioned = true;
        yield return null;
    }
}
