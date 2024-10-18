using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoActionCube : CubeHandler
{
    // Start is called before the first frame update

    void Update()
    {
        // Ensures Cube looks towards the camera
        transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, 0, 0);
    }

    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            string name = other.gameObject.name;
            if (Actions.Length == 1)
            {
                switch(name)
                {
                    case "Timber":
                        other.gameObject.GetComponent<Timber>().Act(0);
                        break;
                    case "Brier":
                        other.gameObject.GetComponent<Brier>().Act(0);
                        break;
                }
            }
        }
    }
}
