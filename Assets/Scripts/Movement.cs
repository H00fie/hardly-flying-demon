using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//'Monobehaviour' is inherited by 'Movement'.
public class Movement : MonoBehaviour
{
    //Below are PARAMETERS - they are for tuning and are typically set in the editor (things with the [SerializeField]).
    //How much I want to change how much force is being applied every frame to the upward movement.
    [SerializeField] float mainThrust = 100f;
    //And for the rotation movement.
    [SerializeField] float rotationThrust = 1f;
    //Different audio clips for various events.
    [SerializeField] AudioClip thrusting;
    //I will need to drag and drop my chosen particle system into this variable's box in Unity!
    [SerializeField] ParticleSystem darkMagicParticles;
    [SerializeField] ParticleSystem leftHandParticles;
    [SerializeField] ParticleSystem rightHandParticles;

    //Below are things I want to CACHE - references for readability or speed. I am actually getting the compontent in
    //the .Start().
    //To store the rigidbody of my demon.
    Rigidbody rb;
    //To store the audio source for my demon.
    AudioSource audioSource;
    
    //Below would come any variables that have something to do with STATE. If I had any.
    //E.g. 'bool isAlive;' <--- about Halikal.

    // Start is called before the first frame update
    void Start()
    {
        //To cache the rigidbody of the demon within my variable.
        rb = GetComponent<Rigidbody>();
        //To cache a reference to my audio source.
        audioSource = GetComponent<AudioSource>();
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
            //If there was only one audio source on my component and there was only one clip on that audio source, I wouldn't need
            //to specify what the clip is. The 'if' ensures that the clip be only played once at a time, without it, it would be
            //triggered multiple times and they would overlap.
            if(!audioSource.isPlaying){
                //The .PlayOneShot() is a different method than .Play() because it allows for a parameter to be passed in. Due to
                //that, I can specify what clip exactly I want played.
                audioSource.PlayOneShot(thrusting);
                //Below is the sole line of this 'if' statement when there was only one audio clip available. The .Play() didn't
                //allow for any parameters to be passed in so it was good when there was a single clip available. It was simply
                //inferred what's it supposed to play since there was only one option.
                // audioSource.Play();
            }
            if(!darkMagicParticles.isPlaying){
                //When the space is pressed, emit the particles.
                darkMagicParticles.Play();
            }
        } else {
            //If the 'space' is not pressed, don't play the audio.
            audioSource.Stop();
            //... and stop emitting the particles!
            darkMagicParticles.Stop();
        }
    }

    void ProcessRotation(){
        if(Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
            //When the rotation happens - emit particles from the proper hand.
            if(!rightHandParticles.isPlaying)
            {               
                rightHandParticles.Play();
            }
        } 
        else if(Input.GetKey(KeyCode.D))
        {
            //The other direction is basically the same, but negative.
            ApplyRotation(-rotationThrust);
            if(!leftHandParticles.isPlaying)
            {               
                leftHandParticles.Play();
            }
        }
        //
        else
        {
            rightHandParticles.Stop();
            leftHandParticles.Stop();
        }
    }

    //In C# all methods are private by default.
    private void ApplyRotation(float rotationThisFrame){
        //The physics system in Unity reacts with objects on impact which can cause conflicts with player controlled movement. In
        //practice it can make my demon fail to react to player's commands when bumping into other game objects. In order to
        //prevent it, I need to freeze the physics system rotation. The mechanism below will prevent the physics system from
        //having an impact on my demon if it's being issued a command to rotate.
        rb.freezeRotation = true;
        //I am reaching for the object's 'transform' property within Unity. The object is whatever the script is assigned to.
        //'Vector3.forward' means '0, 0, 1'.
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime * 100);
        //The value is additionally multiplied because there was no movement without it.
        rb.freezeRotation = false;
    }
}
