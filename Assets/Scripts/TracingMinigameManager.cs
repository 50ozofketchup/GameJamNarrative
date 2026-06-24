using UnityEngine;
public class TracingMinigameManager : MonoBehaviour
{
    [SerializeField] private GameObject tracingPanel;
    [SerializeField] private TracingMinigame tracingMinigame;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private VictimInteraction victimInteraction;

    private void Start()
    {
        tracingPanel.SetActive(false);

        // subscribe to minigame result events
        tracingMinigame.OnSuccess += HandleSuccess;
        tracingMinigame.OnFail += HandleFail;
    }

    // called when player interacts to start tracing
    public void Open()
    {
        tracingPanel.SetActive(true);
        playerMovement.inputLocked = true;
        mouseLook.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        tracingMinigame.StartMinigame();
        Debug.Log("Tracing minigame opened.");
    }

    // shared cleanup, hide panel and give player contorl back
    private void Close()
    {
        tracingPanel.SetActive(false);
        playerMovement.inputLocked = false;
        mouseLook.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleSuccess()
    {
        Debug.Log("Tracing complete. Sample taken successfully.");
        Close();
        victimInteraction.OnTracingCompleted();
    }

    private void HandleFail()
    {
        Debug.Log("Tracing failed. Too many mistakes.");
        Close();
    }

    private void OnDestroy()
    {
        // unsubscribe to avoid dangling references
        tracingMinigame.OnSuccess -= HandleSuccess;
        tracingMinigame.OnFail -= HandleFail;
    }
}