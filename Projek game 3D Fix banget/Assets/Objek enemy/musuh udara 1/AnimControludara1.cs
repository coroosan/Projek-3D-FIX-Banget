using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControludara1 : MonoBehaviour
{
    private Animator animator;
    private bool dead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("walk", true); // Set animasi awal ke berjalan
    }

    void Update()
    {
        if (dead) return; // Jika sudah mati, hentikan kontrol animasi

        // Contoh kontrol animasi saat berjalan
        bool walk = true /* masukkan logika berjalan, misalnya jika enemy mendeteksi player */;
        animator.SetBool("walk", walk);
    }

    public void Die()
    {
        dead = true;
        animator.SetBool("dead", true); // Set animasi mati
        animator.SetBool("walk", false); // Nonaktifkan animasi berjalan
    }
}