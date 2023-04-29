using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AugmentController : MonoBehaviour
{
   public void HomeScreen()
    {
        SceneManager.LoadScene("Title");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Remake Level 2");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Remake Level 3");
    }
}
