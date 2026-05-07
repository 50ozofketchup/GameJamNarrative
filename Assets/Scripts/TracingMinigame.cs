using UnityEngine;
using UnityEngine.UI;

public class TracingMinigame : MonoBehaviour
{
    [Header("Tracing Settings")]
    [SerializeField] private float tolerance = 20f;
    [SerializeField] private int maxMistakes = 3;
    [SerializeField] private int totalPoints = 40;

    [Header("UI References")]
    [SerializeField] private RectTransform tracingArea;
    [SerializeField] private Image progressIndicator;
    [SerializeField] private Image cursorDot;

    [Header("Path Visuals")]
    [SerializeField] private Color pathColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField] private Color completedColor = Color.green;
    [SerializeField] private Color startPointColor = Color.yellow;
    [SerializeField] private float dotSize = 12f;

    private Vector2[] _pathPoints;
    private Image[] _pathDots;
    private int _currentPointIndex = 0;
    private int _mistakes = 0;
    private bool _active = false;
    private bool _mouseHeld = false;

    public System.Action OnSuccess;
    public System.Action OnFail;

    public void StartMinigame()
    {
        ClearDots();
        _pathPoints = GenerateOPath();
        _pathDots = new Image[_pathPoints.Length];
        _currentPointIndex = 0;
        _mistakes = 0;
        _active = false;
        _mouseHeld = false;

        DrawPath();
        UpdateProgress();
    }

    private void Update()
    {
        if (_pathPoints == null) return;

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            tracingArea, Input.mousePosition, null, out mousePos);

        if (cursorDot != null)
            cursorDot.rectTransform.anchoredPosition = mousePos;

        if (Input.GetMouseButtonDown(0))
        {
            if (!_active && _currentPointIndex == 0 &&
                Vector2.Distance(mousePos, _pathPoints[0]) <= tolerance)
            {
                _active = true;
            }
            _mouseHeld = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseHeld = false;
            if (_active) RegisterMistake();
        }

        if (!_active || !_mouseHeld) return;

        float distToNext = Vector2.Distance(mousePos, _pathPoints[_currentPointIndex]);

        if (distToNext <= tolerance)
        {
            if (_pathDots[_currentPointIndex] != null)
                _pathDots[_currentPointIndex].color = completedColor;

            _currentPointIndex++;
            UpdateProgress();

            if (_currentPointIndex >= _pathPoints.Length)
            {
                _active = false;
                OnSuccess?.Invoke();
            }
        }
        else
        {
            float distToPrev = _currentPointIndex > 0
                ? Vector2.Distance(mousePos, _pathPoints[_currentPointIndex - 1])
                : 0f;

            if (distToPrev > tolerance * 2.5f)
                RegisterMistake();
        }
    }

    private void RegisterMistake()
    {
        _mistakes++;
        _currentPointIndex = 0;
        _active = false;
        _mouseHeld = false;

        foreach (var dot in _pathDots)
            if (dot != null) dot.color = pathColor;

        if (_pathDots.Length > 0 && _pathDots[0] != null)
            _pathDots[0].color = startPointColor;

        UpdateProgress();
        Debug.Log($"Tracing mistake. {maxMistakes - _mistakes} remaining.");

        if (_mistakes >= maxMistakes)
            OnFail?.Invoke();
    }

    private void DrawPath()
    {
        for (int i = 0; i < _pathPoints.Length; i++)
        {
            GameObject dotObj = new GameObject($"Dot_{i}", typeof(Image));
            dotObj.transform.SetParent(tracingArea, false);

            Image img = dotObj.GetComponent<Image>();
            img.color = i == 0 ? startPointColor : pathColor;
            img.rectTransform.sizeDelta = i == 0
                ? new Vector2(dotSize * 1.8f, dotSize * 1.8f)
                : new Vector2(dotSize, dotSize);
            img.rectTransform.anchoredPosition = _pathPoints[i];

            _pathDots[i] = img;
        }
    }

    private void ClearDots()
    {
        if (_pathDots == null) return;
        foreach (var dot in _pathDots)
            if (dot != null) Destroy(dot.gameObject);
    }

    private void UpdateProgress()
    {
        if (progressIndicator == null || _pathPoints == null) return;
        progressIndicator.fillAmount = (float)_currentPointIndex / _pathPoints.Length;
    }

    private Vector2[] GenerateOPath()
    {
        Vector2[] points = new Vector2[totalPoints];
        float rx = tracingArea.rect.width * 0.35f;
        float ry = tracingArea.rect.height * 0.35f;

        for (int i = 0; i < totalPoints; i++)
        {
            float angle = Mathf.PI * 2f * i / totalPoints;
            points[i] = new Vector2(Mathf.Cos(angle) * rx, Mathf.Sin(angle) * ry);
        }

        return points;
    }
}