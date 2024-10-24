using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    public int cubeID;
    private Vector3 CamEulers;
    void Update()
    {
        // Ensures Cube looks towards the camera
        CamEulers = Camera.main.transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0, CamEulers.y, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Player>().lastCube = cubeID;    
    }
}
