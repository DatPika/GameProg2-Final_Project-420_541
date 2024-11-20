using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    private Vector3 targetPosition;
    private Vector3 targetDirection;
    public float moveSpeed = 10.0f;
    public float turnSpeed = 10.0f;

    // Update is called once per frame
    void Update() 
    {
        // Define the desired target position and direction
        targetPosition = new Vector3(player.position.x, player.position.y + 2.5f, player.position.z);
        // targetDirection = player.rotation;

        // Step to adjust to time scale (frame independent)
        var moveStep = moveSpeed * Time.deltaTime;
        var turnStep = turnSpeed * Time.deltaTime;
        
        // Making sure the camera follows the target
        // We don't want the camera too close
        if (Vector3.Distance(transform.position, targetPosition) > 5.0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveStep);
        }

        // Making sure the camera rotates to the player
        // transform.rotation = Vector3.RotateTowards(transform.rotation, targetDirection, turnStep, 0.0f);
    }
}
