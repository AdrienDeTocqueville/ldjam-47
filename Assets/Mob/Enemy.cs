using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject dust;
    public int vie = 100;
    public float freezeTime = 0.3f;

    public void TakeDamage(int damage)
    {
        vie -= damage;
        Instantiate(dust, transform.position, Quaternion.identity);

        if (vie <= 0)
            Destroy(gameObject);
        else
        {
            var script = GetComponent<MobAI>();
            if (script)
                script.Freeze(freezeTime);
        }
    }
}
