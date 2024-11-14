using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class Player_Sprite : MonoBehaviour
{
    bool facingRight = true;
    
    Animator anim;
    PlayerInput myInput;
    Vector2 direction;
    public Rigidbody rb;

    public SpriteLibrary sprites;

    public SpriteLibraryAsset[] directionSprites;
    public Transform PlayerBody;



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
        if(Mathf.Abs(PlayerBody.forward.z)>0.5)
        {
            switch(Mathf.Sign(PlayerBody.forward.z))
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

        if(Mathf.Abs(PlayerBody.forward.x)*1.1f>Mathf.Abs(PlayerBody.forward.z))
        {
            sprites.spriteLibraryAsset = directionSprites[1];//Right
        }



        if(PlayerBody.forward.x < -0.1 && facingRight)
        {
            Flip();
        }
        
        if(PlayerBody.forward.x > 0.1 && !facingRight)
        {
            Flip();
        }

        Vector3 vel = new Vector3(rb.velocity.x,0,rb.velocity.z); 

        if(vel.magnitude>0.3f)
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
