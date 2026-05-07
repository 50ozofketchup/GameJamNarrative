using UnityEngine;

public class LockpickManager : MonoBehaviour
{
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private TimingMinigame minigame;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private TimerSystem timerSystem;
    [SerializeField] private Door door;

    private CharacterController _characterController;
    private bool _locked = true;

    public bool IsLocked => _locked;

    private void Start()
    {
        _characterController = playerMovement.GetComponent<CharacterController>();
        minigamePanel.SetActive(false);
        minigame.OnSuccess += HandleSuccess;
        minigame.OnFail += HandleFail;
    }

    public void Open()
    {
        minigamePanel.SetActive(true);
        playerMovement.inputLocked = true;
        mouseLook.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        minigame.StartGame();
    }

    public void ResetLock()
    {
        _locked = true;
        door.Close();
        playerMovement.inputLocked = false;
        mouseLook.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleSuccess()
    {
        _locked = false;
        door.Open();
        Close();
        timerSystem.StartTimer(playerMovement.transform.position, _characterController);
    }

    private void HandleFail() => Close();

    private void Close()
    {
        minigamePanel.SetActive(false);
        playerMovement.inputLocked = false;
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