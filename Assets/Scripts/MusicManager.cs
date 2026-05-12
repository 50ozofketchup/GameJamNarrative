using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    [Header("BGM")]
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip storyMusic;

    [Header("Scene Names")]
    [SerializeField] private string gameplaySceneName = "GameplayScene";
    [SerializeField] private string storySceneName = "StoryScene";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayForCurrentScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayForCurrentScene(scene.name);
    }

    private void PlayForCurrentScene(string sceneName)
    {
        AudioClip clip = sceneName == gameplaySceneName ? gameplayMusic :
                         sceneName == storySceneName ? storyMusic : null;

        if (clip == null || audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}