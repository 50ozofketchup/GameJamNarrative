using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float speed = 2f;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private bool _isOpen = false;

    private void Start()
    {
        _closedRotation = transform.rotation;
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f));
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            _isOpen ? _openRotation : _closedRotation,
            speed * Time.deltaTime
        );
    }

    public void Open() => _isOpen = true;
    public void Close() => _isOpen = false;
}
