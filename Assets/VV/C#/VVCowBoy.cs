using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class VVCowBoy : MonoBehaviourPun
{
    #region variable
    VVAnimationManager _anim = new VVAnimationManager();
    public float MoveSpeed = 5;
    public GameObject playerCam;
    public SpriteRenderer sprite;
    public PhotonView photonview1;
    //private bool AllowMoving = true;

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
    public bool isMobileInput = false;

    Vector3 Mobilemovement = Vector3.zero;

    
    const string ANIM_IDLE = "Idle";
    const string ANIM_RUN = "run";
    const string ANIM_JUMP = "jump";
    const string ANIM_SHOOT = "shoot";


    #endregion variable

    float time_next_shoot = 0.25F;

    const string anim_float_new1 = "new1";
    const string anim_float_new2 = "new2";
    const float anim_float_idle = 0;
    const float anim_float_run = 1;
    const float anim_float_shoot = 3;
    const float anim_float_jump = 2;

    void Awake()
    {
        if (photonView.IsMine)
        {
            VVGameManager.instance.LocalPlayer = this.gameObject;
            _anim.SetUpAnimator(this.gameObject.transform.Find("Cowboy").GetComponent<Animator>());
      
            _anim.SetFloat(anim_float_new1, anim_float_idle);
            _anim.SetFloat(anim_float_new2, anim_float_idle);

            playerCam.SetActive(true);
            playerCam.transform.SetParent(null, false);

            PlayerName.text = "You : " + PhotonNetwork.NickName + "|| TEAM [" + PhotonNetwork.LocalPlayer.CustomProperties["Team"] + "]";
            PlayerName.color = Color.green;
            MyName = PhotonNetwork.NickName;
        }
        else
        {
            PlayerName.text = photonview1.Owner.NickName + "|| TEAM [" + photonView.Owner.CustomProperties["Team"] + "]";
            PlayerName.color = Color.red;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && !DisableInputs)
        {
            checkInputs();
        }
    }

    void PC_Input()
    {
        var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += movement * MoveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Hoding_Shoot();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                isShoot = true;
                StartCoroutine("KeyUp_Shoot");
            }
            else
            {
                if (movement.magnitude > 0)
                {
                    PlayAnimation(ANIM_RUN);
                }
                if (movement.magnitude <= 0)
                {
                    PlayAnimation(ANIM_IDLE);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            playerCam.GetComponent<VVCameraFollow2D>().offset = new Vector3(1.3f, 1.53f, 0);
            photonview1.RPC("Flip_Right", RpcTarget.AllBuffered);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerCam.GetComponent<VVCameraFollow2D>().offset = new Vector3(-1.3f, 1.53f, 0);
            photonview1.RPC("Flip_Left", RpcTarget.AllBuffered);
        }
    }

    void MobileInput()
    {
        transform.position += Mobilemovement * MoveSpeed * Time.deltaTime;

        
        if (Mobilemovement.x != 0)
        {
            PlayAnimation(ANIM_RUN);
        }
        else
        {
            PlayAnimation(ANIM_IDLE);
        }
    }
    private void checkInputs()
    {
        if (gameObject.transform.position.y <= -3.3F)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, -2.8F, gameObject.transform.position.z);
        }

        if (isMobileInput)
        {
            MobileInput();
  
        }
        else
        {
            PC_Input();
        }
    }


    bool isShoot = false;
    float time_shoot = 0;

    void Hoding_Shoot()
    {
        Debug.Log("Hoding_Shoot()");
        time_shoot = time_shoot - Time.deltaTime;
        if (time_shoot <= 0)
        {
            time_shoot = 0.25F;
            StartCoroutine("ShootBullet");
        }
    }
  
    IEnumerator KeyUp_Shoot()
    {
        PlayAnimation(ANIM_SHOOT);
        yield return new WaitForSeconds(0.1f);
        Instantiate_Bullet();
        yield return new WaitForSeconds(0.2f);
        isShoot = false;
        PlayAnimation(ANIM_IDLE);
    }

    #region ANIMATION
  
    void PlayAnimation(string clipName)
    {
        if (clipName == ANIM_IDLE && !isShoot)
        {
            _anim.SetFloat(anim_float_new1, anim_float_idle);
            _anim.SetFloat(anim_float_new2, anim_float_idle);

        }
        else if (clipName == ANIM_RUN && !isShoot)
        {
            _anim.SetFloat(anim_float_new1, anim_float_run);
            _anim.SetFloat(anim_float_new2, anim_float_run);
        }
        else if (clipName == ANIM_JUMP && !isShoot)
        {
            _anim.SetFloat(anim_float_new1, anim_float_jump);
            _anim.SetFloat(anim_float_new2, anim_float_jump);
        }
        else if (clipName == ANIM_SHOOT)
        {
            _anim.SetFloat(anim_float_new1, anim_float_shoot);
            _anim.SetFloat(anim_float_new2, anim_float_shoot);
        }
    }

    #endregion ANIMATION

    IEnumerator ShootBullet()
    {
        PlayAnimation(ANIM_SHOOT);
        yield return new WaitForSeconds(0.1f);
        Instantiate_Bullet();
    }

    void Instantiate_Bullet()
    {
        if (sprite.flipX == false)
        {
            GameObject bullete = PhotonNetwork.Instantiate(BulletePrefab.name, new Vector2(BulleteSpawnPointRight.position.x, BulleteSpawnPointRight.position.y), Quaternion.identity, 0);
            bullete.GetComponent<VVBullet>().localPlayerObj = this.gameObject;
        }

        if (sprite.flipX == true)
        {
            GameObject bullete = PhotonNetwork.Instantiate(BulletePrefab.name, new Vector2(BulleteSpawnPointleft.position.x, BulleteSpawnPointleft.position.y), Quaternion.identity, 0);
            bullete.GetComponent<VVBullet>().localPlayerObj = this.gameObject;
            bullete.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void Flip_Right()
    {
        sprite.flipX = false;
    }

    [PunRPC]
    void Flip_Left()
    {
        sprite.flipX = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (rb == null)
        {
            return;
        }
        if (col.gameObject.tag == "Ground")
        {
            IsGrounded = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            PlayAnimation(ANIM_IDLE);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            IsGrounded = false;
        }
    }

    float delayTime = 0.1F;
    public void Jump()
    {
        if (IsGrounded)
        {
            PlayAnimation(ANIM_JUMP);
            // rb.AddForce(new Vector2(0, jumpForce * Time.deltaTime));
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }



    #region mobile input
    public void On_RightMove()
    {
        playerCam.GetComponent<VVCameraFollow2D>().offset = new Vector3(1.3f, 1.53f, 0);
        photonview1.RPC("Flip_Right", RpcTarget.AllBuffered);
        Mobilemovement = new Vector3(1, 0, transform.position.z);
    }
    public void On_LeftMove()
    {
        playerCam.GetComponent<VVCameraFollow2D>().offset = new Vector3(-1.3f, 1.53f, 0);
        photonview1.RPC("Flip_Left", RpcTarget.AllBuffered);
        Mobilemovement = new Vector3(-1, 0, transform.position.z);
    }

    public void On_Mobile_ShootUp()
    {
        isShoot = true;
         StartCoroutine("KeyUp_Shoot");
        //Debug.Log("shoot");
    }

    public void On_Mobile_ShootRelease()
    {
        isShoot = false;
        PlayAnimation(ANIM_IDLE);
    }

    public void On_Mobile_ShootHold()
    {
        Hoding_Shoot();
    }

    public void On_PointerExit()
    {
        PlayAnimation(ANIM_IDLE);
        Mobilemovement = new Vector3(0, 0, transform.position.z);
    }
    #endregion

}
