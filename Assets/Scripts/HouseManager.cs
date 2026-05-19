using UnityEngine;

public class HouseManager : MonoBehaviour
{
    [System.Serializable]
    public class HouseData
    {
        public GameObject lightsParent;
        public GameObject doorPivot;
    }

    [SerializeField] private HouseData[] houses = new HouseData[5];
    [SerializeField] private NightCompleteTrigger nightCompleteTrigger;

    private void Start()
    {
        int nightIndex = GameProgressionManager.Instance != null
            ? GameProgressionManager.Instance.currentNight - 1
            : 0;

        nightIndex = Mathf.Clamp(nightIndex, 0, houses.Length - 1);

        for (int i = 0; i < houses.Length; i++)
        {
            bool isTargetHouse = i == nightIndex;

            if (houses[i].lightsParent != null)
                houses[i].lightsParent.SetActive(!isTargetHouse);

            if (houses[i].doorPivot != null)
            {
                houses[i].doorPivot.GetComponent<DoorInteraction>().enabled = isTargetHouse;

                if (isTargetHouse)
                    nightCompleteTrigger.SetLockpickManager(
                        houses[i].doorPivot.GetComponent<LockpickManager>());
            }
        }
    }
}