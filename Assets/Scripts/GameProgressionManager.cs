using UnityEngine;
using Ink.Runtime;

public class GameProgressionManager : MonoBehaviour
{
    public static GameProgressionManager Instance { get; private set; }

    [Header("Opening")]
    public TextAsset[] openingSegments;

    [Header("Night Outros")]
    public TextAsset[] night1Segments;
    public TextAsset[] night2Segments;
    public TextAsset[] night3Segments;
    public TextAsset[] night4Segments;
    public TextAsset[] night5Segments;

    [HideInInspector] public int currentNight = 1;
    [HideInInspector] public bool isOpening = true;

    public TextAsset[] GetCurrentSegments()
    {
        if (isOpening) return openingSegments;

        return currentNight switch
        {
            1 => night1Segments,
            2 => night2Segments,
            3 => night3Segments,
            4 => night4Segments,
            5 => night5Segments,
            _ => null
        };
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}