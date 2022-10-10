using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInRangeScript : MonoBehaviour
{
    [SerializeField]
    GameObject raycastRightHand;
    [SerializeField]
    GameObject raycastLeftHand;
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
            raycastLeftHand.GetComponent<MushroomPointAndPickup>().objectIsWithinRadius=true;
            raycastRightHand.GetComponent<MushroomPointAndPickup>().objectIsWithinRadius=true;
        }
    }

    private void OnTriggerExit(Collider other) {
         if(other.CompareTag("Mushroom")){
            raycastLeftHand.GetComponent<MushroomPointAndPickup>().objectIsWithinRadius=false;
            raycastRightHand.GetComponent<MushroomPointAndPickup>().objectIsWithinRadius=false;
            //have to reset its position when it falls back of the hands
        }
    }
}
