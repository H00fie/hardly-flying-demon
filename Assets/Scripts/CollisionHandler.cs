using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//'UnityEngine.SceneManagement' is required for me to use the methods of the SceneManager class.
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip cheer;
    [SerializeField] AudioClip bump;
    //Particles to be used upon completing the level successfully or bumping into an obstacle. The
    //respective audio clips have been added within Unity - both particle effects are part of Halikal's
    //prefab and they've been dragged from the game objects' window straight to the fields in the Inspector's
    //component of this script.
    [SerializeField] ParticleSystem cheerParticles;
    [SerializeField] ParticleSystem bumpParticles;

    AudioSource audioSource;

    //If Halikal crashed into an unfriendly object and then, due to momentum, reached the exit,
    //I want only the crash's sequence to be triggered, so Halikal needs to enter a "dead zone"
    //for the duration of the time between the crash and the resolving of that collision (I added
    //a short while of delay there!).
    bool isTransitioning = false;
    //To be able to turn the collision on and off for debugging purposes.
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() 
    {
        RespondToDebugKeys();    
    }

    void RespondToDebugKeys()
    {
        //.GetKeyDown() is when the button is pressed. A 'cheat' key for debugging purposes.
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        //To toggle off and on the collision for debugging purposes. I need a boolean variable for it.
        //The mechanism used below is a standard way of toggling a functionality. Essentially, pressing 'C'
        //will switch the value of the variable between true and false.
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    void OnCollisionStay(Collision other) 
    {
        //Halikal is to react with the world only if he's currently not in the process of advancing to the next level
        //or starting the current one anew. The reacting with the world should also be switched off if I toggled the
        //collision off with the 'C' key for debugging purposes.
        //I could wrap the entire 'switch' with that 'if' statement, but I can simply return from this method, if
        //Halikal is currently transitioning or 'C' is pressed instead.
        if(isTransitioning || collisionDisabled) { return; }


        //To decide what object has Halikal bumped into, decided by objects' tags.
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly!");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        //I do not need to change my state variable back to its default because whether Halikal succeeds or crashes,
        //I am reloading a level and that automatically reloads the state back to the original state of the class.
        isTransitioning = true;
        //The below stops other sounds from playing when the sequence has started. The thrusting will now longer
        //generate the sound.
        audioSource.Stop();
        audioSource.PlayOneShot(cheer);
        cheerParticles.Play();
        //If the exit is reached, stop moving, and load the next level after a second.
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", 1f);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(bump);
        bumpParticles.Play();
        //Halikal has the Movement script attached to him. I can reach from here for that component in the Inspector
        //section in Unity and disable the script atfer the demon has crashed into an unfriendly object.
        GetComponent<Movement>().enabled = false;
        //'Invoke' is a method that allows me to invoke a method after a specified amount of time has passed.
            //I can use it to give the player a moment to realize they have collided with an unfriendly object
            //before reloading the level. I need to refer to the invoked method by its name as a string.
        Invoke("ReloadLevel", levelLoadDelay);
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
