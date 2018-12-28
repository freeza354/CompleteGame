using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour {

    public GameObject playerAvatar;
    public PhotonView phView;
    public int playerTeam;

	// Use this for initialization
	void Start () {

        phView = GetComponent<PhotonView>();
        
        if (phView.IsMine)
        {
            phView.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }

	}

    // Update is called once per frame
    void Update()
    {
        if (playerAvatar == null && playerTeam != 0)
        {
            if (playerTeam == 1)
            {
                Debug.Log("Creating player for team 1");

                int spawnPicker = Random.Range(0, GameSetup._gs.spawnPointsTeam1.Length);
                if (phView.IsMine)
                {
                    playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                        GameSetup._gs.spawnPointsTeam1[spawnPicker].position, GameSetup._gs.spawnPointsTeam1[spawnPicker].rotation, 0);

                }

            }
            else
            {
                Debug.Log("Creating player for team 2");

                int spawnPicker = Random.Range(0, GameSetup._gs.spawnPointsTeam2.Length);
                if (phView.IsMine)
                {
                    playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                        GameSetup._gs.spawnPointsTeam2[spawnPicker].position, GameSetup._gs.spawnPointsTeam2[spawnPicker].rotation, 0);

                }
            }

        }

    }

    //Only sent to master client
    [PunRPC]
    void RPC_GetTeam()
    {
        playerTeam = GameSetup._gs.nextPlayerteams;
        GameSetup._gs.UpdateTeam();
        phView.RPC("RPC_SendTeam", RpcTarget.OthersBuffered, playerTeam);
    }

    [PunRPC]
    void RPC_SendTeam(int team)
    {
        playerTeam = team;
    }

}
