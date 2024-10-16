using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelSelection : MonoBehaviour
{
    private PlayerInput myInput;
    private bool rotationComplete = true;
    public Transform targetRotation;
    

    // Start is called before the first frame update
    void Start()
    {
        myInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(CombatManager.direction.x)>0 && rotationComplete) {
            StartCoroutine(RotateWheel((int)Mathf.Sign(CombatManager.direction.x)));
        }
    }

    IEnumerator RotateWheel(int direction) {
        rotationComplete = false;
        targetRotation.Rotate(Vector3.up,direction*72f,Space.Self);
        while (!rotationComplete){
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation.rotation, 0.2f);
            if (transform.rotation == targetRotation.rotation)
            {
                break;
            }
            yield return null;
        }
        rotationComplete = true;
        yield return null;
    }

}
