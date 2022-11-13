using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedMushroomSizeChangeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.gameObject.activeSelf){
            foreach(Transform child in transform) {
              child.localScale = new Vector3(0.5f,0.5f,0.5f);
            }
        }
    }
}
