using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PhotonLobbyCustomMatch : MonoBehaviourPunCallbacks, ILobbyCallbacks {

    public static PhotonLobbyCustomMatch lobby;

    public string roomName;
    public GameObject roomList;
    public Transform roomPanel;

    private void Awake()
    {
        lobby = this;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateRoom()
    {
        Debug.Log("Try creating new room...");
        
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPLayers };
        PhotonNetwork.CreateRoom( roomName, roomOps);
    }
    
    public void OnCancelButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public void OnRoomNameChanged(string name)
    {
        roomName = name;
    }

    public void JoinRoomClick()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    void ClearRoomListing()
    {
        while (roomPanel.childCount != 0)
        {
            Destroy(roomPanel.GetChild(0).gameObject);
        }
    }

    void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempList = Instantiate(roomList, roomPanel);
            RoomButton tempbutton = tempList.GetComponent<RoomButton>();

            tempbutton.roomName = room.Name;
            tempbutton.SetRoom();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player connected to PUN master server!");

        PhotonNetwork.AutomaticallySyncScene = true; // all other players connected wil load master scene
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        ClearRoomListing();
        foreach (RoomInfo room in roomList)
        {
            ListRoom(room);
        }
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Tried create another room, but it's already exist!");
        //CreateRoom();
    }

}
