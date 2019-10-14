using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //main parameters
    public float speedMove;
    public float jumpPower;
    public float hitRate;
    public float nextHit = -1f;

    //gameplay parameters
    private float gravityForce;
    private Vector3 moveVector; //character movement direction

    //component links
    private CharacterController ch_controller;
    private Animator ch_animator;
    private MobilePlayerController mob_ch_contr;

    void Start()
    {
        ch_controller = GetComponent<CharacterController>();
        ch_animator = GetComponent<Animator>();
        mob_ch_contr = GameObject.FindGameObjectWithTag("Joystick").GetComponent<MobilePlayerController>();
    }

    void Update()
    {
        CharacterMove();
        GamingGravity();
    }

    private void CharacterMove()
    {
        if (ch_controller.isGrounded)
        {
            //set character movement direction
            moveVector = Vector3.zero;
            moveVector.x = mob_ch_contr.Horizontal() * speedMove; //Input.GetAxis("Horizontal")
            moveVector.z = mob_ch_contr.Vertical() * speedMove; //Input.GetAxis("Vertical")

            //анимация передвижения персонажа при его перемещении
            if (moveVector.x != 0 || moveVector.z != 0)
            {
                ch_animator.SetBool("Move", true);
                float playerSpeed = Mathf.Sqrt(Mathf.Pow(mob_ch_contr.Horizontal(), 2) + Mathf.Pow(mob_ch_contr.Vertical(), 2));
                ch_animator.SetFloat("Speed", playerSpeed);
            }
            else
                ch_animator.SetBool("Move", false);

            //rotation of characters in the direction of movement
            if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
            {
                Vector3 direction = Vector3.RotateTowards(transform.forward, moveVector, speedMove, 0.0f);
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        moveVector.y = gravityForce;
        ch_controller.Move(moveVector * Time.deltaTime);
    }

    private void GamingGravity()
    {
        if (!ch_controller.isGrounded)
            gravityForce -= 20f * Time.deltaTime;
        else
            gravityForce = -1f;

        //CharacterJump()
    }

    public void CharacterJump()
    {
        if (ch_controller.isGrounded) // && Input.GetKeyDown(KeyCode.Space)
        {
            gravityForce = jumpPower;
        }
    }

    public void CharacterHit()
    {
        if(Time.time > nextHit && ch_controller.isGrounded)
        {
            nextHit = Time.time + hitRate;
            Debug.Log("Hit");

            //add fit animation with trigger "Hit"
        }
    }
}
