using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

///<summary>
///Creates Raycast from the pointer finger of the hand to which the script is attached.
///Lerps the mushroom nearby to the hand, if it is pointed.
///</summary>
public class MushroomPointAndPickup : MonoBehaviour
{
    [SerializeField]
    GameManagerScript GameManager;
    [SerializeField]
    HandModelBase HandModelBase;
    private Hand _hand;
    Finger _index;
    Finger _middle;
    Finger _ring;
    Finger _pinky;
    [SerializeField]
    Vector3 offsetForPointing=new Vector3(0f,0f,0f);
   
    Transform startMarker;
    Vector3 endMarker;
    public float speed = 0.1F;
    private float startTime;
    private float journeyLength;

    GameObject ObjectIsPointed=null;
    //public bool objectIsWithinRadius=false;

    void Start()
    {
        _hand = this.GetComponent<HandModelBase>().GetLeapHand();
        _index = _hand.GetIndex();
        _middle = _hand.GetMiddle();
        _ring = _hand.GetRing();
        _pinky = _hand.GetPinky();
        //have to change if the hand's location is decided to change
        if(_hand.IsLeft)
        {
            offsetForPointing= new Vector3(-0.1f,0.18f,0f);
        }
        else{
            offsetForPointing= new Vector3(0.11f,0.18f,0f);
        }
    }


    void Update()
    {
        if(ObjectIsPointed){
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            ObjectIsPointed.transform.position = Vector3.Lerp(startMarker.position, endMarker, fractionOfJourney);
        }
    }

     void FixedUpdate()
    {
        ///<summary>
        ///if the object comes near the hands the mushroom will fall on the floor
        ///the player has to pick/grab it(2 motions - pinch/grab and also any hand) 
        ///</summary>
        if(!GameManager.objectIsWithinRadius){
            //point and pull function
            raycastFromTheIndexFinger();
        } else{
            ObjectIsPointed=null;
        }
        
    }

    void raycastFromTheIndexFinger(){
         //RaycastObjects Layer
        int layerMask = 1 << 6;
        //check for pointing
        if(_index.IsExtended && !_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended){
            RaycastHit hit;
            Vector3 startPoint = transform.position+offsetForPointing;
            if (Physics.Raycast(startPoint, transform.TransformDirection(_index.Direction), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(startPoint, transform.TransformDirection(_index.Direction) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                //Animate
                objectHasBeenDetected(hit,startPoint);
            }
            else
            {
                Debug.DrawRay(startPoint, transform.TransformDirection(_index.Direction) * 1000, Color.white);
                Debug.Log("Did not Hit");
                ObjectIsPointed=null;
            }
        }
        else
        {
            //Debug.Log("Close your fingers and point at the object");
        }
    }

    void objectHasBeenDetected(RaycastHit pointedMushroom,Vector3 endMarkerVector)
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;
        startMarker=pointedMushroom.transform;
        endMarker=endMarkerVector;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker);

       ObjectIsPointed=pointedMushroom.transform.gameObject;
    }


}
