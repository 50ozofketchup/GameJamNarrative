using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; } // singleton

    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        // enforce singleton, persist across scene loads
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // start fully black (covers initial scene load)
        Color c = blackScreen.color;
        c.a = 1f;
        blackScreen.color = c;
    }

    public void FadeIn() // fade from black to clear
    {
        StartCoroutine(Fade(1f, 0f));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        yield return Fade(0f, 1f); // fade to black
        yield return SceneManager.LoadSceneAsync(sceneName); // load new scene
        yield return Fade(1f, 0f); // fade back in
    }

    // smoothly transitions blackScreen alpha from "from" to "to" over fadeDuration (fromfromtoto heh :p)
    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color c = blackScreen.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, elapsed / fadeDuration);
            blackScreen.color = c;
            yield return null;
        }
        c.a = to; // snap to exact final value
        blackScreen.color = c;
    }
}