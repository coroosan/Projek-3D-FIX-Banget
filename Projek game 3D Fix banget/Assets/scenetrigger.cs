using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public SceneTransitionManager transitionManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Memastikan transitionManager tidak null sebelum memanggil metode
            if (transitionManager != null)
            {
                transitionManager.TriggerSceneTransition();
            }
            else
            {
                Debug.LogError("SceneTransitionManager belum diassign di Inspector!");
            }
        }
    }
}
