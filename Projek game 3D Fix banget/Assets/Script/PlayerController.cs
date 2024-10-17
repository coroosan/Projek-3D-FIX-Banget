using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 5f;

    private Transform cameraTransform;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction aimAction;
    private InputAction shootAction;

    private PlayerInput playerInput;

    // Tambahkan reference untuk Canvas dan EventSystem
    [SerializeField]
    private Canvas targetCanvas;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    // Variables for shooting bullets
    [SerializeField]
    private GameObject bulletPrefab;  // Prefab peluru
    [SerializeField]
    private Transform bulletSpawnPoint;  // Tempat spawn peluru
    [SerializeField]
    private float bulletSpeed = 10f;  // Kecepatan peluru
    [SerializeField]
    private float bulletDamage = 10f;  // Damage peluru

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        aimAction = playerInput.actions["Aim"];
        shootAction = playerInput.actions["Shoot"];

        // Ambil komponen GraphicRaycaster dari Canvas
        raycaster = targetCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        // Kunci kursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Menyembunyikan kursor
    }

    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
    }

    private void ShootGun()
    {
        // Buat ray dari posisi pointer
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };

        // Daftar untuk menyimpan hasil raycast
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        raycaster.Raycast(pointerData, raycastResults);

        // Cek jika ada objek UI yang terdeteksi
        if (raycastResults.Count > 0)
        {
            // Ada objek UI yang terdeteksi, jangan lakukan tembakan
            return;
        }

        // Spawn bullet dari spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Mengarahkan bullet ke arah depan kamera
        Vector3 shootingDirection = cameraTransform.forward;
        bulletRb.velocity = shootingDirection * bulletSpeed;

        // Berikan damage saat peluru mengenai objek
        BulletController bulletScript = bullet.GetComponent<BulletController>();
        if (bulletScript != null)
        {
            bulletScript.SetDamage(bulletDamage);
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate the player to face the direction of movement
        if (aimAction.IsPressed())
        {
            // Arahkan kamera ke arah depan dan atur rotasi karakter
            Vector3 aimDirection = cameraTransform.forward;
            aimDirection.y = 0; // Pastikan tidak ada rotasi pada sumbu Y
            if (aimDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            // Rotate player to camera direction if not aiming
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        // Mengembalikan kursor saat script dihentikan
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Menampilkan kembali kursor
    }
}
