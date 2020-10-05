using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Rewired;

public class PlayerAttack : MonoBehaviour
{
    public float attackCooldown;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;

    private float timeSinceAttack = 0.0f;

    Vector2 intialPosition;
    Quaternion intialRotation;
    Animator animator;
    Player reinput;

    

    private void Awake()
    {
        intialPosition = transform.position;
        intialRotation = transform.rotation;
        reinput = ReInput.players.GetPlayer(0);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ResetScene();

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    
    void ResetScene()
    {
        // Reset Player state to initial
        transform.position = intialPosition;
        transform.rotation = intialRotation;

        timeSinceAttack = 0.0f;
        
        // Reset activable platforms
        var activables = GameObject.FindObjectsOfType<Activable>();
        foreach (var activable in loopers)
            activable.Loop();

        // Loop mob movement
        var loopers = GameObject.FindObjectsOfType<MotionLooper>();
        foreach (var looper in loopers)
            looper.Loop();
    }
}
