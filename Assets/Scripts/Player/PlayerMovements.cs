using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovements : MonoBehaviour
{

    CharacterController2D controller;
    Player player;
    
	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
    



    void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        controller = gameObject.GetComponent<CharacterController2D>();
    }

	
	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (player.GetButtonDown("Jump"))
		{
			jump = true;
		}
        
		/*
		if (player.GetButtonDown("Crouch"))
		{
			crouch = true;
		}
		else if (player.GetButtonUp("Crouch"))
		{
			crouch = false;
		}
		*/
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
}
