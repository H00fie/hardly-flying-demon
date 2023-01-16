using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Debug.Log("You have finished the game!");
                break;
            case "Soul":
                Debug.Log("You have harvested a soul!");
                break;
            default:
                Debug.Log("You've failed!");
                break;
        }    
    }
}
