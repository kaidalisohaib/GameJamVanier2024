using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxSize = 3.0f;  // Set the maximum size you want
    public float growthRate = 0.1f;  // Set the rate at which the object grows per frame
    public float damage = 50;

    void Start()
    {
        StartCoroutine(ScaleUp());
    }

    IEnumerator ScaleUp()
    {
        Vector3 initialScale = transform.localScale;
        float time = Time.time;
        while (transform.localScale.x < maxSize)
        {
            float deltaTime = Time.time - time;
            time = Time.time;
            // Increase the size gradually
            transform.localScale += new Vector3(growthRate, growthRate, growthRate)* deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches exactly the maxSize
        transform.localScale = new Vector3(maxSize, maxSize, maxSize);

        // Optionally, you can add additional actions or logic here after the scaling is complete
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "enemy") {
            Debug.Log(collision.gameObject.name);
            collision.gameObject.GetComponent<enemyController>().removeHealth(damage);
        }
    }
}
