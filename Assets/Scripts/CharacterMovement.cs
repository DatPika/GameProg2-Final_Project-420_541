using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// imports needed for UI
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    
    // Variable needed for UI + mechanics
    public GameObject MessagePanel;
    public Text MessageText;
    public GameObject obelisk;
    public bool invisibility = false;
    public bool erase = false;
    public bool interaction = true;

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
        InteractWithObelisk();
        
        
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
    public void OpenMessagePanel(string message, float delay)
    {
        // Start a coroutine to open the panel after the delay
        StartCoroutine(OpenPanelAfterDelay(message, delay));
    }

    // Coroutine to handle the delay and open the panel
    private IEnumerator OpenPanelAfterDelay(string message, float delay)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // After the delay, set the message and activate the MessagePanel
        MessageText.text = message;
        MessagePanel.SetActive(true);
    }

    public void CloseMessagePanel(float delay)
    {
        StartCoroutine(ClosePanelAfterDelay(delay));
    }

    // Coroutine to handle the delay
    private IEnumerator ClosePanelAfterDelay(float delay)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // After the delay, close the MessagePanel
        MessagePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Obelisk")){
            OpenMessagePanel("- Press E to interact -", 0f);
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Obelisk")){
            CloseMessagePanel(0f);
        }
    }

    private void InteractWithObelisk(){
        // Getting scene name to decide what ability to grant
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if(MessagePanel.activeSelf){
            Debug.Log("Scene Active...");
            // Level 1
            if(sceneName == "Intro-Hike"){
                Debug.Log("Entering Invisibility Interaction...");
                // If E is pressed while the Message Panel is shown
                //      Invisibility is unlocked, then interactions are blocked.
                //      User invisibility bool in invis. method.
                if(Input.GetKeyDown(KeyCode.E)){
                    invisibility = true;
                    interaction = false;

                    // Disabling obelisk collider to avoid further interaction
                    //      and MessagePanel opening and closing
                    Debug.Log("Disabling Obelisk collider");
                    obelisk.GetComponent<Collider>().enabled = false;

                    // User friendly UI
                    OpenMessagePanel("- Invisibility Obtained -", 0f);
                    CloseMessagePanel(3f);
                    OpenMessagePanel("- Press X to turn Invisible -", 3.5f);
                    CloseMessagePanel(6.5f);
                }
            }

            // Level 2
            if(sceneName == "Scene2"){
                Debug.Log("Entering Erase Interaction...");
                // If E is pressed while the Message Panel is shown
                //      Erase is unlocked, then interactions are blocked.
                //      User erase bool in invis method.
                if(Input.GetKeyDown(KeyCode.E)){
                    erase = true;
                    interaction = false;

                    // Disabling obelisk collider to avoid further interaction
                    //      and MessagePanel opening and closing
                    Debug.Log("Disabling Obelisk collider");
                    obelisk.GetComponent<Collider>().enabled = false;

                    // User friendly UI
                    OpenMessagePanel("- Erase Obtained -", 0f);
                    CloseMessagePanel(3f);
                    OpenMessagePanel("- Press X to execute Erase -", 3.5f);
                    CloseMessagePanel(6.5f);
                }
            }
        }
    }
}