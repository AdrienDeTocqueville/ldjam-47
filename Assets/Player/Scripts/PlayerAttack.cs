using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    public int hp = 3;
    public GameObject heart;
    public float attackCooldown;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;

    private float timeSinceAttack = 0.0f;

    Animator animator;

    GameObject[] hearts;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hearts = new GameObject[hp];
        for (int i = 0; i < hp; i++)
        {
            var h = Instantiate(heart);
            h.transform.parent = Camera.main.transform;

            var pos = h.transform.localPosition;
            pos.x += i * 1.5f;
            h.transform.localPosition = pos;

            hearts[i] = h;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) && timeSinceAttack <= 0)
        {
            timeSinceAttack = attackCooldown;
            animator.SetTrigger("attack");

            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            if (enemiesToDamage.Length == 0)
                return;

            // Damage enemies
            bool hasHit = false;
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                if (enemiesToDamage[i].isTrigger)
                    continue;

                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                hasHit = true;
            }

            // Shake camera only when hit
            if (hasHit) Camera.main.GetComponent<CameraShaker>().Shake();
        }
        else
        {
            timeSinceAttack -= Time.deltaTime;
        }
    }
    
    public void Hit()
    {
        hp--;
        Destroy(hearts[hp]);
        if (hp == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void Loop()
    {
        timeSinceAttack = 0.0f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
