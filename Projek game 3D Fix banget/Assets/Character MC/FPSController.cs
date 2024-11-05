using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FPSPlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpHeight = 2f;
    public float gravity = 9.8f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    public Image crosshair; // Referensi ke Crosshair UI
    public float health = 100f; // Kesehatan pemain

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    public bool canMove = true;
    private float verticalVelocity = 0f;
    private bool isJumping = false;
    private bool isDead = false; // Menandakan apakah pemain mati
    private Animator animator;

    private CharacterController characterController;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetCrosshairSize(50f); // Ukuran default
    }

    void Update()
    {
        if (!isDead) // Hanya jalankan kontrol jika tidak mati
        {
            HandleMovement();
            HandleJumping();
            HandleRotation();
            HandleCrosshair();
        }
        else
        {
            // Logika setelah mati, jika diperlukan
            // Misalnya, menonaktifkan input atau memberikan efek visual
        }

        // Cek kesehatan
        if (health <= 0 && !isDead)
        {
            Die();
        }
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
            verticalVelocity -= gravity * Time.deltaTime;
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
        crosshair.rectTransform.sizeDelta = new Vector2(size, size);
    }

    public void Die()
    {
        if (isDead) return; // Hindari panggilan berulang
        isDead = true;

        animator.SetBool("isDead", true); // Memicu animasi mati
        GetComponent<Collider>().enabled = false; // Menonaktifkan collider agar tidak bisa berinteraksi

        // Menonaktifkan semua gerakan
        canMove = false;
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            health -= damage; // Kurangi kesehatan
            if (health <= 0)
            {
                Die(); // Jika kesehatan <= 0, panggil metode Die()
            }
        }
    }
}
