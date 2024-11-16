using UnityEngine;

public class CameraDeath : MonoBehaviour
{
    public Animator cameraAnimator;  // Referensi ke Animator Kamera

    public void TriggerDeathAnimation()
    {
        // Memicu trigger 'DeathTrigger' yang ada di Animator
        cameraAnimator.SetTrigger("DeathTrigger");
    }
}