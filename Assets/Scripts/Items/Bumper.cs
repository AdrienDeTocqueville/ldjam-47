using UnityEngine;

public class Bumper : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        Vector2 v = rb.velocity;

        v.y = 0.0f;
        rb.velocity = v;
        rb.angularVelocity = 0.0f;
    }
}
