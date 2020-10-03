using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int vie = 100;
    public void TakeDamage(int damage)
    {
        vie -= damage;
    }

    void Update(){
        if(vie <= 0){
            Destroy(gameObject);
        }

    }
}
