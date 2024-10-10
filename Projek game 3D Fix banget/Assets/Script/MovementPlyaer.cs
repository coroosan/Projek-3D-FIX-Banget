using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlyaer : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f; // Kecepatan jalan normal
    public float sprintSpeed = 12f; // Kecepatan saat sprint
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    private Vector3 targetPosition; // Posisi tujuan yang dihasilkan oleh raycast
    private bool isMovingToTarget = false; // Apakah karakter sedang bergerak ke target

    void Update()
    {
        // Mengecek apakah pemain menyentuh tanah
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Input untuk gerakan pemain (WASD)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Mengatur sprint
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        // Pergerakan manual dengan keyboard
        if (direction.magnitude >= 0.1f)
        {
            MoveWithKeyboard(direction, currentSpeed);
        }
        else if (isMovingToTarget)
        {
            MoveToTarget(currentSpeed);
        }

        // Menambahkan gravitasi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Lompat
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Raycast untuk gerakan mouse
        if (Input.GetMouseButtonDown(0)) // Klik kiri mouse
        {
            PerformRaycast();
        }
    }

    // Fungsi untuk menggerakkan karakter menggunakan input keyboard
    void MoveWithKeyboard(Vector3 direction, float currentSpeed)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
    }

    // Fungsi untuk menggerakkan karakter ke titik target raycast
    void MoveToTarget(float currentSpeed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        // Jika sudah dekat dengan target, hentikan pergerakan
        if (distance < 0.1f)
        {
            isMovingToTarget = false;
            return;
        }

        // Mengatur rotasi karakter agar menghadap target
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // Gerakkan karakter ke target
        controller.Move(direction * currentSpeed * Time.deltaTime);
    }

    // Fungsi untuk melakukan raycast dari kamera ke dunia berdasarkan posisi mouse
    void PerformRaycast()
    {
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Simpan posisi target dari raycast
            targetPosition = hit.point;
            isMovingToTarget = true;
        }
    }
}