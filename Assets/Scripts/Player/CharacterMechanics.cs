using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// imports needed for UI
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMechanics : MonoBehaviour
{
    // Surface object to find the renderer for invisibility
    public GameObject playerSurface;
    public GameObject playerJoints;
    
    // Variable needed for UI + mechanics
    public GameObject MessagePanel;
    public Text MessageText;
    public GameObject obelisk;
    public bool canTurnInvisible = false;
    // public bool canErase = false; //* No time, but would be used for kill mechanic
    public bool interaction = true;
    public bool isInvisible = false;

    Renderer surfaceRend;
    Renderer jointsRend;
    Color originalSurfaceColor;
    Color originalJointsColor;

    // Start is called before the first frame update
    void Start()
    {
        surfaceRend = playerSurface.GetComponent<Renderer>();
        jointsRend = playerJoints.GetComponent<Renderer>();
        originalSurfaceColor = surfaceRend.material.color;
        originalJointsColor = jointsRend.material.color;
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetButtonDown("TurnInvisible"))
        {
            ToggleInvisibility();
        }
        InteractWithObelisk();
    }

    public void ToggleInvisibility()
    {
        // If we have the power, then we are allowed to turn invisible
        if (canTurnInvisible)
        {
            // Toggle invisibility
            isInvisible = !isInvisible;
            if (isInvisible)
            {
                // Have to do for each renderer
                surfaceRend.material.color = Color.gray;
                jointsRend.material.color = Color.gray;
            }
            else
            {
                surfaceRend.material.color = originalSurfaceColor;
                jointsRend.material.color = originalJointsColor;
            }
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
        if(other.CompareTag("Obelisk") && interaction){
            OpenMessagePanel("- Press E to interact -", 0f);
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Obelisk") && interaction){
            CloseMessagePanel(0f);
        }
    }

    private void InteractWithObelisk(){
        // Getting scene name to decide what ability to grant
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if(MessagePanel.activeSelf){
            // Debug.Log("Scene Active...");
            // Level 1
            if(sceneName == "Intro-Hike" || sceneName == "Scene2"){
                // Debug.Log("Entering Invisibility Interaction...");
                // If E is pressed while the Message Panel is shown
                //      Invisibility is unlocked, then interactions are blocked.
                //      User invisibility bool in invis. method.
                if(Input.GetKeyDown(KeyCode.E)){
                    canTurnInvisible = true;
                    interaction = false;

                    // Disabling obelisk collider to avoid further interaction
                    //      and MessagePanel opening and closing
                    // Debug.Log("Disabling Obelisk collider");
                    obelisk.GetComponent<Collider>().enabled = false;

                    // User friendly UI
                    OpenMessagePanel("- Invisibility Obtained -", 0f);
                    CloseMessagePanel(3f);
                    OpenMessagePanel("- Press Q to toggle Invisibility -", 3.5f);
                    CloseMessagePanel(6.5f);
                }
            }

            //* Level 2 ==> Originally we wanted to implement a kill mechanic, but ran out of time
            // if(sceneName == "Scene2"){
            //     // Debug.Log("Entering Erase Interaction...");
            //     // If E is pressed while the Message Panel is shown
            //     //      Erase is unlocked, then interactions are blocked.
            //     //      User erase bool in invis method.
            //     if(Input.GetKeyDown(KeyCode.E)){
            //         canErase = true;
            //         interaction = false;

            //         // Disabling obelisk collider to avoid further interaction
            //         //      and MessagePanel opening and closing
            //         // Debug.Log("Disabling Obelisk collider");
            //         // Collider col = obelisk.GetComponent<Collider>();
            //         // Print the name of the collider and its type
                    

            //         // User friendly UI
            //         OpenMessagePanel("- Erase Obtained -", 0f);
            //         CloseMessagePanel(3f);
            //         OpenMessagePanel("- Press X to execute Erase -", 3.5f);
            //         CloseMessagePanel(6.5f);
            //     }
            // }
        }
    }
}
