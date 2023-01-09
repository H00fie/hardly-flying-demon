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
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        }
    }

    void ProcessRotation(){
        if(Input.GetKey(KeyCode.A)){

        } else if(Input.GetKey(KeyCode.D)){

        }
    }
}
