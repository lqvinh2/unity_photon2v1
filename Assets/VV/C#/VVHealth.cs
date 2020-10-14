using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class VVHealth : MonoBehaviourPun
{
    const float max_health = 1F;
    public Image fillImage;
    public float health = max_health;
 


    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public BoxCollider2D collider;
    public GameObject playerCanvas;

    public GameObject UI_img_player_A_killYou;

    public VVCowBoy playerScript;
    public GameObject KillGotKilledText;

    // Start is called before the first frame update
    void Start()
    {
       
    }


    [PunRPC]
    public void HealthUpdate(float damage)
    {
        fillImage.fillAmount -= damage;
        health = fillImage.fillAmount;
        CheckHealth();
    }

    public void CheckHealth()
    {
        if (photonView.IsMine && health <= 0)
        {
            VVGameManager.instance.EnableRespawn();
            playerScript.DisableInputs = true;
            this.GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void death()
    {
        rb.gravityScale = 0;
        collider.enabled = false;
        sr.enabled = false;
        playerCanvas.SetActive(false);
    }

    [PunRPC]
    public void Revive()
    {
        // set from VVBullet ->  OnTriggerEnter2D(Collider2D collision) {collision.gameObject.SetActive(false);}
        gameObject.SetActive(true);
        gameObject.GetComponent<VVHurtEffect>().ChangeColor_WHITE();

        rb.gravityScale = 1;
        collider.enabled = true;
        sr.enabled = true;
        playerCanvas.SetActive(true);
        fillImage.fillAmount = max_health;
        health = max_health;
    }

    public void EnableInputs()
    {
        playerScript.DisableInputs = false;
    }

    enum typeKill
    {
        killer,
        gotKill
       
    }



    [PunRPC]
    void YouGotKilledBy(string name)
    {
        GameObject can = GameObject.Find("Canvas_hole_game");

        Vector2 pos = new Vector2(UI_img_player_A_killYou.transform.position.x, UI_img_player_A_killYou.transform.position.y);
        GameObject go = Instantiate(UI_img_player_A_killYou, pos, Quaternion.identity);
        go.transform.SetParent(can.transform, false);
        go.transform.Find("txt_player_kill_you").GetComponent<Text>().text = name + " kill you !";

        Destroy(go, 3);
    }

    [PunRPC]
    void GetScore(string killerName_dieName)
    {
        InstantiateKillText(Color.red, killerName_dieName);
    }

    [PunRPC]
    void YouKilled(string name)
    {
        InstantiateKillText(Color.green, "You Killed : " + name);
    }

    void InstantiateKillText(Color c, string text)
    {
        Vector2 pos = new Vector2(KillGotKilledText.transform.position.x, KillGotKilledText.transform.position.y);
        GameObject go = Instantiate(KillGotKilledText, pos, Quaternion.identity);
        go.transform.SetParent(VVGameManager.instance.KillGotKilledFeedBox.transform, false);
        go.GetComponent<Text>().text = text;
        go.GetComponent<Text>().color = c;
        Destroy(go, 3);
    }



}
