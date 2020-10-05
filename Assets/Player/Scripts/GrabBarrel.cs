using UnityEngine;

public class GrabBarrel : MonoBehaviour
{
    Collider2D contact;
    GameObject barrel = null;
    bool grabbing = false;

    private void Awake()
    {
        foreach (var collider in GetComponents<BoxCollider2D>())
        {
            if (collider.isTrigger)
            {
                contact = collider;
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (grabbing) // Ungrab
		    Ungrab();
            else if (barrel != null) // Grab
            {
                barrel.transform.SetParent(transform);
                Vector3 pos = barrel.transform.localPosition; pos.y = 0.0f;
                barrel.transform.localPosition = pos;

                barrel.GetComponent<Rigidbody2D>().simulated = false;
                grabbing = true;
            }
        }
    }

    public void Ungrab()
    {
	    if (grabbing)
	    {
		    barrel.GetComponent<Rigidbody2D>().simulated = true;
		    barrel.transform.SetParent(null);
		    grabbing = false;
	    }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!grabbing && collision.gameObject.CompareTag("Barrel"))
            barrel = collision.gameObject;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!grabbing && collision.gameObject == barrel)
            barrel = null;
    }
}
