using UnityEngine;

public class NightCompleteTrigger : MonoBehaviour
{
    [SerializeField] private string storySceneName = "StoryScene";
    [SerializeField] private MinigameState minigameState;
    [SerializeField] private LockpickManager lockpickManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        bool doorUnlocked = !lockpickManager.IsLocked;
        bool harvestComplete = minigameState.secondMinigameCompleted;

        if (!doorUnlocked || !harvestComplete)
        {
            GameProgressionManager.Instance.currentMode = GameProgressionManager.StoryMode.BadEnding2;
            SceneTransition.Instance.LoadScene(storySceneName);
            return;
        }

        GameProgressionManager.Instance.currentMode = GameProgressionManager.StoryMode.NightOutro;
        SceneTransition.Instance.LoadScene(storySceneName);
    }
}