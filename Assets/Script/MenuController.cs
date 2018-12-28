using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public void OnClickedCharacterPicks(int selectedChar)
    {
        if (PlayerInfo.pInfo != null)
        {
            PlayerInfo.pInfo.playerSelectedChar = selectedChar;
            PlayerPrefs.SetInt("PlayerCharacter", selectedChar);
        }
    }

}
