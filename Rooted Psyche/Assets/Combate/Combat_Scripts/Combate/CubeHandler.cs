using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    //TODO Add action functionality as a gameobject variable

    // Update is called once per frame
    void Update()
    {
        // Ensures Cube looks towards the camera
        transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, 0, 0);
    }
}
