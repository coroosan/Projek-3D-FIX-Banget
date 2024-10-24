using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FPSPlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpHeight = 2f; // Tinggi lompatan
    public float gravity = 9.8f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    public Image crosshair; // Referensi ke Crosshair UI

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    private float verticalVelocity = 0f;
    private bool isJumping = false;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set ukuran awal crosshair
        SetCrosshairSize(50f); // Ukuran default
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleRotation();
        HandleCrosshair();
    }

    void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Tambahkan gravitasi manual
        if (characterController.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;

            if (Input.GetButtonDown("Jump") && canMove)
            {
                isJumping = true;
                verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; // Terapkan gravitasi saat tidak di tanah
        }

        moveDirection.y = verticalVelocity;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleJumping()
    {
        // Tidak ada perubahan di sini karena lompat sudah dikelola di HandleMovement()
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
        // Menyembunyikan crosshair saat menembak
        if (Input.GetButton("Fire1"))
        {
            SetCrosshairSize(30f); // Ubah ukuran saat menembak
        }
        else
        {
            SetCrosshairSize(50f); // Ukuran default
        }
    }

    void SetCrosshairSize(float size)
    {
        // Mengatur ukuran crosshair
        crosshair.rectTransform.sizeDelta = new Vector2(size, size);
    }
}