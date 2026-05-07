using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private GameObject item1; // Anesthesia placeholder
    [SerializeField] private GameObject item2; // Cutting tool placeholder

    [Header("Hand Anchor")]
    [SerializeField] private Transform handAnchor; // Empty GameObject, child of camera, bottom right

    private GameObject[] _items;
    private int _selectedIndex = -1;

    public int SelectedIndex => _selectedIndex;

    private void Start()
    {
        _items = new GameObject[] { item1, item2 };

        foreach (var item in _items)
            if (item != null) item.SetActive(false);

        SelectItem(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectItem(1);
    }

    private void SelectItem(int index)
    {
        if (_selectedIndex == index) return;

        if (_selectedIndex >= 0 && _items[_selectedIndex] != null)
            _items[_selectedIndex].SetActive(false);

        _selectedIndex = index;

        if (_items[_selectedIndex] != null)
        {
            _items[_selectedIndex].SetActive(true);
            _items[_selectedIndex].transform.SetParent(handAnchor, false);
            _items[_selectedIndex].transform.localPosition = Vector3.zero;
            _items[_selectedIndex].transform.localRotation = Quaternion.identity;
        }
    }
}