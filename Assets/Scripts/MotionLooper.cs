using System.Collections;
using System.Collections.Generic;
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

        public bool Diff(Vector2 position, Vector2 rotation, float epsilon)
        {
            return (Vector2.Dot(position, this.position) > Mathf.Epsilon * Mathf.Epsilon * epsilon) &&
                (Vector2.Dot(rotation, this.rotation) > Mathf.Epsilon * Mathf.Epsilon * epsilon);
        }
    }
    List<KeyFrame> frames;
    bool replaying = false;
    float refTime;
    int currentFrame;

    public readonly bool replayMotion = true;
    public float epsilon = 10.0f;
    public GameObject explosion = null;

    void Awake()
    {
        frames = new List<KeyFrame>();
        frames.Add(new KeyFrame()
        {
           timestamp = 0,
           position = transform.position,
           rotation = transform.rotation.eulerAngles
        });
        refTime = Time.time;
    }

    void Update()
    {
        if (!replayMotion)
            return;

        if (!replaying)
        {
            Vector2 position = transform.position;
            Vector3 rotation = transform.rotation.eulerAngles;

            if (frames[frames.Count - 1].Diff(position, rotation, epsilon))
                frames.Add(new KeyFrame()
                {
                   timestamp = Time.time - refTime,
                   position = position,
                   rotation = rotation
                });
        }
        else
        {
            float now = Time.time - refTime;
            if (now > frames[currentFrame].timestamp)
                currentFrame++;

            if (currentFrame >= frames.Count)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                GameObject.Destroy(gameObject);
                return;
            }

            KeyFrame a = frames[currentFrame - 1];
            KeyFrame b = frames[currentFrame];

            float alpha = (now - a.timestamp) / (b.timestamp - a.timestamp);
            transform.position = Vector2.Lerp(a.position, b.position, float.IsNaN(alpha) ? 0.0f : alpha);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(a.rotation), Quaternion.Euler(a.rotation), alpha);
        }
    }


    public void Loop()
    {
        //Debug.Log(frames.Count);

        refTime = Time.time;
        replaying = true;
        currentFrame = 1;

        Destroy(GetComponent<Rigidbody>());

        transform.position = frames[0].position;
        transform.rotation = Quaternion.Euler(frames[0].rotation);

        var mobAIScript = GetComponent<MobAI>();
        if (mobAIScript != null)
            mobAIScript.enabled = false;
    }
}
