using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour
{


    // Start is called before the first frame update
    void Awake()
    {
        Physics.gravity = new Vector3(0,-20f,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
