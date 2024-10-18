using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput myInput;
    private static InputAction actionButton;
    private Rigidbody rb;
    public string playerName;
    public static bool locked = false;
    private bool canJump = false;
    // Start is called before the first frame update
    void Start()
    {
        myInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        actionButton = myInput.actions[gameObject.name];    
        if(!locked)
        {
            if(actionButton.WasPressedThisFrame() && canJump && !locked)
            {
                rb.velocity = Vector3.up*50f;
                canJump = false;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        canJump = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ActionCube")
        {
            WheelSelection.lockedRotation = true;
            rb.velocity = -rb.velocity;
        }
    }
}
