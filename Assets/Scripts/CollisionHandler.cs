using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//'UnityEngine.SceneManagement' is required for me to use the methods of the SceneManager class.
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionStay(Collision other) 
    {
        //To decide what object has Halikal bumped into, decided by objects' tags.
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly!");
                break;
            case "Finish":
                LoadNextLevel();
                break;
            case "Soul":
                Debug.Log("You have harvested a soul!");
                break;
            default:
                StartCrashSequence();
                break;
        }    
    }

    void StartCrashSequence()
    {
        //Halikal has the Movement script attached to him. I can reach from here for that component in the Inspector
        //section in Unity and disable the script atfer the demon has crashed into an unfriendly object.
        GetComponent<Movement>().enabled = false;
        //'Invoke' is a method that allows me to invoke a method after a specified amount of time has passed.
            //I can use it to give the player a moment to realize they have collided with an unfriendly object
            //before reloading the level. I need to refer to the invoked method by its name as a string.
        Invoke("ReloadLevel", 1f);
    }

    void ReloadLevel()
    {
        //I need to specify what level/scene I want to reload. I can check the index of the
        //level in Unity -> File -> Build Settings -> Scenes In Build. To begin with, it was
        //empty and I added my sandbox (the "main" scene) there by pressing the 'Add Open Scenes'
        //button but simply dragging it from the Scenes catalog would do the trick as well.
        //I can either provide the index which would allow me to refer to different levels
        //(e.g. '0 + 1') or I can provide the name of the scene as a string.
        // SceneManager.LoadScene(0); OR SceneManager.LoadScene("Sandbox");
        //I can also trade it for "return whatever number I think the scene's index currently is".
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        //If the next scene's index is the same as the total number of the scenes that I have,
        //then I want to go back to the beginning.
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
