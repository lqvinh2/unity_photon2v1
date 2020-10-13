using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class VVMobileInputScript : MonoBehaviour
{
    public GameObject canvasMobileInput;

    // ref to local player
    public GameObject localPlayer;
    public Button btn_right;
    public Button btn_left;

    Color color_highlightedColor_origin;

    void Start()
    {
        FindLocalPlayer();
    }

    private void Update()
    {
        if (localPlayer == null)
            FindLocalPlayer();

        if (localPlayer != null)
        {
            if (localPlayer.GetComponent<VVCowBoy>().isMobileInput == false)
                canvasMobileInput.SetActive(false);
            else
                canvasMobileInput.SetActive(true);
        }
    }

    void FindLocalPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // find local player from all the players in game scene
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                var colors = btn_right.colors;
                color_highlightedColor_origin = colors.highlightedColor;

                localPlayer = player;
                break;
            }
        }
    }

    public void On_RightMove()
    {
        if (localPlayer != null)
        {
            var colors_btn_R = btn_right.colors;
            colors_btn_R.normalColor = Color.red;
            colors_btn_R.highlightedColor = new Color32(255, 100, 100, 255);
            btn_right.colors = colors_btn_R;

            var colors_btn_L = btn_left.colors;
            colors_btn_L.normalColor = Color.white;
            colors_btn_L.highlightedColor = color_highlightedColor_origin;
            btn_left.colors = colors_btn_L;

            localPlayer.GetComponent<VVCowBoy>().On_RightMove();
        }
    }

    public void On_LeftMove()
    {
        if (localPlayer != null)
        {
            var colors_btn_R = btn_right.colors;
            colors_btn_R.normalColor = Color.white;
            colors_btn_R.highlightedColor = color_highlightedColor_origin;
            btn_right.colors = colors_btn_R;

            var colors_btn_L = btn_left.colors;
            colors_btn_L.normalColor = Color.red;
            colors_btn_L.highlightedColor = new Color32(255, 100, 100, 255);
            btn_left.colors = colors_btn_L;

            localPlayer.GetComponent<VVCowBoy>().On_LeftMove();
        }
    }

    public void On_PointerExit()
    {
        if (localPlayer != null)
        {
            var colors_btn_R = btn_right.colors;
            colors_btn_R.normalColor = Color.white;
            colors_btn_R.highlightedColor = color_highlightedColor_origin;
            btn_right.colors = colors_btn_R;

            var colors_btn_L = btn_left.colors;
            colors_btn_L.normalColor = Color.white;
            colors_btn_L.highlightedColor = color_highlightedColor_origin;
            btn_left.colors = colors_btn_L;

            localPlayer.GetComponent<VVCowBoy>().On_PointerExit();
        }
    }
    
    public void Jump()
    {
        if (localPlayer != null)
        {
            localPlayer.GetComponent<VVCowBoy>().Jump();
        }
    }

    public void ShootUp()
    {
        if (localPlayer != null)
        {
            localPlayer.GetComponent<VVCowBoy>().On_Mobile_ShootUp();
        }
    }

    public void ShootHold()
    {
        if (localPlayer != null)
        {
            localPlayer.GetComponent<VVCowBoy>().On_Mobile_ShootHold();
        }
    }


    public void ShootRelease()
    {
        if (localPlayer != null)
        {
            localPlayer.GetComponent<VVCowBoy>().On_Mobile_ShootRelease();
        }
    }

    public void ShowPlayerInGame()
    {
        VVGameManager.instance.cpCanvas.SetActive(true);
    }
    public void Exist_ShowPlayerInGame()
    {
        VVGameManager.instance.cpCanvas.SetActive(false);
    }

    public void Show_Panel_Exit()
    {
        VVGameManager.instance.ToggleLeaveScreen();
    }

    public void On_SendChat()
    {
        if (localPlayer != null)
        {
            localPlayer.GetComponent<VVChatManager>().ChatSendMsg();
        }
    }
}
