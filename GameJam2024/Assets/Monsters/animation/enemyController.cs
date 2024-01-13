using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class enemyController : MonoBehaviour
{
    private Vector3 lastPosition;
    private Animator animator;
    public Transform player;
    public float attackCooldown = 0.75f;
    public float health = 100;
    public float damage = 30;

    float timer = 0;
    MainCharacter playerController;
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
        playerController = GameObject.Find("MC").GetComponent<MainCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerController != null && Time.time - timer >= attackCooldown) {
            playerController.removeHealth(damage);
            Debug.Log("DAMAGE TO MC");
            Debug.Log(playerController.health);
            timer = Time.time;
        }
        Vector3 direction = transform.position - lastPosition;
        float distance = Mathf.Infinity;
        if (playerController != null) {
            distance = Vector3.Distance(transform.position, player.position);
        }

        if (direction.magnitude > 0.01f) { 
            animator.SetBool("isRunning",true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isJumping", false);
        }
        if (direction.magnitude <= 0.01f)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
        }
        if (distance <= 2)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            if(Time.time - timer >= attackCooldown)
            {
                timer = Time.time;
            }
        }
        if(distance >= 2)
        {
            animator.SetBool("isAttacking", false);
            timer = Time.time;
        }

        lastPosition = transform.position;
        if (playerController == null)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isAttacking", false);
        }
    }

    public void removeHealth(float damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
