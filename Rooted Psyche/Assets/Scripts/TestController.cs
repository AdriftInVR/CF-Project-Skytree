using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestController : MonoBehaviour
{
    Rigidbody rb;
    public bool canJump = true;

    [SerializeField]
    private float spd, jumpSpd;
    private float sqrspd;
    public float decelerationMultiplier = 0.5f;

    PlayerInput myInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myInput = GetComponent<PlayerInput>();
        SetMaxVelocity(spd);
    }

    void FixedUpdate()
    {
        Vector2 direction2D = myInput.actions["Direction"].ReadValue<Vector2>();
        direction2D.Normalize();
        Vector3 direction3D = new Vector3(direction2D.x, 0, direction2D.y);

        var v = rb.velocity;

        // Check if the velocity vector is greater than the maximum allowed
        if(v.sqrMagnitude > sqrspd){
            // Trunk the excess speed back to the limit with the normalized vector
            rb.velocity = new Vector3(direction2D.x * spd, rb.velocity.y, direction2D.y * spd);
        } 

        if(myInput.actions["Direction"].ReadValue<Vector2>().sqrMagnitude > 0f)
        {            
            // Apply force to move instantly
            rb.AddForce(direction3D * spd, ForceMode.Impulse);
        }
        else
        {
            // Apply counter-force to stop instantly
            rb.AddForce(new Vector3(-rb.velocity.x,0,-rb.velocity.z), ForceMode.Impulse);
        }
    }


    private void OnJump(){
        if(canJump){
            rb.AddForce(new Vector3(0,jumpSpd,0));
            canJump = false;
            Debug.Log(rb.velocity);
        }
    }

    private void SetMaxVelocity(float maxVelocity)
    {
        this.spd = maxVelocity;
        sqrspd = maxVelocity * maxVelocity;
    }

    private void OnCollisionEnter(Collision col) {
        canJump = true;
    }
}
