using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks {

    //Room Info
    public static PhotonRoom room;

    public bool isGameLoaded;
    public int currentScene;

    private PhotonView phView;

    //Playre Info
    Player[] photonPlayers;
    public int PlayersInRoom;
    public int CurrentNumberInRoom;

    public int PlayerInGame;

    //Delayed Start
    private bool readyToCount;
    private bool readyToStart;
    private float lessThanMaxPlayer;
    private float atMaxPLayer;
    private float timeToStart;

    public float startingTime;

    private void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    // Use this for initialization
    void Start () {

        phView = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayer = startingTime;
        atMaxPLayer = 6;
        timeToStart = startingTime;

	}
	
	// Update is called once per frame
	void Update () {

        if (MultiplayerSettings.multiplayerSettings.delayedStart)
        {
            if (PlayersInRoom == 1)
            {
                RestartTimer();
            }
            if (isGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPLayer -= Time.deltaTime;
                    lessThanMaxPlayer = atMaxPLayer;
                    timeToStart = atMaxPLayer;
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayer -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayer;
                }
                Debug.Log("Time to start the game : " + timeToStart);

                if (timeToStart <= 0f)
                {
                    StartGame();
                }

            }
        }

	}

    void StartGame()
    {
        isGameLoaded = true;

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (MultiplayerSettings.multiplayerSettings.delayedStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);

    }

    void RestartTimer()
    {
        lessThanMaxPlayer = startingTime;
        timeToStart = startingTime;
        atMaxPLayer = 6;
        readyToCount = false;
        readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;

        if (currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene)
        {
            isGameLoaded = true;

            if (MultiplayerSettings.multiplayerSettings.delayedStart)
            {
                phView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        PlayerInGame++;
        if (PlayerInGame == PhotonNetwork.PlayerList.Length)
        {
            phView.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "PhotonNetworkPLayer"), transform.position, Quaternion.identity, 0);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Player are now in room.");

        photonPlayers = PhotonNetwork.PlayerList;
        PlayersInRoom = photonPlayers.Length;
        CurrentNumberInRoom = PlayersInRoom;
        PhotonNetwork.NickName = CurrentNumberInRoom.ToString();

        if (MultiplayerSettings.multiplayerSettings.delayedStart)
        {
            Debug.Log("Players in room out of max players possible (" + PlayersInRoom + " : " + MultiplayerSettings.multiplayerSettings.maxPLayers + ")");

            if (PlayersInRoom > 1)
            {
                readyToCount = true;
            }
            if (PlayersInRoom == MultiplayerSettings.multiplayerSettings.maxPLayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + "has disconnected.");
        PlayersInRoom--;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("New Player has joined room.");

        photonPlayers = PhotonNetwork.PlayerList;
        PlayersInRoom++;
        if (MultiplayerSettings.multiplayerSettings.delayedStart)
        {
            Debug.Log("Players in room out of max players possible (" + PlayersInRoom + " : " + MultiplayerSettings.multiplayerSettings.maxPLayers + ")");

            if (PlayersInRoom > 1)
            {
                readyToCount = true;
            }
            if (PlayersInRoom == MultiplayerSettings.multiplayerSettings.maxPLayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

    }

}
