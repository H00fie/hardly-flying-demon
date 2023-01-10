using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//'Monobehaviour' is inherited by 'Movement'.
public class Movement : MonoBehaviour
{
    //To store the rigidbody of my demon.
    Rigidbody rb;
    //To cache by how much I want to change how much force is being applied every frame to the upward movement.
    [SerializeField] float mainThrust = 100f;
    //And for the rotation movement.
    [SerializeField] float rotationThrust = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //To cache the rigidbody of the demon within my variable.
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust(){
        //If I push the spacebar...
        if(Input.GetKey(KeyCode.Space)){
            //...add the force to the rigidbody relative to its current coordinates. 'Vector3' is three values (x, y and z).
            //In a 2D game it would be 'Vector3'. A vector is both direction and magnitude. Ssays in what direction and at what
            //speed a thing is going.
            //'Vector3' is the same as saying '1, 1, 1', e.g. 'Vector3.up' means '0, 1, 0'. 'mainThrust' is there so I can change
            //the dafult value of force applied. 'Time.deltaTime' is to make it framerate independent.
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime * 10);
            //The value is additionally multiplied because there was no movement without it.
        }
    }

    void ProcessRotation(){
        if(Input.GetKey(KeyCode.A)){
            ApplyRotation(rotationThrust);
        } else if(Input.GetKey(KeyCode.D)){
            //The other direction is basically the same, but negative.
            ApplyRotation(-rotationThrust);
        }
    }

    //In C# all methods are private by default.
    private void ApplyRotation(float rotationThisFrame){
        //The physics system in Unity reacts with objects on impact which can cause conflicts with player controlled movement. In
        //practice it can make my demon fail to react to player's commands when bumping into other game objects. In order to
        //prevent it, I need to freeze the physics system rotation. The mechanism below will prevent the physics system from
        //having on impact on my demon if it's being issued a command to rotate.
        rb.freezeRotation = true;
        //I am reaching for the object's 'transform' property within Unity. The object is whatever the script is assigned to.
        //'Vector3.forward' means '0, 0, 1'.
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime * 100);
        //The value is additionally multiplied because there was no movement without it.
        rb.freezeRotation = false;
    }
}
