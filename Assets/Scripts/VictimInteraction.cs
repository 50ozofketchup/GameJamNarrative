using UnityEngine;
using TMPro;

public class VictimInteraction : MonoBehaviour
{
    public enum VictimState { Idle, MaskOn, SampleTaken, MaskRemoved }

    [Header("Settings")]
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private HotbarManager hotbar;
    [SerializeField] private TracingMinigameManager tracingManager;
    [SerializeField] private MinigameState minigameState;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI promptText;

    private VictimState _state = VictimState.Idle;

    private void Update()
    {
        bool inRange = IsVictimInRange(out RaycastHit hit);

        UpdatePrompt(inRange);

        if (inRange && Input.GetKeyDown(interactKey))
            HandleInteract();
    }

    private void HandleInteract()
    {
        switch (_state)
        {
            case VictimState.Idle:
                if (hotbar.SelectedIndex == 0)
                {
                    _state = VictimState.MaskOn;
                    Debug.Log("Mask applied to victim.");
                }
                else
                {
                    Debug.Log("Must use item 1 first.");
                }
                break;

            case VictimState.MaskOn:
                if (hotbar.SelectedIndex == 1)
                {
                    Debug.Log("Starting tracing minigame.");
                    tracingManager.Open();
                }
                else
                {
                    Debug.Log("Must use item 2 to take sample.");
                }
                break;

            case VictimState.SampleTaken:
                _state = VictimState.MaskRemoved;
                minigameState.secondMinigameCompleted = true;
                Debug.Log("Mask removed. Second minigame complete. Head to the exit.");
                break;
        }
    }

    public void OnTracingCompleted()
    {
        _state = VictimState.SampleTaken;
        Debug.Log("Sample taken. Remove the mask now.");
    }

    private void UpdatePrompt(bool inRange)
    {
        if (promptText == null) return;

        if (!inRange)
        {
            promptText.text = "";
            return;
        }

        promptText.text = _state switch
        {
            VictimState.Idle => hotbar.SelectedIndex == 0 ? "Press F to apply mask" : "Equip item 1 first",
            VictimState.MaskOn => hotbar.SelectedIndex == 1 ? "Press F to take sample" : "Equip item 2",
            VictimState.SampleTaken => "Press F to remove mask",
            VictimState.MaskRemoved => "Leave through the exit",
            _ => ""
        };
    }

    private bool IsVictimInRange(out RaycastHit hit)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out hit, interactDistance))
            return hit.transform == transform;
        return false;
    }
}