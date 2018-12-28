using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour {

    public GameObject playerCharacter;
    public int charValue;

    public int playerHealth;
    public int playerDamage;

    public Camera playerCamera;
    public AudioListener playerAudioListener;

    private PhotonView phView;

	// Use this for initialization
	void Start () {
        phView = GetComponent<PhotonView>();
        if (phView.IsMine)
        {
            phView.RPC("RPC_AddChar", RpcTarget.AllBuffered, PlayerInfo.pInfo.playerSelectedChar);
        }
        else
        {
            Destroy(playerCamera);
            Destroy(playerAudioListener);
        }
	}
	
    [PunRPC]
    void RPC_AddChar(int selectedChar)
    {
        charValue = selectedChar;
        playerCharacter = Instantiate(PlayerInfo.pInfo.allChars[selectedChar], transform.position, transform.rotation, transform);
    }

}
