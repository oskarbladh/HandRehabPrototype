using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///RAYCAST ONLY NOT OBJECT///////////////////////////////////////////////////////////////
///Boolean script to turn off RAYCAST from the finger if it comes near the range of hands
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
           //Add this gameObject to MushroomsInRange for handling the camera movement when multiple mushrooms come in range
           if(!GameManager.MushroomsInRange.Contains(other.gameObject))
                GameManager.MushroomsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
         if(other.CompareTag("Mushroom")){
           GameManager.objectIsWithinRadius=false;
            //remove the gameObject from MushroomsInRange
        }
    }
}
