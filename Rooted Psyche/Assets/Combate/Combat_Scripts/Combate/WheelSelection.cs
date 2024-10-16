using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelSelection : MonoBehaviour
{
    private PlayerInput myInput;
    private InputAction cancelButton;
    private bool rotationComplete = true;
    public Transform targetRotation;
    public static bool lockRotation = false;
    private int cubes = 0;
    private Transform[] cubeTransforms = new Transform[10]; 
    

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform){
            if(child.gameObject.active)
            {
                cubeTransforms[cubes] = child;
                cubes++; 
            }
        }
        for (int i = 0; i<cubes; i++)
        {
            cubeTransforms[i].Rotate(Vector3.up,i*(360f/cubes),Space.Self);
        }
        myInput = GetComponent<PlayerInput>();
        cancelButton = myInput.actions["Cancel"];
    }

    // Update is called once per frame
    void Update()
    {
        if (!lockRotation)
        {
            if (Mathf.Abs(CombatManager.direction.x)>0 && rotationComplete) {
                StartCoroutine(RotateWheel((int)Mathf.Sign(CombatManager.direction.x)));
            }
        }
        else
        {
            if (cancelButton.WasPressedThisFrame() && lockRotation)
            {
                lockRotation = false;
            }
        }
    }

    IEnumerator RotateWheel(int direction) {
        rotationComplete = false;
        targetRotation.Rotate(Vector3.up,direction*(360f/cubes),Space.Self);
        while (!rotationComplete){
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation.rotation, 0.1f);
            if (Mathf.Abs(targetRotation.rotation.x - transform.rotation.x) <0.0001f)
            {
                transform.rotation = targetRotation.rotation;
                break;
            }
            yield return null;
        }
        rotationComplete = true;
        yield return null;
    }

}
