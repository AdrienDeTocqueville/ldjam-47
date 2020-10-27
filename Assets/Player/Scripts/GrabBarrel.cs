using UnityEngine;

public class GrabBarrel : MonoBehaviour
{
    GameObject barrel = null;
    bool grabbing = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (grabbing) // Ungrab
		        Ungrab();
            else if (barrel != null) // Grab
            {
                GameObject.FindObjectOfType<LevelLogic>().TriggerRecordMode();

                barrel.transform.SetParent(transform);
                Vector3 pos = barrel.transform.localPosition; pos.y = 0.0f;
                barrel.transform.localPosition = pos;

                barrel.GetComponent<MotionLooper>().StartRecord();
                barrel.GetComponent<Rigidbody2D>().simulated = false;
                grabbing = true;
            }
        }
    }

    void Ungrab()
    {
	    if (grabbing)
	    {
		    barrel.GetComponent<Rigidbody2D>().simulated = true;
		    barrel.transform.SetParent(null);
		    grabbing = false;
	    }
    }

    public void Loop()
    {
        Ungrab();
        barrel = null;
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
