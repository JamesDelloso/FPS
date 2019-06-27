using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.OfflineMode = true;
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.up, Quaternion.identity);
        //player.name = PhotonNetwork.LocalPlayer.NickName + " - " + PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
