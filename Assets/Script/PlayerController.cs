using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    public Animator animator;

    private int desiredLane = 1;
    public float laneDistance = 4;

    public float JumpForce;
    public float Gravity = -20;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("ANIMATOR: Komponen Animator TIDAK DITEMUKAN!");
        }
        else
        {
            Debug.Log("ANIMATOR: Komponen Animator DITEMUKAN");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("GAME START: " + PlayerManager.isGameStarted);

        if (!PlayerManager.isGameStarted)
        {
            return;
        }

        if (animator == null)
        {
            Debug.LogError("ANIMATOR: Animator masih NULL saat UPDATE!");
            return;
        }

        animator.SetBool("isStarted", true);
        Debug.Log("ANIMATOR: Set isStarted = true");
        direction.z = forwardSpeed;

        // Hanya lompat sekali ketika di ground dan ada swipe up
        if (controller.isGrounded && SwipeManager.swipeup)
        {
            Jump();
            animator.SetTrigger("Jump");
            Debug.Log("ANIMATOR: Trigger Jump diaktifkan");
        }

        // Gravitasi
        if (!controller.isGrounded)
        {
            direction.y += Gravity * Time.deltaTime;
        }

        // Gerak ke samping
        if (SwipeManager.swiperight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }

        if (SwipeManager.swipeleft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        Vector3 targetposition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetposition += Vector3.left * laneDistance;
        } else if (desiredLane == 2) 
        {
            targetposition += Vector3.right * laneDistance;        
        }
       
        if (transform.position == targetposition)
            return;
        Vector3 diff = targetposition - transform.position;
        Vector3 moveDir = diff.normalized*25*Time.deltaTime;
        if (moveDir.sqrMagnitude <  diff.sqrMagnitude) 
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }



    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y =   JumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacles")
        {
            PlayerManager.gameover = true;
            animator.SetTrigger("Die");
            Debug.Log("ANIMATOR: Trigger Die diaktifkan");
        }
    }
}
