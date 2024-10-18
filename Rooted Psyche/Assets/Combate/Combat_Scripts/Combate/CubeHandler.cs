using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    //TODO Add action functionality as a gameobject variable
    public Action[] Actions;
    public CombatManager combatManager;
    // Update is called once per frame

    protected Vector3 CamEulers;

    void Update()
    {
        // Ensures Cube looks towards the camera
        CamEulers = Camera.main.transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0, CamEulers.y, 0);
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
