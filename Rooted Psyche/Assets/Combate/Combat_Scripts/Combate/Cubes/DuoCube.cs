using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoCube : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        WheelSelection.lockedRotation = true;
        PanelHandler.DuoActive(other.gameObject.GetComponent<Player>());      
    }
}
