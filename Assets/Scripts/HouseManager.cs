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
    [SerializeField] private TimerSystem timerSystem;

    private void Start()
    {
        int nightIndex = GameProgressionManager.Instance != null
            ? GameProgressionManager.Instance.currentNight - 1
            : 0;

        nightIndex = Mathf.Clamp(nightIndex, 0, houses.Length - 1);
        Debug.Log($"Current night from GPM: {GameProgressionManager.Instance.currentNight}, nightIndex: {nightIndex}");

        for (int i = 0; i < houses.Length; i++)
        {
            bool isTargetHouse = i == nightIndex;

            if (houses[i].lightsParent != null)
                houses[i].lightsParent.SetActive(!isTargetHouse);

            if (houses[i].doorPivot != null)
            {
                houses[i].doorPivot.GetComponentInChildren<DoorInteraction>().enabled = isTargetHouse;

                if (isTargetHouse)
                {
                    LockpickManager lm = houses[i].doorPivot.GetComponent<LockpickManager>();
                    Debug.Log($"Setting LockpickManager from house {i}, lm null: {lm == null}");
                    nightCompleteTrigger.SetLockpickManager(lm);
                    timerSystem.SetLockpickManager(lm);
                }
            }
        }
    }
}