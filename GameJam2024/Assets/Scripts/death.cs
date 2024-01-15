using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class death : MonoBehaviour
{
    public static int lastScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        lastScore = MainCharacter.score;

        GameObject score = GameObject.Find("Score");
        if (score != null)
        {
            score.GetComponent<TMP_Text>().text = "Score: " + lastScore;
        }
    }

    public void restart() {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        lastScore = 0;
        MainCharacter.score = 0;

    }

    public void exit()
    {
        Application.Quit();
    }
}
