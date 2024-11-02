using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBatre : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(gameObject, ps.main.duration); // Hancurkan setelah durasi particle system
        }
        else
        {
            Destroy(gameObject, 2f); // Durasi default jika tidak ada particle system
        }
    }
}