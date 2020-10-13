using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class CowBoy : MonoBehaviourPun {

    public float MoveSpeed = 5;
    public GameObject playerCam;
    public SpriteRenderer sprite;
    public PhotonView photonview;
    public  Animator anim;
    private bool AllowMoving = true;

    public GameObject BulletePrefab;
    public Transform BulleteSpawnPointRight;
    public Transform BulleteSpawnPointleft;

    public Text PlayerName;
    public bool IsGrounded = false;
    public bool DisableInputs = false;
    private Rigidbody2D rb;
    public float jumpForce;

    public string MyName;
   
    //set this variable to true to enable mobile input in editor
    public bool EnableEditorInput = false;

    Vector3 Mobilemovement = Vector3.zero;

    // Use this for initialization
    void Awake ()
    {
        if (photonView.IsMine)
        {
            GameManager.instance.LocalPlayer = this.gameObject;
            playerCam.SetActive(true);
            playerCam.transform.SetParent(null, false);
            PlayerName.text = "You : "+PhotonNetwork.NickName;
            PlayerName.color = Color.green;
            MyName = PhotonNetwork.NickName;
        }
        else
        {
            PlayerName.text = photonview.Owner.NickName;
           
            PlayerName.color = Color.red;

        }

    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine&& !DisableInputs)
        {
            checkInputs();
        }
    }

  

    private void checkInputs()
    {
      //  var movement;
        if (AllowMoving)
        {
            if (EnableEditorInput && Application.platform == RuntimePlatform.WindowsEditor)
            {
                var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
                transform.position += movement * MoveSpeed * Time.deltaTime;

            }
            else {
                transform.position += Mobilemovement * MoveSpeed * Time.deltaTime;
                
            }

        }
        if (Input.GetKeyDown(KeyCode.RightControl) && anim.GetBool("IsMove") == false)
        {
            shot();
        }
        else if (Input.GetKeyUp(KeyCode.RightControl))
        {
            anim.SetBool("IsShot", false);
            AllowMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            anim.SetBool("IsMove", true);
        }
        if (Input.GetKeyDown(KeyCode.D) && anim.GetBool("IsShot") == false)
        {

            //FlipSprite_Right()
            playerCam.GetComponent<CameraFollow2D>().offset = new Vector3(1.3f, 1.53f, 0);
            photonview.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMove", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && anim.GetBool("IsShot") == false)
        {

            //FlipSprite_Left()
            playerCam.GetComponent<CameraFollow2D>().offset = new Vector3(-1.3f, 1.53f, 0);


            photonview.RPC("FlipSprite_Left", RpcTarget.AllBuffered);

        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("IsMove", false);
        }
    }

    public void shot()
    {
        if (anim.GetBool("IsMove") == true)
            return;

        if (sprite.flipX == false)
        {
            GameObject bullete = PhotonNetwork.Instantiate(BulletePrefab.name, new Vector2(BulleteSpawnPointRight.position.x, BulleteSpawnPointRight.position.y), Quaternion.identity, 0);
            bullete.GetComponent<Bullete>().localPlayerObj = this.gameObject;
        }
        
        if (sprite.flipX == true)
        {
            GameObject bullete = PhotonNetwork.Instantiate(BulletePrefab.name, new Vector2(BulleteSpawnPointleft.position.x, BulleteSpawnPointleft.position.y), Quaternion.identity, 0);
            bullete.GetComponent<Bullete>().localPlayerObj = this.gameObject;

            bullete.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);
        }
        
        anim.SetBool("IsShot", true);
        AllowMoving = false;

    }
    public void shotUp() {
        anim.SetBool("IsShot", false);
        AllowMoving = true;
    }
    [PunRPC]
    private void FlipSprite_Right()
    {
        sprite.flipX = false;
    }
    [PunRPC]
    private void FlipSprite_Left()
    {
        sprite.flipX = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            IsGrounded = true;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            IsGrounded = false;
        }
    }

   public void Jump()
    {
        if(IsGrounded)
        rb.AddForce(new Vector2(0, jumpForce * Time.deltaTime));
    }

    #region MOBILE INPUTS

    public void On_RightMove()
    {
        anim.SetBool("IsMove", true);
        if (anim.GetBool("IsShot") == false)
        {

            playerCam.GetComponent<CameraFollow2D>().offset = new Vector3(1.3f, 1.53f, 0);
            photonview.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
        }
        Mobilemovement = new Vector3(1, 0, transform.position.z);

    }
    public void On_LeftMove()
    {
        anim.SetBool("IsMove", true);
        if (anim.GetBool("IsShot") == false) { 
        playerCam.GetComponent<CameraFollow2D>().offset = new Vector3(-1.3f, 1.53f, 0);
        photonview.RPC("FlipSprite_Left", RpcTarget.AllBuffered);
        }
        Mobilemovement = new Vector3(-1, 0, transform.position.z);

    }
    public void On_PointerExit()
    {
        anim.SetBool("IsMove", false);

        Mobilemovement = new Vector3(0, 0, transform.position.z);

    }
    #endregion
}
