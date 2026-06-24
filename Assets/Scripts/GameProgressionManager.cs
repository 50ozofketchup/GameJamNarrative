using UnityEngine;
public class GameProgressionManager : MonoBehaviour
{
    public enum StoryMode { Opening, NightOutro, BadEnding1, BadEnding2 }
    public static GameProgressionManager Instance { get; private set; } // singleton

    [Header("Opening")]
    public TextAsset[] openingSegments;

    [Header("Night Outros")]
    public TextAsset[] night1Segments;
    public TextAsset[] night2Segments;
    public TextAsset[] night3Segments;
    public TextAsset[] night4Segments;
    public TextAsset[] night5Segments;

    [Header("Bad Endings")]
    public TextAsset[] badEnding1Segments;
    public TextAsset[] badEnding2Segments;

    [HideInInspector] public int currentNight = 1;
    [HideInInspector] public StoryMode currentMode = StoryMode.Opening;
    [HideInInspector] public Vector3 badEnding1ReturnPosition; // where player respawns after bad ending 1

    // returns the correct Ink segment array based on current story mode/night
    public TextAsset[] GetCurrentSegments()
    {
        return currentMode switch
        {
            StoryMode.Opening => openingSegments,
            StoryMode.BadEnding1 => badEnding1Segments,
            StoryMode.BadEnding2 => badEnding2Segments,
            StoryMode.NightOutro => currentNight switch
            {
                1 => night1Segments,
                2 => night2Segments,
                3 => night3Segments,
                4 => night4Segments,
                5 => night5Segments,
                _ => null
            },
            _ => null
        };
    }

    private void Awake()
    {
        // enforce singleton, persist across scene loads
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}