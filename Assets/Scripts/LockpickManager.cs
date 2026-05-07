using UnityEngine;

public class LockpickManager : MonoBehaviour
{
    [SerializeField] private KeyCode openKey = KeyCode.E;
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private TimingMinigame minigame;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private TimerSystem timerSystem;

    private CharacterController _characterController;
    private bool _locked = true;

    private void Start()
    {
        _characterController = playerMovement.GetComponent<CharacterController>();
        minigamePanel.SetActive(false);
        minigame.OnSuccess += HandleSuccess;
        minigame.OnFail += HandleFail;
    }

    private void Update()
    {
        if (Input.GetKeyDown(openKey) && !minigamePanel.activeSelf && _locked)
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

    // Called by TimerSystem when the timer expires
    public void ResetLock()
    {
        _locked = true;
        playerMovement.enabled = true;
        mouseLook.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleSuccess()
    {
        _locked = false;
        Close();
        timerSystem.StartTimer(playerMovement.transform.position, _characterController);
    }

    private void HandleFail() => Close();

    private void Close()
    {
        minigamePanel.SetActive(false);
        playerMovement.enabled = true;
        mouseLook.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        minigame.OnSuccess -= HandleSuccess;
        minigame.OnFail -= HandleFail;
    }
}