using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class MobAI : MonoBehaviour
{
    public enum Direction { Left, Right };

    public Direction direction = Direction.Left;
    public float speed = 2.0f;

    int collideLayers;
    int groundLayers;
    Vector2 extents;
    Rigidbody2D rb;
    bool wasOnGround;

    void Start()
    {
        collideLayers = LayerMask.NameToLayer("Ground") | LayerMask.NameToLayer("Mob");
        groundLayers = LayerMask.NameToLayer("Ground");

        rb = GetComponent<Rigidbody2D>();
        foreach (var collider in GetComponents<BoxCollider2D>())
        {
            if (!collider.isTrigger)
                extents = collider.size * 0.5f * transform.localScale;
        }

        wasOnGround = false;

        Switcheroo(direction);
    }

    bool IsOnGround()
    {
        Vector2 offset = (direction == Direction.Left) ? Vector2.left : Vector2.right;

        Vector2 raycast = (Vector2)transform.position - extents.x * offset;
        Debug.DrawRay(raycast, Vector2.down);

        var result = Physics2D.Raycast(raycast, Vector2.down, Mathf.Infinity, 1 << groundLayers);
        if (result.collider != null && result.distance <= extents.y + 0.05f)
            return true;
        
        raycast = (Vector2)transform.position + extents.x * offset;
        Debug.DrawRay(raycast, Vector2.down);

        result = Physics2D.Raycast(raycast, Vector2.down, Mathf.Infinity, 1 << groundLayers);
        return (result.collider != null && result.distance <= extents.y + 0.05f);
    }

    void Update()
    {
        // Only move if touching the ground
        if (IsOnGround())
        {
            if (!wasOnGround)
            {
                wasOnGround = true;
                rb.velocity = Vector2.zero;
            }
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (wasOnGround)
        {
            wasOnGround = false;
            Vector2 v = rb.velocity;
            v.x = speed * (direction == Direction.Left ? -1 : 1);
            rb.velocity = v;
        }

        if (Input.GetKeyDown("e"))
        {
            var loopers = GameObject.FindObjectsOfType<MotionLooper>();
            foreach (var looper in loopers)
            {
                looper.Loop();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //if ((collision.gameObject.layer & collideLayers) != 0)
            Switcheroo(1 - direction);
    }


    public void Switcheroo(Direction dir)
    {
        direction = dir;

        var angles = transform.rotation.eulerAngles;
        angles.y = (direction == Direction.Left) ? 180.0f : 0.0f;
        transform.rotation = Quaternion.Euler(angles);
    }
}
