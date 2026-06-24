using UnityEngine;
public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private MouseLook mouseLook;

    private void Start()
    {
        var gpm = GameProgressionManager.Instance;

        // if returning from bad ending 1, spawn at the special return point instead of default spawn
        if (gpm != null && gpm.currentMode == GameProgressionManager.StoryMode.BadEnding1)
        {
            PlacePlayer(gpm.badEnding1ReturnPosition);
        }
        else
        {
            PlacePlayer(transform.position); // default spawn point
        }

        ResetCameraRotation();
    }

    private void PlacePlayer(Vector3 position)
    {
        // disable CharacterController briefly so it doesn't fight the manual position change
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.position = position;
        cc.enabled = true;
    }

    private void ResetCameraRotation()
    {
        playerCamera.localRotation = Quaternion.identity;
        if (mouseLook != null)
            mouseLook.ResetRotation();
    }
}