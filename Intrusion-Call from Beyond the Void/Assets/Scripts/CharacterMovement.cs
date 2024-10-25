using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Vector3 playerVelocity;
    Vector3 move;

    public float walkSpeed = 5;
    public float runSpeed = 8; 
    public float jumpHeight = 1;
    public float gravity = -9.18f;
    public bool isGrounded;
    public bool isRunning;
    private CharacterController controller;
    private Animator animator;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        isGrounded = controller.isGrounded;
        if (animator.applyRootMotion == false)
        {
            ProcessMovement();
        }
        ProcessGravity();
    }
  
    void ProcessMovement()
    { 
        // WASD
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // Turns the player towards the direction they're heading
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        isRunning = Input.GetButton("Run");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y =  Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        // Moves the player
        controller.Move(move * Time.deltaTime * ((isRunning)?runSpeed:walkSpeed));
    }
    
    public void ProcessGravity()
    {
        // To ensure the player 'sticks' to the ground
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        velocity.y = playerVelocity.y * Time.deltaTime;

        controller.Move(velocity);
    }

    public float GetMoveSpeed()
    {
        // TODO: Check if removing 2nd condition breaks game loop
        if (isRunning && (move != Vector3.zero))// Left shift
        {
            return runSpeed;
        }
        else if (move != Vector3.zero)
        {
            return walkSpeed;
        }
        else 
        {
            return 0f;
        }
    }
}