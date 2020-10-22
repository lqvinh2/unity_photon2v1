using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class VVGameManager : MonoBehaviourPunCallbacks
{
    #region variable
    public GameObject cpCanvas;
    public VVConnectedPlayer cp;
    public GameObject playerPrefab;


    public GameObject canvas;
    public GameObject sceneCam;


    public Text spawnTimer;
    public GameObject respawnUI;

    private float TimeAmount = 3;
    private bool startRespawn;
    public Text pingrate;

    [HideInInspector]
    public GameObject LocalPlayer;
    public static VVGameManager instance = null;

    public GameObject LeaveScreen;

    public GameObject feedbox;
    public GameObject feedText_Prefab;

    public GameObject KillGotKilledFeedBox;

    public GameObject MobileInput;
    #endregion variable


    void Awake()
    {
        instance = this;
        itemName_pick_by_ListPlayer = new Dictionary<string, List<string>>();
        canvas.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        cp.AddLocalPlayer();
        cp.GetComponent<PhotonView>().RPC("UpdatePlayerList", RpcTarget.OthersBuffered, PhotonNetwork.NickName);

    }
    public Dictionary<string, List<string>> itemName_pick_by_ListPlayer;
    // Update is called once per frame
    void Update()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }

        if (LocalPlayer == null)
            return;

        if (startRespawn)
            StartRespawn();

        if (this.LocalPlayer.GetComponent<VVCowBoy>().isMobileInput == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleLeaveScreen();
            }

            if (Input.GetKey(KeyCode.Tab))
            {
                cpCanvas.SetActive(true);
            }
            else
                cpCanvas.SetActive(false);
        }
       

        pingrate.text = "Ping : " + PhotonNetwork.GetPing();
    }

    public void ToggleLeaveScreen()
    {
        if (LeaveScreen.activeSelf)
        {
            LeaveScreen.SetActive(false);
        }
        else
        {
            LeaveScreen.SetActive(true);
        }
    }

    public void btn_press_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }

    public void StartRespawn()
    {
        TimeAmount -= Time.deltaTime;
        spawnTimer.text = "Respawn in : " + TimeAmount.ToString("F0");

        if (TimeAmount <= 0)
        {
            respawnUI.SetActive(false);
            startRespawn = false;
            PlayerRelocation();
            LocalPlayer.GetComponent<VVHealth>().EnableInputs();
            LocalPlayer.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
        }
    }


    public void PlayerRelocation()
    {
        float randomPosition = Random.Range(-3, 3);
        LocalPlayer.transform.localPosition = new Vector2(randomPosition, 2);
    }

    public float respawnPlayer_time = 3;

    public void EnableRespawn()
    {
        TimeAmount = respawnPlayer_time;
        startRespawn = true;
        respawnUI.SetActive(true);
    }
    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-5, 5);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(playerPrefab.transform.position.x * randomValue, playerPrefab.transform.position.y), Quaternion.identity, 0);
       
        canvas.SetActive(false);
        sceneCam.SetActive(false);
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        
        GameObject go = Instantiate(feedText_Prefab, new Vector2(0f, 0f), Quaternion.identity);
        go.transform.SetParent(feedbox.transform);
        go.GetComponent<Text>().text = player.NickName + " has joined the game";
        Destroy(go, 3);

    }
    public override void OnPlayerLeftRoom(Player player)
    {
        cp.RemovePlayerList(player.NickName);
        GameObject go = Instantiate(feedText_Prefab, new Vector2(0f, 0f), Quaternion.identity);
        go.transform.SetParent(feedbox.transform);
        go.GetComponent<Text>().text = player.NickName + " has left the game";
        Destroy(go, 3);
    }



}
