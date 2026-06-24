using System.Collections;
using UnityEngine;
using TMPro;
public class InternalMonologue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monologueText;
    [SerializeField] private float displayDuration = 5f;
    [TextArea][SerializeField] private string message = "I need to get inside. The door should be lockable from the outside...";

    private static bool _hasShown = false; // shared across all instances, ensures this monologue only ever plays once per game session

    private void Start()
    {
        if (_hasShown)
        {
            gameObject.SetActive(false); // already shown before, skip
            return;
        }
        StartCoroutine(ShowMessage());
    }

    private IEnumerator ShowMessage()
    {
        _hasShown = true;
        monologueText.text = message; // display the line
        yield return new WaitForSeconds(displayDuration); // wait before hiding
        gameObject.SetActive(false);
    }
}