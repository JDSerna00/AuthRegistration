using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwap : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene(1);
    }
}
