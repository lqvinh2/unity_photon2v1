using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class VVSpawnItem_Enemy_Network : MonoBehaviourPunCallbacks
{
    
    public GameObject itemPrefab;
    float timeSpawnItem = 0;
    float timeSpawnENemy1 = 0;
    readonly float max_timeSpawnItem = 2;
    readonly float max_timeSpawnEnemy1 = 10;

    public GameObject enemy1_Prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }




    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            timeSpawnItem = timeSpawnItem - Time.deltaTime;
            if (timeSpawnItem <= 0)
            {
                timeSpawnItem = max_timeSpawnItem;
                StartCoroutine(SpawnItem());
            }

            timeSpawnENemy1 = timeSpawnENemy1 - Time.deltaTime;
            if (timeSpawnENemy1 <= 0)
            {
                timeSpawnENemy1 = max_timeSpawnEnemy1;
                StartCoroutine(SpawnEnemy1());
            }


        }
    }

    // spawn item hay enemy thi` du`ng PhotonNetwork.InstantiateRoomObject
    IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(0.1F);

        float randomPosition = UnityEngine.Random.Range(-10, 10);
        //PhotonNetwork.Instantiate(itemPrefab.name, new Vector3(randomPosition, -0.51F, -1.370586F), Quaternion.identity, 0);
        PhotonNetwork.InstantiateRoomObject(itemPrefab.name, new Vector3(randomPosition, -0.51F, -1.370586F), Quaternion.identity, 0);
    }

    IEnumerator SpawnEnemy1()
    {
        yield return new WaitForSeconds(0.1F);
        float randomPosition = UnityEngine.Random.Range(-10, 10);
        PhotonNetwork.InstantiateRoomObject(enemy1_Prefab.name, new Vector3(randomPosition, -0.51F, -1.370586F), Quaternion.identity);
    }

}
