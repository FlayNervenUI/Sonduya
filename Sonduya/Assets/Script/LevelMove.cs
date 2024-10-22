using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    public int sceneBuildIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Trigger entered"); // Для консоли
        // Нужно создать тэг "Player" и назначить его игроку
        if(other.tag == "Player")
        {
            print("Switching scene to " + sceneBuildIndex); // для консоли
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
