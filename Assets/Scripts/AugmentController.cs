using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AugmentController : MonoBehaviour
{
   public void AugmentOne()
    {
        SceneManager.LoadScene("Level2 aug1");
    }

    public void AugmentTwo()
    {
        SceneManager.LoadScene("Level2 aug2");
    }

    public void AugmentThree()
    {
        SceneManager.LoadScene("Level2 aug3");
    }
}
