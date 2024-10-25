using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloCube : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        WheelSelection.lockedRotation = true;
        PanelHandler.SoloActive(other.gameObject.GetComponent<Player>());
    }
}
