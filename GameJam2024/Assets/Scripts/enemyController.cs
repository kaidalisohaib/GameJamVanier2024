using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class enemyController : MonoBehaviour
{
    private Vector3 lastPosition;
    private Animator animator;
    Transform player;
    public float attackCooldown = 0.75f;
    public float health = 100;
    public float damage = 30;
    public float damageDistance = 2;
    public float growthRate = 7.0f;
    public AudioSource audioSource;
    public AudioClip[] audioClips = new AudioClip[2];

    float timer;
    MainCharacter playerController;
    public int monsterScore;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.loop = true;
        timer = attackCooldown;
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
        playerController = GameObject.Find("MC").GetComponent<MainCharacter>();
        player = GameObject.Find("MC").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || MainCharacter.dead || health==0)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
            return;
        }
        if (timer <= 0) {
            audioSource.clip = audioClips[1];
            audioSource.Play();
            playerController.removeHealth(damage);
            timer = attackCooldown;
        }
        Vector3 direction = transform.position - lastPosition;
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (direction.magnitude > 0.01f) { 
            animator.SetBool("isRunning",true);
            animator.SetBool("isIdle", false);
        }
        if (direction.magnitude <= 0.01f)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRunning", false);
        }
        if (distance <= damageDistance)
        {
           
            
            animator.SetBool("isAttacking", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunning", false);
            if(timer <= attackCooldown)
            {
                timer -= Time.deltaTime;
            }
        } else if(distance > damageDistance)
        {
            audioSource.clip = audioClips[0];
            animator.SetBool("isAttacking", false);
            timer = attackCooldown;
        }

        lastPosition = transform.position;
    }

    public void removeHealth(float damage) {
        health -= damage;
        if (health <= 0) {
            MainCharacter.score += monsterScore;
            health = 0;
            StartCoroutine(ScaleDownAndDie());
        }
    }

    IEnumerator ScaleDownAndDie()
    {
        Vector3 initialScale = transform.localScale;
        float time = Time.time;
        while (transform.localScale.x > 0)
        {
            float deltaTime = Time.time - time;
            time = Time.time;
            // Increase the size gradually
            transform.localScale -= new Vector3(growthRate, growthRate, growthRate) * deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches exactly the maxSize
        transform.localScale = new Vector3(0, 0, 0);

        // Optionally, you can add additional actions or logic here after the scaling is complete
        Destroy(gameObject);
    }
}
