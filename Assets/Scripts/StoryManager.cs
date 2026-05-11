using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class StoryManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private Image backgroundImage;

    [Header("Background")]
    [SerializeField] private Sprite defaultBackground;

    [Header("Scene Names")]
    [SerializeField] private string gameplaySceneName = "GameplayScene";

    private TextAsset[] _segments;
    private int _segmentIndex = 0;
    private bool _ready = false;

    private void Start()
    {
        storyPanel.SetActive(false);

        // Show black screen first, wait for left click to begin
        StartCoroutine(WaitForClick());
    }

    private IEnumerator WaitForClick()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        SceneTransition.Instance.FadeIn();
        _segments = GameProgressionManager.Instance.GetCurrentSegments();
        _segmentIndex = 0;
        ShowSegment();
    }

    private void Update()
    {
        if (!_ready) return;

        if (Input.GetMouseButtonDown(0))
            AdvanceSegment();
    }

    private void ShowSegment()
    {
        if (_segments == null || _segmentIndex >= _segments.Length || _segments[_segmentIndex] == null)
        {
            OnAllSegmentsDone();
            return;
        }

        _ready = false;
        storyPanel.SetActive(true);

        Story story = new Story(_segments[_segmentIndex].text);
        string fullText = "";

        while (story.canContinue)
            fullText += story.Continue();

        storyText.text = fullText.Trim();

        // Small delay so the click that triggered this doesn't immediately advance
        StartCoroutine(ReadyAfterDelay());
    }

    private IEnumerator ReadyAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        _ready = true;
    }

    private void AdvanceSegment()
    {
        _segmentIndex++;

        if (_segmentIndex >= _segments.Length || _segments[_segmentIndex] == null)
        {
            OnAllSegmentsDone();
            return;
        }

        ShowSegment();
    }

    private void OnAllSegmentsDone()
    {
        _ready = false;
        storyPanel.SetActive(false);

        if (GameProgressionManager.Instance.isOpening)
        {
            GameProgressionManager.Instance.isOpening = false;
            SceneTransition.Instance.LoadScene(gameplaySceneName);
        }
        else
        {
            GameProgressionManager.Instance.currentNight++;

            if (GameProgressionManager.Instance.currentNight > 5)
            {
                // Game complete — extend this later for an ending screen
                Debug.Log("All nights complete.");
                return;
            }

            SceneTransition.Instance.LoadScene(gameplaySceneName);
        }
    }
}