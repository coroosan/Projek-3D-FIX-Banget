using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Ambil komponen Animator dari objek
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Contoh kontrol animasi menggunakan parameter
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("JumpTrigger");  // Trigger animasi lompat
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("IsRunning", true);  // Aktifkan animasi lari
        }
        else
        {
            animator.SetBool("IsRunning", false);  // Nonaktifkan animasi lari
        }

        // Mengontrol animasi dengan float pada Blend Tree
        float move = Input.GetAxis("Vertical");
        animator.SetFloat("Blend", move);
    }
}