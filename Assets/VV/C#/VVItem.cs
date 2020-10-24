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
        if (photonView.IsMine)
        {
            // StartCoroutine(destroyBullete());
        }
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject localPlayerGotItem;
    string namePlayerGotItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView player = collision.gameObject.GetComponent<PhotonView>();
        if (player.IsMine && player.tag.ToLower() == "player")
        {
            // this.gameObject.SetActive(false);
            localPlayerGotItem = collision.gameObject;
            namePlayerGotItem = localPlayerGotItem.GetComponent<VVCowBoy>().MyName;
            this.gameObject.name = "item_" + (localPlayerGotItem.GetComponent<VVCowBoy>().listItemOnHand.Count + 1).ToString();


            // prevent pick up item in case : at the same time 2 player pick item
            //                                we just need decide only one can pick
            this.GetComponent<PhotonView>().RPC("BroadcastAllPlayerItemPicked", RpcTarget.AllBuffered, this.gameObject.name, namePlayerGotItem);
            //StartCoroutine(destroyItem(gameObject.name));
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered, gameObject.name);
        }

    }

    [PunRPC]
    void BroadcastAllPlayerItemPicked(string itemName, string namePlayerPickItem)
    {
        if (VVGameManager.instance)
        {
            var dic = VVGameManager.instance.itemName_pick_by_ListPlayer;

            if (dic.ContainsKey(itemName))
            {
                var players = dic[itemName];
                players.Add(namePlayerPickItem);

                dic[itemName] = players;
            }
            else
            {
                List<string> tt = new List<string>();
                tt.Add(namePlayerPickItem);
                dic.Add(itemName, tt);
            }
         }
    }

    float DestroyTime = 10F;
    IEnumerator destroyItem(string itemName)
    {
        yield return new WaitForSeconds(DestroyTime);
        this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered, itemName);
    }


    [PunRPC]
    void Destroy(string itemName)
    {
        try
        {
            if (VVGameManager.instance)
            {
                 var dic = VVGameManager.instance.itemName_pick_by_ListPlayer;

                if (dic.ContainsKey(itemName))
                {
                    var players = dic[itemName];
                    dic.Remove(itemName);

                    // cùng time 2 Player cùng lúc nhận được item thì biến này sẽ khác null 
                    // lúc này cần xử lý lấy thằng đầu tiên trong mảng trước.
                    if (localPlayerGotItem)
                    {
                        string namePlayerLocal = localPlayerGotItem.GetComponent<VVCowBoy>().MyName;
                        string namePlayerPickUpFirst = players[0];

                        if (namePlayerLocal == namePlayerPickUpFirst)
                        {
                            ItemInfo itemInfo = new ItemInfo();
                            itemInfo.name = this.gameObject.name;
                            localPlayerGotItem.GetComponent<VVCowBoy>().OKA = true;
                            localPlayerGotItem.GetComponent<VVCowBoy>().listItemOnHand.Add(itemInfo);
                        }
                    }
                }

            }

            Destroy(this.gameObject);
        }
        catch 
        {

          
        }
       
    }



}
