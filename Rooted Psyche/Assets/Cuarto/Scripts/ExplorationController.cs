using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplorationController : MonoBehaviour
{
    Rigidbody rb;
    public bool canJump = true;
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private float spd, jumpSpd;

    [SerializeField]
    private int player;
    private string playerName;
    private float sqrspd;
    PlayerInput myInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myInput = GetComponent<PlayerInput>();
        SetMaxVelocity(spd);
        // if multiplayer
        playerName = player==0?"Timber":"Brier";
    }

    void FixedUpdate()
    {
        Vector2 direction2D = myInput.actions["Direction"].ReadValue<Vector2>();
        direction2D.Normalize();

        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        camF.y = 0;
        camR.y = 0;

        Vector3 relF = camF * direction2D.y;
        Vector3 relR = camR * direction2D.x;

        Vector3 moveDir = relF + relR;

        Vector3 direction3D = new Vector3(moveDir.x, 0, moveDir.z);

        var v = rb.velocity;

        // Check if the velocity vector is greater than the maximum allowed
        if(v.sqrMagnitude > sqrspd){
            // Trunk the excess speed back to the limit with the normalized vector
            rb.velocity = new Vector3(moveDir.x * spd, rb.velocity.y, moveDir.z * spd);
        } 

        transform.LookAt(transform.position+direction3D);
        if(player == 0 &&myInput.actions["Direction"].ReadValue<Vector2>().sqrMagnitude > 0f)
        {            
            // Apply force to move instantly
            rb.AddForce(transform.forward * spd, ForceMode.Impulse);
        }
        else
        {
            // Apply counter-force to stop instantly
            rb.AddForce(new Vector3(-rb.velocity.x,0,-rb.velocity.z), ForceMode.Impulse);
        }
        if(myInput.actions[playerName].WasPressedThisFrame() && canJump){
            rb.velocity = new Vector3(rb.velocity.x,jumpSpd,rb.velocity.z);
            canJump = false;
        }
    }


    private void SetMaxVelocity(float maxVelocity)
    {
        this.spd = maxVelocity;
        sqrspd = maxVelocity * maxVelocity;
    }

    private void OnCollisionEnter(Collision col) {
        // Draw a different colored ray for every normal in the collision
        foreach (var contact in col.contacts)
        {
            if(contact.normal.y > 0.9f)
            {
                canJump = true;
            }
        }
    }
}