using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput myInput;
    private static InputAction actionButton;
    private Rigidbody rb;
    private Player entity;
    public string inputAction;
    public static bool locked = false;
    public bool canJump = false;
    // Start is called before the first frame update
    void Start()
    {
        myInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        entity = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        actionButton = myInput.actions[inputAction];    
        if(!locked && !CombatManager.menuOpen && entity.isAlive)
        {
            if(actionButton.WasPressedThisFrame() && canJump && !locked)
            {
                rb.velocity = CombatManager.playerTurn? Vector3.up*50f:Vector3.up*60f;
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
            rb.velocity = -rb.velocity;
        }
    }
}
