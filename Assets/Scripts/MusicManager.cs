using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; } // singleton

    [SerializeField] private AudioSource audioSource;

    [Header("BGM")]
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip storyMusic;

    [Header("Scene Names")]
    [SerializeField] private string gameplaySceneName = "GameplayScene";
    [SerializeField] private string storySceneName = "StoryScene";

    private void Awake()
    {
        // enforce singleton, persist across scene loads
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded; // re-check music whenever a new scene loads
    }

    private void Start()
    {
        PlayForCurrentScene(SceneManager.GetActiveScene().name); // handle music for the scene we started in
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayForCurrentScene(scene.name);
    }

    // picks correct clip for scene name and plays it if not already playing
    private void PlayForCurrentScene(string sceneName)
    {
        AudioClip clip = sceneName == gameplaySceneName ? gameplayMusic :
                         sceneName == storySceneName ? storyMusic : null;

        if (clip == null || audioSource.clip == clip) return; // no clip for this scene, or already playing it
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // unsubscribe to avoid dangling reference
    }
}