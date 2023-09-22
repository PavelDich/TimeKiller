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
        transform.Rotate(x_speed * Time.deltaTime, y_speed * Time.deltaTime, z_speed * Time.deltaTime);
    }
}
