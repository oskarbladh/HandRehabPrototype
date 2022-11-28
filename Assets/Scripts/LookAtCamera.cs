using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // [SerializeField]
    // GameObject Camera;

    public float rotationSpeed = 10f;

    void Start()
    {
        transform.LookAt(Camera.main.transform);
    }


    void Update()
    {

        transform.Rotate(0, 0, 20.0f * rotationSpeed * Time.deltaTime);
    }
}
