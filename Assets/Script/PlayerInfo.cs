using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    public static PlayerInfo pInfo;

    public int playerSelectedChar;
    public GameObject[] allChars;

    private void OnEnable()
    {
        if (PlayerInfo.pInfo == null)
        {
            PlayerInfo.pInfo = this;
        }
        else
        {
            if (PlayerInfo.pInfo != this)
            {
                Destroy(PlayerInfo.pInfo.gameObject);
                PlayerInfo.pInfo = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {

        if (PlayerPrefs.HasKey("PlayerCharacter"))
        {
            playerSelectedChar = PlayerPrefs.GetInt("PlayerCharacter");
        }
        else
        {
            playerSelectedChar = 0;
            PlayerPrefs.SetInt("PlayerCharacter", playerSelectedChar);
        }

	}
	
}
