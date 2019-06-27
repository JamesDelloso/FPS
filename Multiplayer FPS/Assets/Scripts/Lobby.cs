using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine.EventSystems;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject roomList;

    [SerializeField]
    private GameObject roomButtonPrefab;

    [SerializeField]
    private Material[] roomColour;

    TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);

    //private readonly string[] roomNames = { "Live for something rather than die for nothing.", "Lead me, follow me, or get the hell out of my way.", "It is fatal to enter a war without the will to win it.", "If you find yourself in a fair fight, you didn't plan your mission properly.", "War is hell.", "Only the dead have seen the end of war.", "No man is a man until he has been a soldier.", "It is fatal to enter a war without the will to win it.", "If I charge, follow me. If I retreat, kill me. If I die, revenge me." };
    private readonly string[] roomNames = { "War is hell", "Whatever it takes", "Death or glory", "Stay alert, stay alive" };

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Awake()
    {

    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public void joinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void createRoom()
    {
        if(PhotonNetwork.InLobby)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2, CustomRoomPropertiesForLobby = new string[] { "Number", "Name", "Map" }, CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Number", 0 }, { "Name", "Room Name" }, { "Map", "Name of Map" } } });
        }
    }

    public override void OnCreatedRoom()
    {
        int roomNumber = 1;
        int[] currentRoomNums = new int[roomList.transform.childCount];
        for (int i = 0; i < currentRoomNums.Length; i++)
        {
            currentRoomNums[i] = int.Parse(roomList.transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().text);
        }
        for (int i = 1; i < currentRoomNums.Length + 2; i++)
        {
            if (!currentRoomNums.Contains(i))
            {
                roomNumber = i;
                break;
            }
        }
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        //ht.Add("Name", roomNames[Random.Range(0, roomNames.Length)]);
        ht.Add("Name", "Room " + roomNumber.ToString());
        ht.Add("Number", roomNumber);
        ht.Add("Map", "Map 1");
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        SceneManager.LoadScene("Room");
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomListInfo)
    {
        Debug.Log(PhotonNetwork.CountOfRooms);
        Debug.Log(roomListInfo.Count);
        foreach (RoomInfo ri in roomListInfo)
        {
            Debug.Log(roomList.transform.Find(ri.Name));
            int siblingIndex = roomList.transform.childCount;
            if (roomList.transform.Find(ri.Name) != null)
            {
                //siblingIndex = roomList.transform.Find(ri.Name).GetSiblingIndex();
                DestroyImmediate(roomList.transform.Find(ri.Name).gameObject);
            }
            if (ri.PlayerCount != 0)
            {
                for (int i = 0; i < roomList.transform.childCount; i++)
                {
                    if (int.Parse(ri.CustomProperties["Number"].ToString()) < int.Parse(roomList.transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().text))
                    {
                        siblingIndex = Mathf.Clamp(i, 0, roomList.transform.childCount);
                        break;
                    }
                }
                GameObject rb = Instantiate(roomButtonPrefab);
                rb.transform.SetParent(roomList.transform);
                rb.transform.SetSiblingIndex(siblingIndex);
                rb.transform.localPosition = Vector3.back;
                rb.transform.localScale = Vector3.one;
                rb.name = ri.Name;

                rb.GetComponent<Button>().onClick.AddListener(delegate { joinRoom(ri.Name); });

                rb.transform.GetChild(0).GetComponent<TMP_Text>().text = ri.CustomProperties["Number"].ToString();
                Debug.Log(rb.transform.GetChild(0).GetComponent<TMP_Text>().text);
                rb.transform.GetChild(1).GetComponent<TMP_Text>().text = ri.CustomProperties["Name"].ToString();
                rb.transform.GetChild(3).GetComponent<TMP_Text>().text = ri.CustomProperties["Map"].ToString();
                rb.transform.GetChild(4).GetComponent<TMP_Text>().text = ri.PlayerCount + "/" + ri.MaxPlayers;
                if(ri.PlayerCount == ri.MaxPlayers)
                {
                    rb.GetComponent<Button>().interactable = false;
                }
            }
        }
        Color32 color = new Color32(0, 0, 0, 128);
        for (int i = 0; i < roomList.transform.childCount; i++)
        {
            //Material material = roomColour[0];
            GameObject rb = roomList.transform.GetChild(i).gameObject;
            if (i % 2 == 1)
            {
                //material = roomColour[1];
                rb.GetComponent<Image>().color = color;
            }
            //roomList.transform.GetChild(i).GetComponent<Image>().material = material;
        }
    }
}
