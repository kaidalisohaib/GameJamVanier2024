using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class outPortal : MonoBehaviour
{
    bool end = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!end && other.gameObject.name == "MC") {
            end = true;
            SceneManager.LoadScene("Won", LoadSceneMode.Single);
        }
    }
}
