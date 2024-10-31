using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrierFollow : MonoBehaviour
{
    Rigidbody rb;
    ExplorationController controller;
    public bool following;
    public Transform target;
    Vector3 targetPos;
    private List<Vector3> positions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<ExplorationController>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Multiplayer
        // Following = false
        var v = rb.velocity;
        targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        // Check if the velocity vector is greater than the maximum allowed
        if(v.sqrMagnitude > controller.sqrspd){
            // Trunk the excess speed back to the limit with the normalized vector
            rb.velocity = new Vector3(controller.direction3D.x * controller.spd, rb.velocity.y, controller.direction3D.z * controller.spd);
        } 
        transform.LookAt(targetPos);

        if(controller.direction2D.sqrMagnitude>0f)
        {
            rb.AddForce(transform.forward* controller.spd, ForceMode.Impulse);
        }
        else
        {
            // Apply counter-force to stop instantly
            rb.AddForce(new Vector3(-rb.velocity.x,0,-rb.velocity.z), ForceMode.Impulse);
        }
        
        if(controller.myInput.actions["Brier"].WasPressedThisFrame() && controller.canJump){
            rb.velocity = new Vector3(rb.velocity.x,controller.jumpSpd,rb.velocity.z);
            controller.canJump = false;
        }
    }
}
