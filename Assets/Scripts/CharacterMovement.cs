using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Vector3 playerVelocity;
    Vector3 move;
    Transform cameraTransform;

    //Rotation variables
    public float smoothTime = 0.05f;
    private float velocity;

    public float walkSpeed = 5.0f;
    public float runSpeed = 8.0f; 
    public float jumpHeight = 0.75f;
    public float gravity = -9.18f;
    public bool isGrounded;
    public bool isRunning;
    private CharacterController controller;
    private Animator animator;
    
    public GameObject MessagePanel;
    // public GameObject Cube;
    // private bool cubePickup = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
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
        // Get movement input relative to the camera
        Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

        // WASD relative to the camera's direction
        move = (cameraForward * Input.GetAxis("Vertical") + cameraRight * Input.GetAxis("Horizontal"));
        
        if (move != Vector3.zero)
        {
            // For smooth rotation
            var targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref velocity, smoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            transform.forward = move;
        }
        // Left Shift
        isRunning = Input.GetButton("Run");
        // Space
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
        if (isRunning)// Left shift
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

    // Methods for showing/hiding MessagePanel
    public void OpenMessagePanel(){
        MessagePanel.SetActive(true);
    }
    public void CloseMessagePanel(){
        MessagePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Cube")){
            OpenMessagePanel();
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Cube")){
            CloseMessagePanel();
        }
    }
}