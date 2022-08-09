using UnityEngine;
using System;
using System.Collections.Generic;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;
    // Update is called once per frame
    private void Update()
    {
        float x = UnityEngine.Random.Range(1f, 2f);
        float y = UnityEngine.Random.Range(1f, 2f);
        float z = UnityEngine.Random.Range(1f, 2f);
        transform.Rotate(x* rotationSpeed*Time.deltaTime, y * rotationSpeed * Time.deltaTime, z * rotationSpeed * Time.deltaTime);
    }
    
}
