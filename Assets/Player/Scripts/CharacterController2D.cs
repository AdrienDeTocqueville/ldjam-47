using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround = 0;						// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck = null;					// A position marking where to check if the player is grounded.

	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

	SpriteRenderer spriteRenderer = null;
	int isAgainstWall = 0;

	Animator animator;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}


	private void FixedUpdate()
	{
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapBoxAll(m_GroundCheck.position, new Vector2(0.35f, 0.1f), m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
		}

#if UNITY_EDITOR
		spriteRenderer.color = new Color(m_Grounded ? 1: 0, isAgainstWall == 0 ? 1: 0, 1);
#endif
	}


	public void Move(float move, bool jump)
	{
		animator.SetBool("grounded", m_Grounded);

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
				Flip();
			else if (m_Grounded || isAgainstWall == 0)
			{
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
				// Animation transition
				animator.SetBool("moving", move != 0.0f);
			}
		}

		// If the player should jump...
		if (m_Grounded && jump)
		{
			Vector2 v = m_Rigidbody2D.velocity;
			v.y = 0.0f;

			m_Rigidbody2D.velocity = v;
			m_Rigidbody2D.angularVelocity = 0.0f;
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (((1<<collision.gameObject.layer) & m_WhatIsGround) != 0)
			isAgainstWall++;
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (((1<<collision.gameObject.layer) & m_WhatIsGround) != 0)
			isAgainstWall--;
	}
}
