using UnityEngine;
public class Door : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float openAngle = 90f; // how far the door swings, in degrees
    [SerializeField] private float speed = 2f; // rotation lerp speed

    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private bool _isOpen = false;

    private void Start()
    {
        _closedRotation = transform.rotation; // store starting rotation as "closed"
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f)); // rotate around Y axis to get "open" pose
    }

    private void Update()
    {
        // smoothly rotate toward open or closed target each frame
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            _isOpen ? _openRotation : _closedRotation,
            speed * Time.deltaTime
        );
    }

    public void Open() => _isOpen = true; // called externally (e.g. by interaction script)
    public void Close() => _isOpen = false;
}