using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCameraFollow : MonoBehaviour
{
    public Transform target; // misalnya kepala karakter
    public Vector3 offset;
    private bool isRagdollActive = false;

    void Update()
    {
        RagdollActivator activator = target.GetComponentInParent<RagdollActivator>();
        if (activator != null && activator.playerHealth <= 0 && !isRagdollActive)
        {
            isRagdollActive = true;
            activator.CheckHealth(); // memicu ragdoll
        }

        if (isRagdollActive)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}
