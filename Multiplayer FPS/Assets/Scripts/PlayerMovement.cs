using UnityEngine;
using System.Collections;
using Photon.Pun;
using System;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private GameObject spine;
    //[SerializeField]
    //private GameObject leftHandGrip;
    //[SerializeField]
    //private GameObject rightHandGrip;
    //[SerializeField]
    //private GameObject leftElbow;
    //[SerializeField]
    //private GameObject rightElbow;
    //[SerializeField]
    //private GameObject leftHand;
    [SerializeField]
    private GameObject head;
    [SerializeField]
    private GameObject eyeFocus;

    private CharacterController characterController;
    private Animator animator;

    private Vector3 moveDirection = Vector3.zero;
    private float jumpPosY;

    private Vector3 spineAngle = Vector3.zero;
    private float lag = 1;
    public Vector3 offset;
    public Vector3 offsetHead;

    // Use this for initialization
    void Start()
    {
        if(PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.OfflineMode = true;
        }

        if (photonView.IsMine)
        {
            Camera.main.transform.SetParent(head.transform);
            Camera.main.transform.localPosition = new Vector3(0, 0.2f, 0.1f);
            Camera.main.transform.localEulerAngles = Vector3.zero;
        }
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (characterController.isGrounded && Input.GetButtonDown("Jump"))
            {
                jumpPosY = transform.position.y + jumpHeight;
                moveDirection.y = speed / 2;
                moveDirection.y *= jumpPosY - transform.position.y;
                Debug.Log(jumpPosY - transform.position.y);
            }
            else if (transform.position.y < jumpPosY && characterController.velocity.y > 0.01f)
            {
                Debug.Log(jumpPosY - transform.position.y);
                moveDirection.y = speed / 2;
                moveDirection.y *= jumpPosY - transform.position.y;
            }
            moveDirection *= speed;

            moveDirection = head.transform.TransformDirection(moveDirection);
            moveDirection += Physics.gravity;
            characterController.Move(moveDirection * Time.deltaTime);

            transform.eulerAngles = transform.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X"));
            spineAngle = spine.transform.eulerAngles;

            animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        }
    }

    void LateUpdate()
    {
        if (photonView.IsMine)
        {
            spine.transform.eulerAngles = weapon.transform.eulerAngles;
            GameObject.Find("swat:Spine1").transform.eulerAngles += offset;
            head.transform.eulerAngles = spine.transform.eulerAngles + offsetHead;
            spine.transform.eulerAngles = spineAngle + new Vector3(-Input.GetAxis("Mouse Y"), 0, 0);
        }
        //if (photonView.IsMine)
        //{
        //    spine.transform.eulerAngles = spineAngle + new Vector3(-Input.GetAxis("Mouse Y"), 0, 0);
        //    head.transform.eulerAngles = spine.transform.eulerAngles + offsetHead;
        //}
        else
        {
            spine.transform.eulerAngles = spineAngle;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(spineAngle);
        }
        else
        {
            spineAngle = (Vector3)stream.ReceiveNext();
            lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
        }
    }
}
