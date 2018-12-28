using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour {

    public static GameSetup _gs;

    public int nextPlayerteams;
    public Transform[] spawnPointsTeam1;
    public Transform[] spawnPointsTeam2;
    public Text healthDisplay;

    private void OnEnable()
    {
        if (GameSetup._gs == null)
        {
            GameSetup._gs = this;
        }
    }

    public void UpdateTeam()
    {
        if (nextPlayerteams == 1)
        {
            nextPlayerteams = 2;
        }
        else
        {
            nextPlayerteams = 1;
        }
    }

    public void DisconnecPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }

}
