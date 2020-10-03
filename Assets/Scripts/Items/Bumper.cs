using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{

    AreaEffector2D effector2D;
    public float forceAngle = 90.0f;
    public float magnitude = 1000.0f;

    void Start()
    {
        effector2D = gameObject.GetComponent<AreaEffector2D>();
        ChangeBoostAngle(forceAngle);
        ChangeBoostMagnitude(magnitude);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        other.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
    }

    public void ChangeBoostAngle(float angle)
    {
        effector2D.forceAngle = angle;
        Debug.Log("angle = " + angle);
    }
    public void ChangeBoostMagnitude(float power)
    {
        effector2D.forceMagnitude = power;
    }
}
