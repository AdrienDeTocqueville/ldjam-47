using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class MotionLooper : MonoBehaviour
{
	public struct KeyFrame
	{
		public float timestamp;
		public Vector2 position;
		public Vector2 rotation;

		public KeyFrame Sub(KeyFrame other)
		{
			return new KeyFrame() {
				timestamp = timestamp - (other.timestamp - timestamp),
				position = position - (other.position - position),
				rotation = rotation - (other.rotation - rotation)
			};
		}
		public KeyFrame Add(KeyFrame other)
		{
			return new KeyFrame() {
				timestamp = timestamp + (timestamp - other.timestamp),
				position = position + (position - other.position),
				rotation = rotation + (rotation - other.rotation)
			};
		}
	}

	enum State { Record, Stop, Replay };

	List<KeyFrame> frames;
	State state = State.Record;
	float refTime;
	int currentFrame;

	Vector2 lastPosition;
	Vector2 lastSavedDirection;
	float lastSavedSpeed;

	public bool replayMotion = true;
	public GameObject explosion = null;

	public bool debugView = false;
	public bool catmullRomSpline = false;
	public float angleTolerance = 0.1f;

	void Awake()
	{
		lastPosition = transform.position;
		lastSavedDirection = Vector2.left;
		lastSavedSpeed = 0.0f;

		frames = new List<KeyFrame>();
		frames.Add(new KeyFrame()
		{
			timestamp = 0,
			position = lastPosition,
			rotation = transform.rotation.eulerAngles
		});
		refTime = Time.time;
	}

	Vector2 CatmullRomSpline(float alpha, KeyFrame[] p)
	{
		float tj(Vector2 a, Vector2 b) {
			var x = 1.0f;
			return Mathf.Pow( Mathf.Pow(b.x-a.x, 2) + Mathf.Pow(b.y-a.y, 2), x / 2 );
		}

		float[] t = new float[4];
		for (int i = 0; i < 3; i++)
			t[i+1] = tj(p[i].position, p[i+1].position) + t[i];

		var s = Mathf.Lerp(t[1], t[2], alpha);
		float coef1, coef2;
		{
			Vector2[] A = new Vector2[3];
			for (int i = 0; i < 3; i++)
			{
				coef1 = (t[i+1] - s) / (t[i+1] - t[i]);
				coef2 = (s - t[i]) / (t[i+1] - t[i]);
				A[i] = coef1*p[i].position + coef2*p[i+1].position;
			}

			Vector2[] B = new Vector2[2];
			for (int i = 0; i < 2; i++)
			{
				coef1 = (t[i+2] - s) / (t[i+2] - t[i]);
				coef2 = (s - t[i]) / (t[i+2] - t[i]);
				B[i] = coef1*A[i] + coef2*A[i+1];
			}

			coef1 = (t[2] - s) / (t[2] - t[1]);
			coef2 = (s - t[1]) / (t[2] - t[1]);
			Vector2 C = coef1*B[0] + coef2*B[1];

			return C;
		}
	}

	void Update()
	{
		if (!replayMotion || state == State.Stop)
			return;

		if (state == State.Record)
		{
			Vector2 position = transform.position;
			Vector3 rotation = transform.rotation.eulerAngles;
			
			Vector2 movement = position - lastPosition;
			float speed = movement.magnitude;
			if (speed < 0.05f)
				return;

			Vector2 currentDirection = movement / speed;
			float aligmment = Vector2.Dot(currentDirection, lastSavedDirection);
			float speedDiff = Mathf.Abs(speed - lastSavedSpeed);
			if (aligmment < 1.0f - angleTolerance)
			{
				AddFrame(position, rotation);

				lastSavedDirection = currentDirection;
				lastSavedSpeed = speed;
			}

			lastPosition = transform.position;
		}
		else
		{
			float lastFrame = frames.Count - (catmullRomSpline ? 1 : 0);
			if (currentFrame >= lastFrame)
				return;

			float now = Time.time - refTime;
			if (now > frames[currentFrame].timestamp)
				currentFrame++;

			if (currentFrame >= lastFrame)
			{
				// Explode
				var e = Instantiate(explosion, transform.position, transform.rotation);
				Object.Destroy(e, 1.2f);
				Disable();
				return;
			}

			KeyFrame a = frames[currentFrame - 1];
			KeyFrame b = frames[currentFrame];

			float alpha = (now - a.timestamp) / (b.timestamp - a.timestamp);

			if (catmullRomSpline)
			{
				transform.position = CatmullRomSpline(alpha, new KeyFrame[]
				{
					frames[currentFrame - 2],
					a, b,
					frames[currentFrame + 1]
				});
			}
			else
			{
				transform.position = Vector2.Lerp(a.position, b.position, float.IsNaN(alpha) ? 0.0f : alpha);
				transform.rotation = Quaternion.Lerp(Quaternion.Euler(a.rotation), Quaternion.Euler(a.rotation), alpha);
			}
		}
	}

	void AddFrame(Vector2 position, Vector3 rotation)
	{
		if (debugView)
			Debug.DrawLine(frames[frames.Count - 1].position, position, Color.blue, 1000);

		frames.Add(new KeyFrame()
		{
			timestamp = Time.time - refTime,
			position = position,
			rotation = rotation
		});
	}

    private void Disable()
    {
		Destroy(GetComponent<Rigidbody2D>());
		foreach (var collider in GetComponents<Collider2D>())
			collider.enabled = false;
		foreach (var renderer in GetComponents<SpriteRenderer>())
			renderer.enabled = false;
	
		state = State.Stop;
    }

    private void Enable()
    {
		foreach (var collider in GetComponents<Collider2D>())
			collider.enabled = true;
		foreach (var renderer in GetComponents<SpriteRenderer>())
			renderer.enabled = true;
	
		state = State.Replay;
    }


    public void StopRecord()
	{
		if (replayMotion && state == State.Record)
		{
			// Save last position
			AddFrame(transform.position, transform.rotation.eulerAngles);

			// Padd frames for catmull rom
			if (catmullRomSpline)
			{
				frames.Insert(0, frames[0].Sub(frames[1]));
				frames.Add(frames[frames.Count - 1].Add(frames[frames.Count - 2]));
			}

			// Disable movement script
			var mobAIScript = GetComponent<MobAI>();
			if (mobAIScript != null)
				mobAIScript.enabled = false;

			Disable();
		}
	}


	public void Loop()
	{
		StopRecord();
		Enable();

		// Reset
		transform.position = frames[0].position;
		transform.rotation = Quaternion.Euler(frames[0].rotation);

		refTime = Time.time;
		currentFrame = catmullRomSpline ? 2 : 1;
	}
}
