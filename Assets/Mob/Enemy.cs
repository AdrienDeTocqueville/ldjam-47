using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject dust;
    public int vie = 100;
    public float freezeTime = 0.3f;

    public void TakeDamage(int damage)
    {
        vie -= damage;

        var d = Instantiate(dust, transform.position, Quaternion.identity);
		Object.Destroy(d, 2f);

        if (vie <= 0)
        {
            var script = GetComponent<MotionLooper>();
            if (script)
                script.StopRecord();
            else
                Destroy(gameObject);
        }
        else
        {
            var script = GetComponent<MobAI>();
            if (script)
                script.Freeze(freezeTime);
        }
    }
}
