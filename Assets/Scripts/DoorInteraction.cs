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

        // show prompt only if door is locked, in range, and minigame isn't already open
        if (promptText != null)
            promptText.text = inRange && lockpickManager.IsLocked && !minigamePanel.activeSelf ? "Press E to lockpick" : "";

        // start lockpicking on key press
        if (inRange && lockpickManager.IsLocked && Input.GetKeyDown(interactKey))
            lockpickManager.Open();

        Ray debugRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    }

    private bool IsDoorInRange()
    {
        // cast a ray from camera forward, check if it hits this door's collider
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            return hit.transform.GetComponentInParent<DoorInteraction>() == this;
        return false;
    }
}