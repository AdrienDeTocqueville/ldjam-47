using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject dust;
    public int vie = 100;
    public void TakeDamage(int damage)
    {
        vie -= damage;
        Instantiate(dust, transform.position, Quaternion.identity);
    }

    void Update(){
        if(vie <= 0){
            Destroy(gameObject);
        }

    }
}
