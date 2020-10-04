using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovements : MonoBehaviour
{
	public float runSpeed = 40f;

	CharacterController2D controller;
	Player player;

	float horizontalMove = 0f;
	bool jump = false;
	

	void Awake()
	{
		player = ReInput.players.GetPlayer(0);
		controller = gameObject.GetComponent<CharacterController2D>();
	}

	void Update ()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
			jump = true;
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
		jump = false;
	}
}
