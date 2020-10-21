using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VVItem : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject localPlayerGotItem;
    string namePlayerGotItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!photonView.IsMine)
            return;

        PhotonView player = collision.gameObject.GetComponent<PhotonView>();

        if (player != null && player.tag.ToLower() == "player")
        {
      
            localPlayerGotItem = collision.gameObject;

            namePlayerGotItem = localPlayerGotItem.GetComponent<VVCowBoy>().MyName;
            this.gameObject.name = "item_" + (localPlayerGotItem.GetComponent<VVCowBoy>().listItemOnHand.Count + 1).ToString();

            Destroy(this.gameObject);
            this.GetComponent<PhotonView>().RPC("CollectItem", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void CollectItem()
    {
        Destroy(this.gameObject);

        if (localPlayerGotItem)
        {
            ItemInfo itemInfo = new ItemInfo();
            itemInfo.name = this.gameObject.name;
            localPlayerGotItem.GetComponent<VVCowBoy>().OKA = true;
            localPlayerGotItem.GetComponent<VVCowBoy>().listItemOnHand.Add(itemInfo);
        }

    }




}
