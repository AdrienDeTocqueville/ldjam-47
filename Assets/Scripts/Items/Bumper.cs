using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        other.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
    }
}
