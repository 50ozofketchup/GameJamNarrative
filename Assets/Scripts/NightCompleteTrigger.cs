using UnityEngine;
public class NightCompleteTrigger : MonoBehaviour
{
    [SerializeField] private string storySceneName = "StoryScene";
    [SerializeField] private MinigameState minigameState;
    [SerializeField] private LockpickManager lockpickManager;

    public void SetLockpickManager(LockpickManager lm)
    {
        lockpickManager = lm;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        bool doorUnlocked = lockpickManager != null && !lockpickManager.IsLocked;
        bool harvestComplete = minigameState.secondMinigameCompleted;

        Debug.Log($"NightCompleteTrigger hit - doorUnlocked: {doorUnlocked}, harvestComplete: {harvestComplete}, lockpickManager null: {lockpickManager == null}");

        // if player didn't unlock the door or didn't finish the harvest minigame, send to bad ending
        if (!doorUnlocked || !harvestComplete)
        {
            GameProgressionManager.Instance.currentMode = GameProgressionManager.StoryMode.BadEnding2;
            SceneTransition.Instance.LoadScene(storySceneName);
            return;
        }

        // otherwise, proceed to normal night outro
        GameProgressionManager.Instance.currentMode = GameProgressionManager.StoryMode.NightOutro;
        SceneTransition.Instance.LoadScene(storySceneName);
    }
}