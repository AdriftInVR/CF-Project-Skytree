using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionCube : MonoBehaviour
{
    public Material[] Colors;
    public bool unlocked;

    void Awake()
    {
        if (unlocked)
        {
            gameObject.GetComponent<MeshRenderer>().material = Colors[1];
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = Colors[0];
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(unlocked)
        {
            WheelSelection.lockedRotation = true;
            PanelHandler.FusionActive();
        }
    }
}
