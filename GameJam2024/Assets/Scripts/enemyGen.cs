using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyGen : MonoBehaviour
{
    public GameObject[] enemies;
    bool geneting = false;
    public int number = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        int n = 0;
        foreach (Transform child in transform)
        {
            n++;
        }

        if (!geneting && n == 0) {
            geneting = true;
            StartCoroutine(newGen());
        }
    }


    IEnumerator newGen()
    {

        yield return new WaitForSeconds(3);
        int randomIndex = Random.Range(1, number+1);
        for (int i = 0; i < randomIndex; i++) {
            int monster = Random.Range(0, enemies.Length);
            Instantiate(enemies[monster], transform.position, Quaternion.identity, this.transform);
        }
        geneting = false;

    }
}
