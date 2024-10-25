using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class ExplorationSpriteHandler : MonoBehaviour
{
    bool facingRight = true;
    
    Animator anim;
    PlayerInput myInput;
    Vector2 direction;
    public Rigidbody rb;

    public SpriteLibrary sprites;

    public SpriteLibraryAsset[] directionSprites;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprites = GetComponent<SpriteLibrary>();
        myInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = myInput.actions["Direction"].ReadValue<Vector2>();
        direction.Normalize();

        if(Mathf.Abs(direction.y)>0.5)
        {
            switch(Mathf.Sign(direction.y))
            {
                case 1:
                    sprites.spriteLibraryAsset = directionSprites[2];//Up
                    break;
                case -1:
                    sprites.spriteLibraryAsset = directionSprites[0];//Down
                    break;
                default:
                    break;
            }
        }

        if(Mathf.Abs(direction.x)>Mathf.Abs(direction.y))
        {
            sprites.spriteLibraryAsset = directionSprites[1];//Right
        }



        if(direction.x < -0.1 && facingRight)
        {
            Flip();
        }
        
        if(direction.x > 0.1 && !facingRight)
        {
            Flip();
        }

        if(direction.magnitude > 0.01 && rb.velocity.magnitude>0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 selfScale = transform.localScale;
        selfScale.x *= -1;
        transform.localScale = selfScale;
    }
}
