using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FPSPlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpHeight = 2f;
    public float gravity = 9.8f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    public Image crosshair;

    public float health = 100f;
    public Animator animator;

    public AudioClip footstepSound;
    public AudioClip shootSound;
    public AudioClip jumpSound;
    public AudioClip deadSound;
    private AudioSource audioSource;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    public bool canMove = true;
    private float verticalVelocity = 0f;
    private bool isJumping = false;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Set ukuran awal crosshair
        SetCrosshairSize(50f);
    }

    void Update()
    {
        if (health <= 0f)
        {
            Die();
            return;
        }

        if (canMove)
        {
            HandleMovement();
        }

        HandleRotation();
        HandleCrosshair();
    }

    void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isWalking = !isRunning && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0);

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsWalking", isWalking);

        if (isWalking || isRunning)
        {
            PlayFootstepSound();
        }

        if (characterController.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;

            if (Input.GetButtonDown("Jump") && canMove)
            {
                isJumping = true;
                verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
                PlayJumpSound();
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleRotation()
    {
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    void HandleCrosshair()
    {
        if (Input.GetButton("Fire1") && canMove)
        {
            SetCrosshairSize(30f);
            PlayShootSound();
        }
        else
        {
            SetCrosshairSize(50f);
        }
    }

    void SetCrosshairSize(float size)
    {
        crosshair.rectTransform.sizeDelta = new Vector2(size, size);
    }

    void Die()
    {
        if (animator != null)
        {
            animator.SetBool("IsDead", true); // Set `IsDead` untuk transisi animasi mati
        }

        if (deadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deadSound);
        }

        canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Hancurkan objek pemain setelah beberapa detik
        Destroy(gameObject, 3f);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Die();
        }
    }

    void PlayFootstepSound()
    {
        if (!audioSource.isPlaying && footstepSound != null)
        {
            audioSource.PlayOneShot(footstepSound);
        }
    }

    void PlayShootSound()
    {
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }
}
