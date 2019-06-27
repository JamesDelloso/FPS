using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using TMPro;

public class Room : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject playerNamePrefab;

    [SerializeField]
    private GameObject alphaTeam;

    [SerializeField]
    private GameObject bravoTeam;

    [SerializeField]
    private GameObject gameMode;

    [SerializeField]
    private GameObject objective;

    [SerializeField]
    private GameObject maxPlayers;

    [SerializeField]
    private GameObject map;

    [SerializeField]
    private GameObject roomName;

    [SerializeField]
    private GameObject readyButton;

    [SerializeField]
    private GameObject switchingSides;

    private GameObject playerName;

    private Player[] teamAlpha;

    private Player[] teamBravo;

    private int teamChildrenNum;

    // Start is called before the first frame update
    void Start()
    {
        teamChildrenNum = alphaTeam.transform.childCount;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length-1; i++)
        {
            Debug.Log(PhotonNetwork.PlayerList[i].NickName);
            playerName = Instantiate(playerNamePrefab);
            if (PhotonNetwork.PlayerList[i].CustomProperties["Team"].ToString() == "Alpha")
            {
                playerName.transform.SetParent(alphaTeam.transform, false);
            }
            else
            {
                playerName.transform.SetParent(bravoTeam.transform, false);
            }
            if(PhotonNetwork.PlayerList[i].CustomProperties["Ready"].ToString() == "false")
            {
                playerName.transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            }
            playerName.name = PhotonNetwork.PlayerList[i].ActorNumber.ToString();
            playerName.transform.GetChild(0).GetComponent<TMP_Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }
        playerName = Instantiate(playerNamePrefab);
        playerName.name = PhotonNetwork.LocalPlayer.ActorNumber.ToString();
        playerName.GetComponent<RawImage>().color = new Color32(255, 255, 0, 128);
        playerName.transform.GetChild(0).GetComponent<TMP_Text>().text = PhotonNetwork.LocalPlayer.NickName;
        playerName.transform.GetChild(1).GetComponent<TMP_Text>().text = "";
        //playerName.GetComponent<TMP_Text>().color = Color.blue;
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        if (alphaTeam.transform.childCount <= bravoTeam.transform.childCount)
        {
            playerName.transform.SetParent(alphaTeam.transform, false);
            switchingSides.transform.GetChild(0).GetComponent<Button>().interactable = false;
            ht.Add("Team", "Alpha");
        }
        else
        {
            playerName.transform.SetParent(bravoTeam.transform, false);
            switchingSides.transform.GetChild(1).GetComponent<Button>().interactable = false;
            ht.Add("Team", "Bravo");
        }
        ht.Add("Ready", "false");
        Debug.Log(PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId));
        if (PhotonNetwork.IsMasterClient)
        {
            roomName.GetComponent<InputField>().interactable = true;
            map.GetComponent<TMP_Dropdown>().interactable = true;
            maxPlayers.GetComponent<TMP_Dropdown>().interactable = true;
            gameMode.GetComponent<TMP_Dropdown>().interactable = true;
            objective.GetComponent<TMP_Dropdown>().interactable = true;
            readyButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Start";
        }
        else
        {
            roomName.GetComponent<InputField>().interactable = false;
            map.GetComponent<TMP_Dropdown>().interactable = false;
            maxPlayers.GetComponent<TMP_Dropdown>().interactable = false;
            gameMode.GetComponent<TMP_Dropdown>().interactable = false;
            objective.GetComponent<TMP_Dropdown>().interactable = false;
        }
        updateProperties();
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void OnJoinedRoom()
    {

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(GameObject.Find(otherPlayer.ActorNumber.ToString()));
        if (PhotonNetwork.IsMasterClient)
        {
            roomName.GetComponent<InputField>().interactable = true;
            map.GetComponent<TMP_Dropdown>().interactable = true;
            maxPlayers.GetComponent<TMP_Dropdown>().interactable = true;
            gameMode.GetComponent<TMP_Dropdown>().interactable = true;
            objective.GetComponent<TMP_Dropdown>().interactable = true;
            readyButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Start";
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerName = Instantiate(playerNamePrefab);
        if (alphaTeam.transform.childCount <= bravoTeam.transform.childCount)
        {
            playerName.transform.SetParent(alphaTeam.transform, false);
        }
        else
        {
            playerName.transform.SetParent(bravoTeam.transform, false);
        }
        playerName.name = newPlayer.ActorNumber.ToString();
        playerName.transform.GetChild(0).GetComponent<TMP_Text>().text = newPlayer.NickName;
        playerName.transform.GetChild(1).GetComponent<TMP_Text>().text = "";
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        updateProperties();
    }

    public void updateProperties()
    {
        TMP_Dropdown dropdown = maxPlayers.GetComponent<TMP_Dropdown>();
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            if (int.Parse(dropdown.options[i].text.Substring(0, 1)) * 2 == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                dropdown.value = i;
                break;
            }
        }
        dropdown = map.GetComponent<TMP_Dropdown>();
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            if (dropdown.options[i].text == PhotonNetwork.CurrentRoom.CustomProperties["Map"].ToString())
            {
                map.GetComponent<TMP_Dropdown>().value = i;
                break;
            }
        }
        roomName.transform.GetChild(0).GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.CustomProperties["Name"].ToString();
    }

    public void maxPlayersChanged()
    {
        int newMax = int.Parse(maxPlayers.transform.GetChild(0).GetComponent<TMP_Text>().text.Substring(0, 1)) * 2;
        if(newMax >= PhotonNetwork.CurrentRoom.PlayerCount && alphaTeam.transform.childCount - teamChildrenNum <= newMax / 2 && bravoTeam.transform.childCount - teamChildrenNum <= newMax / 2)
        {
            PhotonNetwork.CurrentRoom.MaxPlayers = byte.Parse((newMax).ToString());
        }
        else
        {
            maxPlayers.GetComponent<TMP_Dropdown>().value = PhotonNetwork.CurrentRoom.MaxPlayers / 2 - 1;
        }
    }

    public void nameChanged()
    {
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Name", roomName.transform.GetChild(2).GetComponent<TMP_Text>().text);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    public void mapChanged()
    {
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Map", map.transform.GetChild(0).GetComponent<TMP_Text>().text);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    public void maxPlayersClicked()
    {

    }

    public void readyClicked()
    {
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        if (PhotonNetwork.IsMasterClient)
        {
            int alphaReady = 0;
            int bravoReady = 0;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if(PhotonNetwork.PlayerList[i].CustomProperties["Team"].ToString() == "Alpha" && PhotonNetwork.PlayerList[i].CustomProperties["Ready"].ToString() == "true")
                {
                    alphaReady++;
                }
                else if(PhotonNetwork.PlayerList[i].CustomProperties["Team"].ToString() == "Bravo" && PhotonNetwork.PlayerList[i].CustomProperties["Ready"].ToString() == "true")
                {
                    bravoReady++;
                }
            }
            if(playerName.transform.parent == alphaTeam.transform)
            {
                alphaReady++;
            }
            else
            {
                bravoReady++;
            }
            if((alphaReady >= 2 && bravoReady >=2) || (PhotonNetwork.CurrentRoom.MaxPlayers == 2 && alphaReady == 1 && bravoReady == 1))
            {
                PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.CustomProperties["Map"].ToString());
            }
        }
        else
        {
            if (playerName.transform.GetChild(1).GetComponent<TMP_Text>().text == "")
            {
                playerName.transform.GetChild(1).GetComponent<TMP_Text>().text = "Ready";
                ht.Add("Ready", "true");
            }
            else
            {
                playerName.transform.GetChild(1).GetComponent<TMP_Text>().text = "";
                ht.Add("Ready", "false");
            }
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    public void switchTeams()
    {
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        if (playerName.transform.parent == alphaTeam.transform && bravoTeam.transform.childCount - alphaTeam.transform.childCount < 1 && bravoTeam.transform.childCount - teamChildrenNum < PhotonNetwork.CurrentRoom.MaxPlayers / 2)
        {
            ht.Add("Team", "Bravo");
            playerName.transform.SetParent(bravoTeam.transform, false);
            switchingSides.transform.GetChild(1).GetComponent<Button>().interactable = false;
            switchingSides.transform.GetChild(0).GetComponent<Button>().interactable = true;
        }
        else if (playerName.transform.parent == bravoTeam.transform && alphaTeam.transform.childCount - bravoTeam.transform.childCount < 1 && alphaTeam.transform.childCount - teamChildrenNum < PhotonNetwork.CurrentRoom.MaxPlayers / 2)
        {
            ht.Add("Team", "Alpha");
            playerName.transform.SetParent(alphaTeam.transform, false);
            switchingSides.transform.GetChild(0).GetComponent<Button>().interactable = false;
            switchingSides.transform.GetChild(1).GetComponent<Button>().interactable = true;
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        for(int i=0;i<PhotonNetwork.PlayerList.Length;i++)
        {
            Transform tf = alphaTeam.transform.Find(PhotonNetwork.PlayerList[i].ActorNumber.ToString());
            if (tf == null)
            {
                tf = bravoTeam.transform.Find(PhotonNetwork.PlayerList[i].ActorNumber.ToString());
            }
            if (PhotonNetwork.PlayerList[i].CustomProperties["Team"].ToString() == "Alpha")
            {
                tf.SetParent(alphaTeam.transform, false);
            }
            else
            {
                tf.SetParent(bravoTeam.transform, false);
            }
            if (PhotonNetwork.PlayerList[i].CustomProperties["Ready"].ToString() == "true")
            {
                tf.transform.GetChild(1).GetComponent<TMP_Text>().text = "Ready";
            }
            else
            {
                tf.transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            }
        }
    }
}
