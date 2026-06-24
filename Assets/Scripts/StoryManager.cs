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
    private bool _ready = false; // true once current segment text is fully shown and click-to-advance is allowed

    private void Start()
    {
        Debug.Log($"StoryManager Start - mode: {GameProgressionManager.Instance.currentMode}, night: {GameProgressionManager.Instance.currentNight}");
        storyPanel.SetActive(false);
        StartCoroutine(WaitForClick());
    }

    private IEnumerator WaitForClick()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); // wait for player's first click before starting
        SceneTransition.Instance.FadeIn();

        // pull the correct Ink segments for current story mode/nigh
        _segments = GameProgressionManager.Instance.GetCurrentSegments();
        _segmentIndex = 0;
        Debug.Log($"WaitForClick - segments count: {(_segments != null ? _segments.Length : 0)}");
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
        // no more segments -> wrap up
        if (_segments == null || _segmentIndex >= _segments.Length || _segments[_segmentIndex] == null)
        {
            OnAllSegmentsDone();
            return;
        }

        _ready = false;
        storyPanel.SetActive(true);

        // run the Ink story file fully and collect all of its text
        Story story = new Story(_segments[_segmentIndex].text);
        string fullText = "";
        while (story.canContinue)
            fullText += story.Continue();
        storyText.text = fullText.Trim();

        StartCoroutine(ReadyAfterDelay());
    }

    private IEnumerator ReadyAfterDelay()
    {
        // small delay before accepting clicks, so the click that triggered this segment doesn't accidentally skip it
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
        Debug.Log($"OnAllSegmentsDone - mode: {gpm.currentMode}, night: {gpm.currentNight}");

        switch (gpm.currentMode)
        {
            case GameProgressionManager.StoryMode.Opening:
                // opening finished -> go straight into night 1 gameplay
                gpm.currentMode = GameProgressionManager.StoryMode.NightOutro;
                Debug.Log("Opening done, loading gameplay night 1");
                SceneTransition.Instance.LoadScene(gameplaySceneName);
                break;

            case GameProgressionManager.StoryMode.NightOutro:
                // night outro finished -> advance to next night
                Debug.Log($"Night {gpm.currentNight} outro done, incrementing to {gpm.currentNight + 1}");
                gpm.currentNight++;
                if (gpm.currentNight > 5)
                {
                    Debug.Log("All nights complete.");
                    return; // game finished, no further scene load
                }
                SceneTransition.Instance.LoadScene(gameplaySceneName);
                break;

            case GameProgressionManager.StoryMode.BadEnding1:
            case GameProgressionManager.StoryMode.BadEnding2: // "I just couldn't do it!!!"
                // bad ending finished -> retry the same night
                gpm.currentMode = GameProgressionManager.StoryMode.NightOutro;
                Debug.Log($"Bad ending done, retrying night {gpm.currentNight}");
                SceneTransition.Instance.LoadScene(gameplaySceneName);
                break;
        }
    }
}