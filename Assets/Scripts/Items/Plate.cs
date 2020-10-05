using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
	public List<Activable> activations = new List<Activable>();
	bool isPressed = false;

	Vector2 rect;

	private void Awake()
	{
		rect = GetComponent<BoxCollider2D>().size * transform.localScale;
		if (activations.Count == 0)
			Debug.LogError("This pressure plate has nothing to activate !");
	}

	public void Update()
	{
		var hits = Physics2D.BoxCastAll(transform.position, rect, 0, Vector2.up, 0.05f);
		bool pressed = false;
		foreach (var hit in hits)
		{
			if (hit.transform.gameObject.CompareTag("Player") ||
			    hit.transform.gameObject.CompareTag("Barrel") ||
			    hit.transform.gameObject.CompareTag("Mob"))
			{
				pressed = true;
				break;
			}
		}

		if (pressed && !isPressed)
		{
			isPressed = true;
			foreach (var a in activations)
				a.Activate();
		}
		else if (!pressed && isPressed)
		{
			isPressed = false;
			foreach (var a in activations)
				a.Deactivate();
		}
	}
}
