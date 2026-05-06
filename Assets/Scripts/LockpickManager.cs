using UnityEngine;

public class LockpickManager : MonoBehaviour
{
    [SerializeField] private KeyCode openKey = KeyCode.E;
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private TimingMinigame minigame;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;

    private void Start()
    {
        minigamePanel.SetActive(false);
        minigame.OnSuccess += HandleSuccess;
        minigame.OnFail += HandleFail;
    }

    private void Update()
    {
        if (Input.GetKeyDown(openKey) && !minigamePanel.activeSelf)
            Open();
    }

    public void Open()
    {
        minigamePanel.SetActive(true);
        playerMovement.enabled = false;
        mouseLook.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        minigame.StartGame();
    }

    private void Close()
    {
        minigamePanel.SetActive(false);
        playerMovement.enabled = true;
        mouseLook.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleSuccess()
    {
        Close();
        // door.Open() goes here later
    }

    private void HandleFail() => Close();

    private void OnDestroy()
    {
        minigame.OnSuccess -= HandleSuccess;
        minigame.OnFail -= HandleFail;
    }
}