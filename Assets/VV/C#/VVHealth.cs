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

    public VVCowBoy playerScript;
    public GameObject KillGotKilledText;

    // Start is called before the first frame update
    void Start()
    {
        //Vector2 pos = new Vector2(KillGotKilledText.transform.position.x, KillGotKilledText.transform.position.y);

        //GameObject go = Instantiate(KillGotKilledText, pos, Quaternion.identity);
        //go.transform.SetParent(GameManager.instance.KillGotKilledFeedBox.transform, false);
        //go.GetComponent<Text>().text = "You Got Killed by : " + name;
        //go.GetComponent<Text>().color = Color.red;
        //Destroy(go, 3);
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


    [PunRPC]
    void YouGotKilledBy(string name)
    { 
        InstantiateKillText(Color.red, "You Got Killed by : " + name);
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
