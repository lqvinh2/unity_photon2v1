

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class VVEnemy : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{

    [SerializeField]
    float amplitude = 3;

    [SerializeField]
    float frequnce = 6;

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("OnOwnershipRequest");
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("OnOwnershipTransfered");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            float x = transform.position.x;
            float y = Mathf.Sin(Time.time * frequnce) * amplitude;
            float z = transform.position.z;
            transform.position = new Vector3(x, y, z);
        }
      
    }
}
