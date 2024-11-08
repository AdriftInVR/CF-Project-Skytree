using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCube : MonoBehaviour
{
    public Material[] Colors;

    void Awake()
    {
        if (RunHandle.canRun)
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
        if(RunHandle.canRun)
        {
            WheelSelection.lockedRotation = true;
            PanelHandler.RunActive();
        }
    }
}
