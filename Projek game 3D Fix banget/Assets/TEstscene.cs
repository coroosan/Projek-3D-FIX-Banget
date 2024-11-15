using UnityEngine;

public class TestSceneTransition : MonoBehaviour
{
    public SceneTransitionManager transitionManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transitionManager.TriggerSceneTransition();
        }
    }
}
