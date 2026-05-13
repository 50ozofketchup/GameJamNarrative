using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private MouseLook mouseLook;

    private void Start()
    {
        var gpm = GameProgressionManager.Instance;

        if (gpm != null && gpm.currentMode == GameProgressionManager.StoryMode.BadEnding1)
        {
            PlacePlayer(gpm.badEnding1ReturnPosition);
        }
        else
        {
            PlacePlayer(transform.position);
        }

        ResetCameraRotation();
    }

    private void PlacePlayer(Vector3 position)
    {
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