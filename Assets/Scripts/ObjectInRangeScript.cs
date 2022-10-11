using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Boolean script to turn off raycast from the finger if it comes near the range of hands
///</summary>
public class ObjectInRangeScript : MonoBehaviour
{
    [SerializeField]
    GameManagerScript GameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        
        if(other.CompareTag("Mushroom")){
            Debug.Log("Triggered");
           GameManager.objectIsWithinRadius=true;
        }
    }

    private void OnTriggerExit(Collider other) {
         if(other.CompareTag("Mushroom")){
           GameManager.objectIsWithinRadius=false;
            //have to reset its position when it falls back of the hands
        }
    }
}
