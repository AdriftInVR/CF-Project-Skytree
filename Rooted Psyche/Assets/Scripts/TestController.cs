using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestController : MonoBehaviour
{
    Rigidbody rb;
    private bool canJump;
    public float spd, jumpSpd = 50f;

    PlayerInput myInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myInput = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        Vector2 direction2D = myInput.actions["Direction"].ReadValue<Vector2>();
        Vector3 direction3D = new Vector3(direction2D.x,0,direction2D.y);
        direction3D.Normalize();

        rb.AddForce(direction3D  * spd);
    }


    private void OnJump(){
        if(canJump){
            rb.AddForce(new Vector3(0,jumpSpd,0));
            canJump = false;

        }
    }

    private void OnCollisionEnter(Collision col) {
        canJump = true;
    }
}
