using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCube : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        WheelSelection.lockedRotation = true;
        PanelHandler.ItemActive();
    }
}
