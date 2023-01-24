using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{

    //Variables for deciding the position of the game object I want to move. 'Vector3' is a standard way
    //as it allows me to specify x, y and z. I don't need to serialize the starting position as it will
    //be whatever it will be.
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    //This does not need to be of 'Vector3' data type because I will be applying only one number.
    //Adding the 'Range' makes the variable have a slider in Unity and gives me... a range of possible
    //values to choose from. Though given the math below, the range is no longer required and neither
    //is the serialization of the field since it's all been done internally.
    //[SerializeField] [Range(0,1)] float movementFactor;
    float movementFactor;
    [SerializeField] float period = 5f;
    void Start()
    {
        //The starting position is whatever the object's to which this script is attached position is.
        startingPosition = transform.position;
    }

    void Update()
    {
        //The entire mechanism below ensures that a game object will be constantly moving on its own from
        //one position to another and then back again.
        //'tau' - if I take the radius of a circle, make it vertical, bend it around the circle (the angle 
        //I get is called a radian) and count how many times it fits around the circle - I get the tau. It's
        //about 6.28. It's essentially PI * 2. 
        const float tau = Mathf.PI * 2;
        //If 'period' is ever zero, I will get a 'NaN error' as it's impossible to divide by zero. There is a
        //problem when comparing two floats since they can vary by a tiny amount (due to all the decimal places).
        //Using '==' is unpredictabe and I always should specify the acceptable difference. The smallest float
        //I have access to is 'Mathf.Epsilon' - it's better to compare to it rather than comparing to zero.
        //if(period == 0) { return; }
        if(period == Mathf.Epsilon) { return; }
        //The cycle of movement (back and forth) for a floating object equals how much time has elapsed divided
        //by my custom variable.
        float cycles = Time.time / period;
        //The below will give me a value between 1 and -1.
        float rawSinWave = Mathf.Sin(cycles * tau);
        //The below will ensure the movemenet factor (the range) will be moving from 0 to 1 instead of from -1 to 1.
        movementFactor = (rawSinWave + 1f) / 2f;
        //The position of the game object will be changed by the value of Vector3's values multiplied
        //by whatever I specified with the slider (introduced by 'Range').
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
