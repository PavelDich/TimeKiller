using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    public float x_speed = 0;
    public float y_speed = 0;
    public float z_speed = 1f;

    void Update()
    {
        transform.Rotate(x_speed, y_speed, z_speed);
    }
}
