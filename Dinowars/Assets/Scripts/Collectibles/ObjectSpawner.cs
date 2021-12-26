using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField]
    private Transform[] spawners;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private int spawnInterval;

    public static Dictionary<int, int> hkpositions;
    public static Dictionary<int, bool> pointStatus;
    [Server]
    public override void OnStartServer()
    {
        DinowarsNetworkManager.OnServerReadied += InitHealthKitMap;
        
    }

    [Server]
    private void InitHealthKitMap(NetworkConnection conn)
    {
        
        pointStatus = new Dictionary<int, bool>();
        hkpositions = new Dictionary<int, int>();
        for (int i = 0; i < spawners.Length; i++)
        {
            pointStatus[i] = true; 
        }
       
        StartCoroutine(SpawnCoroutine());
    }

    [Server]
    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        SpawnKit();
        StartCoroutine(SpawnCoroutine());
    }
    [Server]
    private void SpawnKit()
    { 
        List<int> avaliableSlots = FindEmptySlots();

        if(avaliableSlots.Count > 0)
        {
            int randomSlotIndex = Random.Range(0, avaliableSlots.Count - 1);
            int randomPointIndex = avaliableSlots[randomSlotIndex];
            //healthKitMap[randomPointIndex].SetActive(true);
            Debug.Log(randomPointIndex);
            spawnOnClients(spawners[randomPointIndex],randomPointIndex);
            pointStatus[randomPointIndex] = false;

        }
    }
    [Server]
    private void spawnOnClients(Transform transform, int index)
    {   

        GameObject hk = Instantiate(objectPrefab,transform);
        NetworkServer.Spawn(hk);
        hkpositions.Add(hk.GetInstanceID(),index);
        


    }
    [Server]
    public static void hkPickedUp(int id)
    {
        int emptySlotIndex = hkpositions[id];
        pointStatus[emptySlotIndex] = true;
    }

    [Server]
    private List<int> FindEmptySlots()
    {
        List<int> emptySlots = new List<int>();

        foreach(int index in pointStatus.Keys)
        {
            if (pointStatus[index])
                emptySlots.Add(index);
        }

        return emptySlots;
    }

    /*private void PrintMap()
    {
        string message = "";

        message =  message + "Values =====================\n";
        foreach(var key in healthKitMap.Keys)
        {
            message = message + string.Format("{0} - {1}", key, healthKitMap[key]) + "\n"; 
        }
        message = message + "=============================";

        Debug.Log(message);
    }*/


}
