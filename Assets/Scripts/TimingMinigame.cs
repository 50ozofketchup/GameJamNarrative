using System;
using UnityEngine;
using TMPro;

public class TimingMinigame : MonoBehaviour
{
    public event Action OnSuccess;
    public event Action OnFail;

    [Header("UI References")]
    [SerializeField] private RectTransform needle;
    [SerializeField] private RectTransform greenZone;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("Timing Settings")]
    [SerializeField] private float initialSpeed = 300f;
    [SerializeField] private float speedIncreasePerRound = 80f;
    [SerializeField] private int totalRounds = 5;

    [Header("Failure Settings")]
    [SerializeField] private int maxFailures = 3;

    [Header("Green Zone")]
    [SerializeField] private float greenZoneMinX = -367f;
    [SerializeField] private float greenZoneMaxX = 367f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip missSound;

    private float _currentSpeed;
    private float _direction = 1f;
    private int _currentRound;
    private int _failures;
    private bool _gameActive;
    private float _boxLeft;
    private float _boxRight;

    public void StartGame()
    {
        RectTransform box = GetComponent<RectTransform>();
        float halfBox = box.rect.width / 2f;
        float halfNeedle = needle.rect.width / 2f;
        _boxLeft = -halfBox + halfNeedle;
        _boxRight = halfBox - halfNeedle;

        _currentRound = 0;
        _failures = 0;
        StartRound();
    }

    private void Update()
    {
        if (!_gameActive) return;

        MoveNeedle();

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            EvaluateInput();
    }

    private void MoveNeedle()
    {
        float newX = needle.anchoredPosition.x + _direction * _currentSpeed * Time.deltaTime;

        if (newX >= _boxRight) { newX = _boxRight; _direction = -1f; }
        else if (newX <= _boxLeft) { newX = _boxLeft; _direction = 1f; }

        needle.anchoredPosition = new Vector2(newX, needle.anchoredPosition.y);
    }

    private void EvaluateInput()
    {
        if (IsNeedleInGreenZone())
        {
            PlaySound(hitSound);
            _currentRound++;

            if (_currentRound >= totalRounds)
            {
                _gameActive = false;
                ShowResult("UNLOCKED!", Color.green);
                Invoke(nameof(TriggerSuccess), 1f);
                return;
            }

            ShowResult($"Hit! {_currentRound}/{totalRounds}", Color.green);
            StartRound();
        }
        else
        {
            PlaySound(missSound);
            _failures++;

            if (_failures >= maxFailures)
            {
                _gameActive = false;
                ShowResult("FAILED!", Color.red);
                Invoke(nameof(TriggerFail), 1f);
                return;
            }

            ShowResult($"Miss! {maxFailures - _failures} left", Color.red);
        }
    }

    private void TriggerSuccess() => OnSuccess?.Invoke();
    private void TriggerFail() => OnFail?.Invoke();

    private void StartRound()
    {
        _currentSpeed = initialSpeed + _currentRound * speedIncreasePerRound;
        _gameActive = true;

        float randomX = UnityEngine.Random.Range(greenZoneMinX, greenZoneMaxX);
        greenZone.anchoredPosition = new Vector2(randomX, greenZone.anchoredPosition.y);

        needle.anchoredPosition = new Vector2(_boxLeft, needle.anchoredPosition.y);
        _direction = 1f;
        ShowResult($"Round {_currentRound + 1} / {totalRounds}", Color.white);
    }

    private bool IsNeedleInGreenZone()
    {
        float needleX = needle.anchoredPosition.x;
        float zoneLeft = greenZone.anchoredPosition.x - greenZone.rect.width / 2f;
        float zoneRight = greenZone.anchoredPosition.x + greenZone.rect.width / 2f;
        return needleX >= zoneLeft && needleX <= zoneRight;
    }

    private void ShowResult(string message, Color color)
    {
        if (resultText == null) return;
        resultText.text = message;
        resultText.color = color;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        audioSource.PlayOneShot(clip);
    }
}