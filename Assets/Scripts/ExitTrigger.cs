using UnityEngine;
public class ExitTrigger : MonoBehaviour
{
    [SerializeField] private TimerSystem timerSystem;
    [SerializeField] private MinigameState minigameState;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // only react to the player
        if (!timerSystem.IsRunning()) return; // timer must currently be active
        if (!minigameState.secondMinigameCompleted) return; // must have finished second minigame first

        timerSystem.StopTimer(); // all conditions met -> stop the timer (player reached exit)
    }
}