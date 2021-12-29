using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField]
    private Vector3[] spawners;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private int spawnInterval;

    //[SyncVar]
    private Dictionary<int, GameObject> healthKitMap;

    public override void OnStartServer()
    {
        InitHealthKitMap();
        //DinowarsNetworkManager.OnServerReadied += (conn) => InitHealthKitMap();
    }

    public override void OnStopServer()
    {
        DinowarsNetworkManager.OnServerReadied -= (conn) => InitHealthKitMap();
    }


    [Server]
    private void InitHealthKitMap()
    {
        healthKitMap = new Dictionary<int, GameObject>();

        for (int i = 0; i < spawners.Length; i++)
            healthKitMap.Add(i, null);
        

        StartCoroutine(SpawnCoroutine());
    }


    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        SpawnKit();
        StartCoroutine(SpawnCoroutine());
    }

    private void SpawnKit()
    {
        List<int> avaliableSlots = FindEmptySlots();

        Debug.Log("AVALIABLE SLOTS : " + avaliableSlots.Count);

        if (avaliableSlots.Count > 0)
        {
            int randomSlotIndex = Random.Range(0, avaliableSlots.Count - 1);
            int randomPointIndex = avaliableSlots[randomSlotIndex];
            healthKitMap[randomPointIndex] = Instantiate(objectPrefab, spawners[randomPointIndex], Quaternion.identity);
            NetworkServer.Spawn(healthKitMap[randomPointIndex]);

        }
    }

    private List<int> FindEmptySlots()
    {
        List<int> emptySlots = new List<int>();

        foreach (int key in healthKitMap.Keys)
        {
            if (healthKitMap[key] == null)
                emptySlots.Add(key);
        }

        

        return emptySlots;
    }

    //private void PrintMap()
    //{
    //    string message = "";

    //    message =  message + "Values =====================\n";
    //    foreach(var key in healthKitMap.Keys)
    //    {
    //        message = message + string.Format("{0} - {1}", key, healthKitMap[key]) + "\n"; 
    //    }
    //    message = message + "=============================";

    //    Debug.Log(message);
    //}


}
