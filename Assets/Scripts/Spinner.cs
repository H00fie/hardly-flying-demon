using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, -0.2f);
    }
}
