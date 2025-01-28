using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    public Animator animator;
    private bool hasStartedAnimation = false;

    private int desiredLane = 1;
    public float laneDistance = 4;

    public float JumpForce;
    public float Gravity = -20;

    public float NormalHeight = 1.5f; // Tinggi normal
    public float SlideHeight = 1.0f; // Tinggi saat slide
    public Vector3 NormalCenter = new Vector3(0, 0.75f, 0); // Center normal
    public Vector3 SlideCenter = new Vector3(0, 0.5f, 0); // Center saat slide

    private bool isSliding = false;
    public float slideDuration = 1.0f; // Durasi slide
    public float slideTransitionSpeed = 5.0f; // Kecepatan transisi height & center

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

    void Update()
    {
        if (PlayerManager.isGameStarted && !PlayerManager.gameover)
        {
            if (forwardSpeed < maxSpeed)
                forwardSpeed += 0.1f * Time.deltaTime;

            direction.z = forwardSpeed;

            if (!hasStartedAnimation)
            {
                animator.SetBool("isStarted", true); // Aktifkan animasi
                Debug.Log("ANIMATOR: isStarted diaktifkan");
                hasStartedAnimation = true; // Hindari pemanggilan ulang
            }

            // Hanya lompat sekali ketika di ground dan ada swipe up
            if (controller.isGrounded && SwipeManager.swipeup)
            {
                Jump();
                animator.SetTrigger("Jump");
                Debug.Log("ANIMATOR: Trigger Jump diaktifkan");
            }

            // Aksi slide
            if (SwipeManager.swipedown && !isSliding)
            {
                StartCoroutine(Slide());
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

            if (desiredLane == 0)
            {
                targetposition += Vector3.left * laneDistance;
            }
            else if (desiredLane == 2)
            {
                targetposition += Vector3.right * laneDistance;
            }

            if (transform.position == targetposition)
                return;
            Vector3 diff = targetposition - transform.position;
            Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = JumpForce;
    }

    private IEnumerator Slide()
    {
        isSliding = true;

        // Aktifkan animasi slide
        animator.SetTrigger("Slide");
        Debug.Log("ANIMATOR: Trigger Slide diaktifkan");

        // Lerp ke nilai slide
        float elapsedTime = 0f;
        float initialHeight = controller.height;
        Vector3 initialCenter = controller.center;

        while (elapsedTime < slideDuration / 2) // Setengah durasi untuk turun
        {
            controller.height = Mathf.Lerp(initialHeight, SlideHeight, elapsedTime / (slideDuration / 2));
            controller.center = Vector3.Lerp(initialCenter, SlideCenter, elapsedTime / (slideDuration / 2));

            elapsedTime += Time.deltaTime * slideTransitionSpeed;
            yield return null;
        }

        // Tahan posisi slide selama sisa durasi
        yield return new WaitForSeconds(slideDuration / 2);

        // Lerp kembali ke nilai normal
        elapsedTime = 0f;
        while (elapsedTime < slideDuration / 2) // Setengah durasi untuk naik kembali
        {
            controller.height = Mathf.Lerp(SlideHeight, NormalHeight, elapsedTime / (slideDuration / 2));
            controller.center = Vector3.Lerp(SlideCenter, NormalCenter, elapsedTime / (slideDuration / 2));

            elapsedTime += Time.deltaTime * slideTransitionSpeed;
            yield return null;
        }

        // Pastikan nilai kembali normal
        controller.height = NormalHeight;
        controller.center = NormalCenter;

        isSliding = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacles")
        {
            PlayerManager.gameover = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
            animator.SetTrigger("Die");
            Debug.Log("ANIMATOR: Trigger Die diaktifkan");
        }
    }
}
