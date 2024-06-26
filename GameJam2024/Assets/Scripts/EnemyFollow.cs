using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    NavMeshAgent enemy;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("MC").transform;
        enemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || MainCharacter.dead) return;
        
        if (Vector3.Distance(enemy.nextPosition,player.position) <=20)
            enemy.SetDestination(player.position);
        else if (enemy.nextPosition != null)
            enemy.SetDestination(enemy.nextPosition);
    }
}
