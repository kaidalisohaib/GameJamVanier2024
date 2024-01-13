using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class animationSpider : MonoBehaviour
{
    private Vector3 lastPosition;
    private Animator animator;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, player.position));
        Vector3 direction = transform.position - lastPosition;
        
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
        if (Vector3.Distance(transform.position, player.position) <= 2)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
        }
        if(Vector3.Distance(transform.position, player.position) >= 2)
        {
            animator.SetBool("isAttacking", false);
        }

        lastPosition = transform.position;
        
    }
}
