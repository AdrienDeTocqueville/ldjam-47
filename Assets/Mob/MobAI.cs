using UnityEngine;

public class MobAI : MonoBehaviour
{
    public enum Direction { Left, Right };

    public Direction direction = Direction.Left;
    public float speed = 2.0f;

    public LayerMask collideLayers;
    public LayerMask groundLayers;
    Vector2 extents;
    Rigidbody2D rb;
    bool wasOnGround;

    float frozen = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        foreach (var collider in GetComponents<BoxCollider2D>())
        {
            if (!collider.isTrigger)
                extents = collider.size * 0.5f * transform.localScale;
        }

        wasOnGround = false;

        Switcheroo(direction);
        GetComponent<MotionLooper>().StartRecord();
    }

    bool IsOnGround()
    {
        Vector2 offset = (direction == Direction.Left) ? Vector2.left : Vector2.right;

        Vector2 raycast = (Vector2)transform.position - extents.x * offset;
        Debug.DrawRay(raycast, Vector2.down);

        var result = Physics2D.Raycast(raycast, Vector2.down, Mathf.Infinity, groundLayers);
        if (result.collider != null && result.distance <= extents.y + 0.05f)
            return true;
        
        raycast = (Vector2)transform.position + extents.x * offset;
        Debug.DrawRay(raycast, Vector2.down);

        result = Physics2D.Raycast(raycast, Vector2.down, Mathf.Infinity, groundLayers);
        return (result.collider != null && result.distance <= extents.y + 0.05f);
    }

    void Update()
    {
        if (frozen >= 0.0f)
            frozen -= Time.deltaTime;

        // Only move if touching the ground
        if (IsOnGround())
        {
            if (!wasOnGround)
            {
                wasOnGround = true;
                rb.velocity = Vector2.zero;
            }
            
            if (frozen <= 0.0f)
                transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (wasOnGround)
        {
            wasOnGround = false;
            Vector2 v = rb.velocity;
            v.x = speed * (direction == Direction.Left ? -1 : 1);
            rb.velocity = v;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1<<collision.gameObject.layer) & collideLayers) != 0)
            Switcheroo(1 - direction);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerAttack>().Hit();
    }


    public void Switcheroo(Direction dir)
    {
        direction = dir;

        var angles = transform.rotation.eulerAngles;
        angles.y = (direction == Direction.Left) ? 180.0f : 0.0f;
        transform.rotation = Quaternion.Euler(angles);
    }

    public void Freeze(float freezeTime)
    {
        frozen = freezeTime;
    }
}
