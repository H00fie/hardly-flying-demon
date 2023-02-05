using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    //The script is added to Halikal himself as he is present in every scene and thus the functionality of quitting will
    //follow him throughout the entire game.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pushed escape.");
            Application.Quit();  
        }
    }
}
