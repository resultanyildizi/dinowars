using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawners;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private int spawnInterval;

    private Dictionary<int, GameObject> healthKitMap;

    private GameObject healthKit;

    private void Awake()
    {
        InitHealthKitMap();
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        SpawnKit();
        PrintMap();
        StartCoroutine(SpawnCoroutine());
    }

    private void SpawnKit()
    { 
        List<int> avaliableSlots = FindEmptySlots();

        if(avaliableSlots.Count > 0)
        {
            int randomSlotIndex = Random.Range(0, avaliableSlots.Count - 1);
            int randomPointIndex = avaliableSlots[randomSlotIndex];
            Transform randomPoint = spawners[randomPointIndex];
            GameObject healthKit = Instantiate(objectPrefab, randomPoint);
            Debug.Log( randomPoint);
            healthKitMap[randomPointIndex] = healthKit;
        }
    }

    private List<int> FindEmptySlots()
    {
        List<int> emptySlots = new List<int>();

        foreach(int key in healthKitMap.Keys)
        {
            if (healthKitMap[key] == null)
                emptySlots.Add(key);
        }

        return emptySlots;
    }

    private void InitHealthKitMap()
    {
        healthKitMap = new Dictionary<int, GameObject>();
        for (int i = 0; i < spawners.Length; i++)
        {
            healthKitMap[i] = null;
        }
    }

    private void PrintMap()
    {
        string message = "";

        message =  message + "Values =====================\n";
        foreach(var key in healthKitMap.Keys)
        {
            message = message + string.Format("{0} - {1}", key, healthKitMap[key]) + "\n"; 
        }
        message = message + "=============================";

        Debug.Log(message);
    }


}
