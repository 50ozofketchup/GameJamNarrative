using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [SerializeField] private TimerSystem timerSystem;
    [SerializeField] private MinigameState minigameState;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!timerSystem.IsRunning()) return;
        if (!minigameState.secondMinigameCompleted) return;

        timerSystem.StopTimer();
    }
}