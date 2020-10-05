using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
	public float runSpeed = 40f;

	CharacterController2D controller;

	float horizontalMove = 0f;
	bool jump = false;
	

	void Awake()
	{
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
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}
}
