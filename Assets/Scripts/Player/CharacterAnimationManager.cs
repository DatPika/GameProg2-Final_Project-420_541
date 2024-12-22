using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movement;
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
    }

    void Update()
    {
        animator.SetFloat("PlayerSpeed", movement.GetMoveSpeed());
        animator.SetBool("IsFalling", !movement.isGrounded);
        if (Input.GetButtonDown("RunningSlide") && movement.isGrounded)
        {
            animator.SetTrigger("DoRunningSlide");
        }
    }
}
