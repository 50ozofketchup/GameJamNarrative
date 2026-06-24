using UnityEngine;
public class LockpickManager : MonoBehaviour
{
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private TimingMinigame minigame;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private TimerSystem timerSystem;
    [SerializeField] private Door door;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorOpenSound;

    private CharacterController _characterController;
    private bool _locked = true;
    public bool IsLocked => _locked;

    private void Start()
    {
        _characterController = playerMovement.GetComponent<CharacterController>();
        minigamePanel.SetActive(false);

        // subscribe to minigame result events
        minigame.OnSuccess += HandleSuccess;
        minigame.OnFail += HandleFail;
    }

    // called when player interacts with the locked door
    public void Open()
    {
        minigamePanel.SetActive(true);
        playerMovement.inputLocked = true;
        mouseLook.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        minigame.StartGame();
    }

    // re-locks the door (e.g. for retry / next night)
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
        Close(); // close minigame UI, restore player control
        timerSystem.StartTimer(playerMovement.transform.position, _characterController);

        if (audioSource != null && doorOpenSound != null)
            audioSource.PlayOneShot(doorOpenSound);
    }

    private void HandleFail() => Close(); // failed attempt, just close minigame UI

    // shared cleanup: hide minigame UI and give player control back
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
        // unsubscribe to avoid dangling references
        minigame.OnSuccess -= HandleSuccess;
        minigame.OnFail -= HandleFail;
    }
}