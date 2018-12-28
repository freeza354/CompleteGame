using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed;
    public float rotationSpeed;

    private PhotonView phView;
    private CharacterController playerCC;

	// Use this for initialization
	void Start () {
        phView = GetComponent<PhotonView>();
        playerCC = GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {

        if (phView.IsMine)
        {
            basicMove();
            basicRotation();
        }

	}

    void basicMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerCC.Move(transform.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerCC.Move(-transform.right * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerCC.Move(transform.right * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerCC.Move(-transform.forward * Time.deltaTime * moveSpeed);
        }
    }

    void basicRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;

        transform.Rotate(new Vector3(0, mouseX, 0));
    }

}
