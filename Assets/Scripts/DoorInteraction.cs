using UnityEngine;
using TMPro;

public class DoorInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LockpickManager lockpickManager;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject minigamePanel;

    private void Update()
    {
        bool inRange = IsDoorInRange();

        if (promptText != null)
            promptText.text = inRange && lockpickManager.IsLocked && !minigamePanel.activeSelf ? "Press E to lockpick" : "";

        if (inRange && lockpickManager.IsLocked && Input.GetKeyDown(interactKey))
            lockpickManager.Open();
    }

    private bool IsDoorInRange()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            return hit.transform == transform || hit.transform.parent == transform;
        return false;
    }
}