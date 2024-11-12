using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Player_Move : MonoBehaviour
{
    Rigidbody rb;
    public bool canJump = true;
    [SerializeField] private Transform cam;
    [HideInInspector] public float sqrspd;
    public float spd, jumpSpd;
    [HideInInspector] public PlayerInput myInput;
    [HideInInspector] public Vector2 direction2D;
    [HideInInspector] public Vector3 direction3D;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myInput = GetComponent<PlayerInput>();
        SetMaxVelocity(spd);
        if (GetComponent<PhotonView>().IsMine)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cam = mainCam.transform;
            }
            else
            {
                Debug.LogWarning("No se encontró una cámara principal");
            }
        }
    }

    void FixedUpdate()
    {
        if (GetComponent<PhotonView>().IsMine== true)
        {
            Move();
        }
    }

    void Move(){
        direction2D = myInput.actions["Direction"].ReadValue<Vector2>();
        direction2D.Normalize();

        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        camF.y = 0;
        camR.y = 0;

        Vector3 relF = camF * direction2D.y;
        Vector3 relR = camR * direction2D.x;
        Vector3 moveDir = relF + relR;

        direction3D = direction2D.magnitude>0.5?new Vector3(moveDir.x, 0, moveDir.z):direction3D;
        if(!gameObject.GetComponent<BrierFollow>() || !gameObject.GetComponent<BrierFollow>().following)
        {

            var v = rb.velocity;

            // Check if the velocity vector is greater than the maximum allowed
            if(v.sqrMagnitude > sqrspd){
                // Trunk the excess speed back to the limit with the normalized vector
                rb.velocity = new Vector3(moveDir.x * spd, rb.velocity.y, moveDir.z * spd);
            } 

            transform.LookAt(transform.position+direction3D);
            if(myInput.actions["Direction"].ReadValue<Vector2>().sqrMagnitude > 0f)
            {            
                // Apply force to move instantly
                rb.AddForce(transform.forward * spd, ForceMode.Impulse);
            }
            else
            {
                // Apply counter-force to stop instantly
                rb.AddForce(new Vector3(-rb.velocity.x,0,-rb.velocity.z), ForceMode.Impulse);
            }
            if(myInput.actions["Timber"].WasPressedThisFrame() && canJump){
                rb.velocity = new Vector3(rb.velocity.x,jumpSpd,rb.velocity.z);
                canJump = false;
            }
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
            if(contact.normal.y > 0.5f)
            {
                canJump = true;
            }
        }
    }
}