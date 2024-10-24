using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCube : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        WheelSelection.lockedRotation = true;
        PanelHandler.RunActive();
    }
}
