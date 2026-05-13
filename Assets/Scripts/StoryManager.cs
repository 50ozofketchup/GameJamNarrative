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

        var gpm = GameProgressionManager.Instance;

        switch (gpm.currentMode)
        {
            case GameProgressionManager.StoryMode.Opening:
                gpm.currentMode = GameProgressionManager.StoryMode.NightOutro;
                SceneTransition.Instance.LoadScene(gameplaySceneName);
                break;

            case GameProgressionManager.StoryMode.NightOutro:
                gpm.currentNight++;
                Debug.Log($"Starting night {gpm.currentNight + 1}");
                if (gpm.currentNight > 5)
                {
                    Debug.Log("All nights complete.");
                    return;
                }
                SceneTransition.Instance.LoadScene(gameplaySceneName);
                break;

            case GameProgressionManager.StoryMode.BadEnding1:
            case GameProgressionManager.StoryMode.BadEnding2:
                gpm.currentMode = GameProgressionManager.StoryMode.NightOutro;
                SceneTransition.Instance.LoadScene(gameplaySceneName);
                break;
        }
    }
}