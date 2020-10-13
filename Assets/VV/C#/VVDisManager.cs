using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class VVDisManager : MonoBehaviourPunCallbacks
{

    public GameObject DisUi;
    public GameObject MenuButton;
    public GameObject ReconnectButton;
    public Text StatusText;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // DontDestroyOnLoad(this.gameObject);
    }



    private void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            DisUi.SetActive(true);

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                ReconnectButton.SetActive(true);
                StatusText.text = "Please try to reconnect";
            }

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                MenuButton.SetActive(true);
                StatusText.text = "Please try to reconnect in the main menu";
            }
        }
    }

    //called by photon
    public override void OnConnectedToMaster()
    {
        if (DisUi.activeSelf)
        {
            MenuButton.SetActive(false);
            ReconnectButton.SetActive(false);
            DisUi.SetActive(false);
        }
    }

    public void OnClick_Recconect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnClick_Menu_goback()
    {
        PhotonNetwork.LoadLevel(0);
    }


}
