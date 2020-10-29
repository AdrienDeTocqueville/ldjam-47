using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float runSpeed = 40f;

	CharacterController2D controller;

	float horizontalMove = 0f;
	bool jump = false;

	Vector2 intialPosition;
	Quaternion intialRotation;


	void Awake()
	{
		controller = gameObject.GetComponent<CharacterController2D>();

		intialPosition = transform.position;
		intialRotation = transform.rotation;
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

	public void Loop()
	{
		GetComponent<GrabBarrel>().Loop();

		transform.position = intialPosition;
		transform.rotation = intialRotation;
	}
}
