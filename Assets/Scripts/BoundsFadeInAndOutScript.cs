using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsFadeInAndOutScript : MonoBehaviour
{
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject LeftHand;

    public float proximity = 0.1f;

    Renderer BoundObjectRenderer;
    Color color;

    float leftDistance = 0;
    float rightDistance = 0;
    float nearestDistance = 0;

    void Start()
    {
        BoundObjectRenderer = GetComponent<Renderer>();
        color = BoundObjectRenderer.material.color;
    }

    void Update()
    {
        // float absoluteDistance = Mathf.Abs(Vector3.Distance(_cube.transform.position, _sphere.transform.position));
        color = BoundObjectRenderer.material.color;
        leftDistance = Vector3.Distance(LeftHand.transform.position, transform.position);
        rightDistance = Vector3.Distance(RightHand.transform.position, transform.position);
        if (leftDistance > rightDistance)
        {
            nearestDistance = rightDistance;
        }
        else
        {
            nearestDistance = leftDistance;
        }
        setTheAlphaValue(nearestDistance);
        //Debug.Log("NearestDistance:"+nearestDistance);
    }

    void setTheAlphaValue(float distance)
    {
        color.a = Mathf.InverseLerp(proximity, 0.0f, distance);
        BoundObjectRenderer.material.color = color;
    }
}
