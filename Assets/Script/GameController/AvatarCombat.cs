using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCombat : MonoBehaviour {

    private PhotonView phView;
    private AvatarSetup playerAvatarSetup;
    private Rigidbody rb;

    public Transform rayOrigin;
    public Text displayHealth;

	// Use this for initialization
	void Start () {

        phView = GetComponent<PhotonView>();
        playerAvatarSetup = GetComponent<AvatarSetup>();
        rb = GetComponent<Rigidbody>();
        displayHealth = GameSetup._gs.healthDisplay;

	}
	
	// Update is called once per frame
	void Update () {

        displayHealth.text = playerAvatarSetup.playerHealth.ToString();

        if (!phView.IsMine)
        {
            return;
        }

        //if (Input.GetMouseButton(0))
        //{
        //    phView.RPC("RPC_Shooting", RpcTarget.All);
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.forward * 5);
        }

	}

    [PunRPC]
    void RPC_Boost()
    {

    }

    [PunRPC]
    void RPC_Shooting()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000))
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log("Ray hitting something.");

            if (hit.transform.tag == "Player")
            {
                hit.transform.gameObject.GetComponent<AvatarSetup>().playerHealth -= playerAvatarSetup.playerDamage;
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.white);
            Debug.Log("Miss");
        }
    }

}
