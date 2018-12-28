using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PhotonLobby : MonoBehaviourPunCallbacks {

    public static PhotonLobby lobby;

    public GameObject connectButton;
    public GameObject cancelButton;

    private void Awake()
    {
        lobby = this;
       
    }

    // Use this for initialization
    void Start () {

        PhotonNetwork.ConnectUsingSettings();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateRoom()
    {
        Debug.Log("Try creating new room...");

        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPLayers };
        PhotonNetwork.CreateRoom("Room : " + randomRoomName, roomOps);
    }

    public void OnConnectButtonClicked()
    {
        connectButton.SetActive(false);
        cancelButton.SetActive(true);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnCancelButtonClicked()
    {
        connectButton.SetActive(true);
        cancelButton.SetActive(false);

        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player connected to PUN master server!");

        PhotonNetwork.AutomaticallySyncScene = true; // all other players connected wil load master scene
        connectButton.SetActive(true);
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("Tries to connect to room, but failed.");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Tried create another room, but it's already exist!");
        CreateRoom();
    }

}
