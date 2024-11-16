using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))] // Memastikan AudioSource ada
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

    public AudioClip footstepSound; // Suara langkah kaki
    public AudioClip shootSound; // Suara tembakan
    public AudioClip jumpSound; // Suara lompatan
    private AudioSource audioSource; // AudioSource untuk memainkan suara

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    private float verticalVelocity = 0f;
    private bool isJumping = false;
    public EnergyWeaponWithSlider energyWeapon; // Drag and drop objek yang memiliki skrip EnergyWeaponWithSlider

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>(); // Mengambil referensi AudioSource
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set ukuran awal crosshair
        SetCrosshairSize(50f); // Ukuran default
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleCrosshair();
    }

    void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift); // Deteksi tombol Shift untuk berlari

        // Hitung kecepatan berjalan dan berlari
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Memainkan suara langkah kaki saat berjalan atau berlari
        if (isRunning || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            PlayFootstepSound();
        }

        // Tambahkan gravitasi manual
        if (characterController.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;

            if (Input.GetButtonDown("Jump") && canMove)
            {
                isJumping = true;
                verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
                PlayJumpSound(); // Mainkan suara lompatan
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; // Terapkan gravitasi saat tidak di tanah
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
        // Menyembunyikan crosshair saat menembak
        if (Input.GetButton("Fire1"))
        {
            // Cek apakah energyWeapon tidak null dan bisa menembak
            if (energyWeapon != null && energyWeapon.canShoot)
            {
                // Kurangi energi saat menembak
                energyWeapon.Shoot(); // Fungsi tembakan pada skrip EnergyWeaponWithSlider

                SetCrosshairSize(30f); // Ubah ukuran saat menembak
                PlayShootSound(); // Mainkan suara tembakan
            }
            else
            {
                // Jika tidak bisa menembak, set crosshair ke ukuran normal
                SetCrosshairSize(50f); // Ukuran default
            }
        }
        else
        {
            SetCrosshairSize(50f); // Ukuran default jika tidak menembak
        }
    }



    void SetCrosshairSize(float size)
    {
        // Mengatur ukuran crosshair
        crosshair.rectTransform.sizeDelta = new Vector2(size, size);
    }

    void Die()
    {
        // Nonaktifkan pergerakan player jika mati
        canMove = false;

        // Hancurkan objek pemain setelah beberapa detik untuk memberi waktu pada animasi mati
        Destroy(gameObject, 3f); // Hancurkan objek setelah 3 detik
    }

    void PlayFootstepSound()
    {
        if (!audioSource.isPlaying) // Cek apakah suara sedang dimainkan
        {
            audioSource.PlayOneShot(footstepSound); // Mainkan suara langkah kaki
        }
    }

    void PlayShootSound()
    {
        audioSource.PlayOneShot(shootSound); // Mainkan suara tembakan
    }

    void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound); // Mainkan suara lompatan
    }
}
