using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        // Ambil komponen Animator dari objek
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Mengontrol animasi dengan float pada Blend Tree untuk berjalan, idle, dan berlari
        float move = Input.GetAxis("Vertical");
        animator.SetFloat("Blend", move);

        // Mengaktifkan dan menonaktifkan animasi lompat menggunakan bool
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jumping", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("Jumping", false);
        }

        // Mengaktifkan dan menonaktifkan animasi menembak menggunakan bool
        if (Input.GetMouseButtonDown(0))  // Klik kiri mouse untuk menembak
        {
            animator.SetBool("Shoot", true);
        }
        else if (Input.GetMouseButtonUp(0)) // Lepaskan klik kiri untuk menghentikan animasi menembak
        {
            animator.SetBool("Shoot", false);
        }

        // Trigger animasi mati (contoh, bisa disesuaikan jika diperlukan trigger atau bool)
        if (Input.GetKeyDown(KeyCode.K))  // Tekan tombol 'K' untuk memicu animasi mati
        {
            animator.SetTrigger("Dead");
        }
    }
}
