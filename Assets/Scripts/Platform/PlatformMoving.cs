using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoving : MonoBehaviour
{


    private Vector3 startPos;
    public GameObject targetObject;

    public float speed = 1.0f;

    bool movingToTarget;
    private GameObject target=null;
    public bool isActivated = true;


    void Start()
    {
        startPos = transform.position;
        movingToTarget = true;
        target = null;

    }
    void FixedUpdate()
    {
        if(isActivated)
        {
            float step = speed * Time.deltaTime;

            if (transform.position == targetObject.transform.position)
            {
                movingToTarget = false;
            }
            else if (transform.position == startPos)
            {
                movingToTarget = true;
            }
            if(movingToTarget == false)
            {
                transform.position = Vector3.MoveTowards (transform.position, startPos, step);
            }
            else if (movingToTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, step);
            }
        }

    }
     void OnCollisionEnter2D(Collision2D other){
     if (other.gameObject.CompareTag("Player")) {
         other.transform.parent = transform;
     }
    }
    
    
    void OnCollisionExit2D(Collision2D other){
        if (other.gameObject.CompareTag("Player")) {
            other.transform.parent = null;
        }
    }

    
}
