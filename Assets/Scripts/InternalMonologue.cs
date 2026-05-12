using System.Collections;
using UnityEngine;
using TMPro;

public class InternalMonologue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monologueText;
    [SerializeField] private float displayDuration = 5f;
    [TextArea][SerializeField] private string message = "I need to get inside. The door should be lockable from the outside...";

    private static bool _hasShown = false;

    private void Start()
    {
        if (_hasShown)
        {
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(ShowMessage());
    }

    private IEnumerator ShowMessage()
    {
        _hasShown = true;
        monologueText.text = message;
        yield return new WaitForSeconds(displayDuration);
        gameObject.SetActive(false);
    }
}