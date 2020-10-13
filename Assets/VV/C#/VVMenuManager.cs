﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class VVMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject UserNameScreen, ConnectScreen;

    [SerializeField]
    private GameObject CreateUserNameButton;

    [SerializeField]

    private InputField UserNameInput, CreateRoomInput, JoinRoomInput;

    void Awake()
    {
        UserNameScreen.SetActive(false);
        ConnectScreen.SetActive(false);
        CreateUserNameButton.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("ok connected to lobby OnJoinedLobby  ");
        UserNameScreen.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom OK !!");

        // AssignTeam
        AssignTeam();
        PhotonNetwork.LoadLevel(1);
    }

    void AssignTeam()
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        int size = PhotonNetwork.PlayerList.GetLength(0);

        if (size % 2== 0)
        {
            hash.Add("Team", 0);
        }
        else
        {
            hash.Add("Team", 1);
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UIMethods
    public void OnClick_CreateNameBtn()
    {

        PhotonNetwork.NickName = UserNameInput.text;
        UserNameScreen.SetActive(false);
        ConnectScreen.SetActive(true);
       
    }

    public void OnNameField_Changed()
    {
        if (UserNameInput.text.Length > 2)
        {
            CreateUserNameButton.SetActive(true);
        }
        else
        {
            CreateUserNameButton.SetActive(false);
        }
    }

    public void Onclick_JoinRoom()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(JoinRoomInput.text, ro, TypedLobby.Default);

    }
    public void Onclick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(CreateRoomInput.text, new RoomOptions { MaxPlayers = 4 }, null);
    }
    #endregion UIMethods
}