using UnityEngine;
using TMPro;
public class TimerSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeLimit = 60f;
    [SerializeField] private float timeReductionPerNight = 20f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("References")]
    [SerializeField] private LockpickManager lockpickManager;
    [SerializeField] private MinigameState minigameState;

    [Header("Scene Names")]
    [SerializeField] private string storySceneName = "StoryScene";

    private float _timeRemaining;
    private bool _running;
    private Vector3 _resetPosition; // where player respawns if timer expires
    private CharacterController _characterController;

    private void Start()
    {
        if (timerText != null) timerText.text = "";
    }

    public void SetLockpickManager(LockpickManager lm)
    {
        lockpickManager = lm;
    }

    private void Update()
    {
        if (!_running) return;
        _timeRemaining -= Time.deltaTime;
        UpdateTimerUI();
        if (_timeRemaining <= 0f)
            OnTimerExpired();
    }

    public void StartTimer(Vector3 resetPosition, CharacterController characterController)
    {
        _resetPosition = resetPosition;
        _characterController = characterController;

        // each completed night reduces the time limit (vois mennä 0 asti mut olis aik silly)
        int night = GameProgressionManager.Instance != null
            ? GameProgressionManager.Instance.currentNight
            : 1;
        _timeRemaining = Mathf.Max(0f, timeLimit - (night - 1) * timeReductionPerNight);
        _running = true;
    }

    public void StopTimer()
    {
        _running = false;
        if (timerText != null) timerText.text = "";
    }

    public bool IsRunning() => _running;

    // called when time runs out before player escapes, sends them to bad ending
    private void OnTimerExpired()
    {
        _running = false;
        if (timerText != null) timerText.text = "";
        minigameState.secondMinigameCompleted = false;
        lockpickManager.ResetLock();
        GameProgressionManager.Instance.currentMode = GameProgressionManager.StoryMode.BadEnding1;
        GameProgressionManager.Instance.badEnding1ReturnPosition = _resetPosition;
        SceneTransition.Instance.LoadScene(storySceneName);
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;
        int seconds = Mathf.CeilToInt(Mathf.Max(_timeRemaining, 0f));
        timerText.text = $"{seconds}s left until you get caught";
    }
}