using UnityEngine;

public class NightCompleteTrigger : MonoBehaviour
{
    [SerializeField] private string storySceneName = "StoryScene";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        SceneTransition.Instance.LoadScene(storySceneName);
    }
}