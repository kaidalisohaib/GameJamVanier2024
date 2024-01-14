using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class death : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(MainCharacter.score);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restart() {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void exit()
    {
        Application.Quit();
    }
}
