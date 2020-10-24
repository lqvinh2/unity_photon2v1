using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class VVSpawnItemNetwork : MonoBehaviourPunCallbacks
{

    public GameObject itemPrefab;
    float timeSpawnItem = 0;
    const float max_timeSpawnItem = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }




    // Update is called once per frame
    void Update()
    {
        if (VVMenuManager.instance.room_creator != "")
        {
            timeSpawnItem = timeSpawnItem - Time.deltaTime;
            if (timeSpawnItem <= 0)
            {
                timeSpawnItem = max_timeSpawnItem;
                StartCoroutine(SpawnItem());
            }
        }
    }

    IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(0.1F);

        float randomPosition = UnityEngine.Random.Range(-10, 10);
        PhotonNetwork.Instantiate(itemPrefab.name, new Vector3(randomPosition, -0.51F, -1.370586F), Quaternion.identity, 0);
    }

}
