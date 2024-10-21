using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Tambahkan untuk menggunakan UI

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    public Image crosshair; // Referensi ke Crosshair UI

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

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
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
    }

    void HandleJumping()
    {
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleRotation()
    {
        characterController.Move(moveDirection * Time.deltaTime);

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
