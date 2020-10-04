using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovements : MonoBehaviour
{

    CharacterController2D controller;
    Player player;
    private Animator animator;
    public Vector2 movement;


    
	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
    

    void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        controller = gameObject.GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
    }

	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}

		Move();
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}

	void Move()
	{
		movement.x = player.GetAxis("Horizontal");

		animator.SetFloat("moveX", movement.x);
		animator.SetFloat("Speed", movement.sqrMagnitude);

		if(movement.x >= 0.9 || movement.x <= -0.9)
		{
			animator.SetFloat("lastMoveX", movement.x);
		}
	}
}
