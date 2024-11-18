using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FPSPlayerController : MonoBehaviour
{
    // Tambahkan deklarasi isRunning di sini
    private bool isRunning = false;

    // Variabel lain...
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpHeight = 2f;
    public float gravity = 9.8f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public Image crosshair;
    public AudioClip footstepSound;
    public AudioClip shootSound;
    public AudioClip jumpSound;
    private AudioSource audioSource;
    private float footstepTimer = 0f;
    public float footstepDelay = 0.5f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    private float verticalVelocity = 0f;
    private bool isJumping = false;
    public EnergyWeaponWithSlider energyWeapon;

    private CharacterController characterController;
    private PauseMenuController pauseMenuController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        pauseMenuController = FindObjectOfType<PauseMenuController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (canMove)
        {
            HandleMovement();
            HandleRotation();
        }

        footstepTimer -= Time.deltaTime;
        HandleCrosshair();
    }

    void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Pindahkan deteksi isRunning ke dalam fungsi ini
        isRunning = Input.GetKey(KeyCode.LeftShift); // Deteksi tombol Shift untuk berlari

        // Hitung kecepatan berjalan dan berlari
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Mainkan suara langkah kaki
        if (isRunning || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
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
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    void HandleCrosshair()
    {
        if (energyWeapon != null && !energyWeapon.canShoot)
        {
            SetCrosshairSize(30f);
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

    void PlayFootstepSound()
    {
        if (footstepTimer <= 0f && (isRunning || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            audioSource.PlayOneShot(footstepSound);
            footstepTimer = footstepDelay;
        }
    }

    void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }
}
