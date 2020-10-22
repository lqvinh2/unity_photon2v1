using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VVBullet : MonoBehaviourPun
{

    public bool MovingDirection;
    public float MoveSpeed = 8;

    public float DestroyTime = 2f;

    [SerializeField]
    float bulleteDamage = 0.01f;

    public string killerName;
    public GameObject localPlayerObj;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            killerName = localPlayerObj.GetComponent<VVCowBoy>().MyName;
            StartCoroutine(destroyBullete());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!MovingDirection)
        {
            transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
        }
    }

    IEnumerator destroyBullete()
    {
        yield return new WaitForSeconds(DestroyTime);
        this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void Destroy()
    {
        Destroy(this.gameObject);
    }

    [PunRPC]
    public void ChangeDirection()
    {
        MovingDirection = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.IsMine))
        {
            if (target.tag == "Player")
            {

                target.RPC("HealthUpdate", RpcTarget.AllBuffered, bulleteDamage);
                target.GetComponent<VVHurtEffect>().GotHit();

                if (target.GetComponent<VVHealth>().health <= 0)
                {
                    Player GotKilled = target.Owner;
                    target.RPC("YouGotKilledBy", GotKilled, killerName);
                    target.RPC("YouKilled", localPlayerObj.GetComponent<PhotonView>().Owner, target.Owner.NickName);
                    target.RPC("GetScore", RpcTarget.OthersBuffered, killerName + " kill " + target.Owner.NickName);
                    collision.gameObject.SetActive(false);
                }
            }
            Destroy(this.gameObject);
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.OthersBuffered);

        }
    }

}
