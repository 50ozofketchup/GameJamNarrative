using UnityEngine;
using TMPro;

public class TimerSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeLimit = 60f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("References")]
    [SerializeField] private LockpickManager lockpickManager;
    [SerializeField] private MinigameState minigameState;

    private float _timeRemaining;
    private bool _running;
    private Vector3 _resetPosition;
    private CharacterController _characterController;

    private void Start()
    {
        if (timerText != null) timerText.text = "";
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
        _timeRemaining = timeLimit;
        _running = true;
    }

    public void StopTimer()
    {
        _running = false;
        if (timerText != null) timerText.text = "";
    }

    public bool IsRunning() => _running;

    private void OnTimerExpired()
    {
        _running = false;
        if (timerText != null) timerText.text = "";
        minigameState.secondMinigameCompleted = false;
        ResetPlayer();
        lockpickManager.ResetLock();
    }

    private void ResetPlayer()
    {
        _characterController.enabled = false;
        _characterController.transform.position = _resetPosition;
        _characterController.enabled = true;
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;
        int seconds = Mathf.CeilToInt(Mathf.Max(_timeRemaining, 0f));
        timerText.text = $"{seconds}s";
    }
}