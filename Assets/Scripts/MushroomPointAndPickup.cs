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
    GameManagerScript GameManager;
    
    HandModelBase HandModelBase;
    private Hand _hand;
    Finger _index;
    Finger _middle;
    Finger _ring;
    Finger _pinky;
    [SerializeField]
    Vector3 offsetForPointing=new Vector3(0f,0f,0f);
    GameObject SelectedObject=null;

    GameObject AimedObject=null;
    LineRenderer laserLineRenderer;
    //public bool objectIsWithinRadius=false;
    Animator AimAnimator;
   
    Transform startMarker;
    Vector3 endMarker;
    public float speed = 0.1f;
    private float startTime;
    private float journeyLength;
    //private bool journeyEnd=false;

    private bool raycastOff=false;

    private MushroomInfo selectedComponentMushroonInfoScript;
    
    private float startPosfinMovementCheckValue;
    bool finMovementDone;
    Vector3 handPosition=new Vector3(0,0,0);
    void Start()
    {
         GameManager=GameManagerScript.instance;
        _hand = this.GetComponent<HandModelBase>().GetLeapHand();
        _index = _hand.GetIndex();
        _middle = _hand.GetMiddle();
        _ring = _hand.GetRing();
        _pinky = _hand.GetPinky();
        startPosfinMovementCheckValue=_hand.Direction.x;
        handPosition=_hand.WristPosition;
        //have to change if the hand's location is decided to change
        if(_hand.IsLeft)
        {
            offsetForPointing= new Vector3(-0.1f,0.18f,0f);
        }
        else{
            offsetForPointing= new Vector3(0.11f,0.18f,0f);
        }
        //laserLineRenderer=this.GetComponent<LineRenderer>();
        AimAnimator=GameManager.AimUIDisplay.transform.Find("Image").GetComponent<Animator>();
    }


    void Update()
    {
        // if(SelectedObject && !journeyEnd){
        //     float distCovered = (Time.time - startTime) * speed;
        //     float fractionOfJourney = distCovered / journeyLength;
        //     Debug.Log("Fraction:"+fractionOfJourney +"\nStartPos:"+startMarker.position+endMarker);
        //     SelectedObject.transform.position = Vector3.Lerp(startMarker.position, endMarker, fractionOfJourney);
        //     if(fractionOfJourney==1){
        //         journeyEnd=true;
        //     }
        // }
        //Debug.Log("Boolean:"+GameManager.objectIsSelected);
        // if(GameManager.isLeft){
        //     if(!_hand.IsLeft)
        //         raycastOff=true;
        // }
        // else
        // {
        //     if(_hand.IsLeft){
        //         //offsetForPointing= new Vector3(0.11f,0.18f,0f);
        //         raycastOff=true;
        //     }
        // }
    }


     void FixedUpdate()
    {
       float disdirect = Mathf.Abs(startPosfinMovementCheckValue - _hand.Direction.x);
      
       Debug.Log("Fin movement distance:"+disdirect);
        ///<summary>
        ///if the object comes near the hands the mushroom will fall on the floor
        ///the player has to pick/grab it(2 motions - pinch/grab and also any hand)
        ///</summary>
        if(GameManager.explorationMode){
            //journeyEnd=true;
        if(GameManager.objectIsSelected && selectedComponentMushroonInfoScript!=null){
             //ObjectIsPointed=null;
            // Debug.Log("Object Selected");
            if(AimAnimator.GetBool("Locked")){
                 if(selectedComponentMushroonInfoScript.isCoveredByLeaves && _hand.PalmNormal.y>0.25 && _hand.PalmNormal.y<0.6){
                    if(_middle.IsExtended && _ring.IsExtended && _pinky.IsExtended && _index.IsExtended){
                       //
                     
                            if(disdirect > 0.07) // checking if distance is less than required distance.
                            {
                                //Debug.Log("Fin movement done");
                                selectedComponentMushroonInfoScript.setLeavesAnimation();
                            }
                        
                    }
                 } else if(_hand.PalmNormal.y>0.7 && !selectedComponentMushroonInfoScript.isCoveredByLeaves){
                    if(!_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended && !_index.IsExtended){
                        SelectedObject.transform.position = GameManager.MushroomTranslatePoint.position;
                        GameManager.AimUIDisplay.SetActive(false);
                        // GameManager.explorationMode=false;
                        // GameManager.AimUIDisplay.SetActive(false);
                        //  // Keep a note of the time the movement started.
                        // startTime = Time.time;
                        // startMarker=SelectedObject.transform;
                        // endMarker=GameManager.MushroomTranslatePoint.position;

                        // // // Calculate the journey length.
                        // journeyLength = Vector3.Distance(startMarker.position, endMarker);
                    }
                 }else if(!_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended && _index.IsExtended){
                    GameManager.objectIsSelected=false;
                    SelectedObject=null;
                    AimedObject=null;
                    AimAnimator.SetBool("Locked",false);
                    AimAnimator.SetBool("Rotate",true);
                }
                
            }
        } else{
            //point and pull function
            if(!raycastOff)
             raycastFromTheIndexFinger();
        }
        }else{

        }
        startPosfinMovementCheckValue = _hand.Direction.x;
        
    }

    void raycastFromTheIndexFinger()
    {
        //RaycastObjects Layer
        int layerMask = 1 << 6;
        //check for pointing
       
        RaycastHit hit;
        Vector3 startPoint = transform.position+offsetForPointing;
        if (Physics.Raycast(startPoint, transform.TransformDirection(_index.Direction), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(startPoint, transform.TransformDirection(_index.Direction) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
            //Animate
            if(hit.transform.gameObject.tag=="Mushroom")
                objectHasBeenDetected(hit,startPoint);
            else if(AimedObject){ objectHasBeenDetectedObjectVersion(AimedObject,startPoint);
            }
        }
        else
        {
            Debug.DrawRay(startPoint, transform.TransformDirection(_index.Direction) * 1000, Color.white);
            //Debug.Log("Did not Hit");
            if(AimedObject){
                 objectHasBeenDetectedObjectVersion(AimedObject,startPoint);
            }
            //ObjectIsPointed=null;
            //GameManager.AimUIDisplay.SetActive(false);
        }
    }

    void objectHasBeenDetected(RaycastHit pointedMushroom,Vector3 endMarkerVector)
    {
        // Keep a note of the time the movement started.
        // startTime = Time.time;
        // startMarker=pointedMushroom.transform;
        // endMarker=endMarkerVector;

        // // Calculate the journey length.
        // journeyLength = Vector3.Distance(startMarker.position, endMarker);
        int indexInAllMushroomsList = GameManager.AllMushrooms.FindIndex(obj=>obj==pointedMushroom.transform.gameObject);
     
            if(indexInAllMushroomsList>=0){
                GameManager.AimUIDisplay.transform.position = GameManager.AllMushrooms[indexInAllMushroomsList].transform.position+new Vector3(0,-0.2f,0);
                GameManager.AimUIDisplay.SetActive(true);
            }
            
            if(!_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended){
               
                if(!AimAnimator.GetBool("Locked")){
                    SelectedObject=GameManager.AllMushrooms[indexInAllMushroomsList];
                    selectedComponentMushroonInfoScript = SelectedObject.GetComponent<MushroomInfo>();
                    StartCoroutine(WaitFunction());
                    GameManager.objectIsSelected=true;
                    AimAnimator.SetBool("Locked",true);
                    AimAnimator.SetBool("Rotate",false);
                    //Debug.Log("Animation is Done");
                  
                    
                    Debug.Log(Time.time);
                }
            }
        AimedObject=GameManager.AllMushrooms[indexInAllMushroomsList];
    }

     void objectHasBeenDetectedObjectVersion(GameObject pointedMushroom,Vector3 endMarkerVector)
    {
        // Keep a note of the time the movement started.
        // startTime = Time.time;
        // startMarker=pointedMushroom.transform;
        // endMarker=endMarkerVector;
        Debug.Log("RaycastOut"+pointedMushroom.tag);
        // // Calculate the journey length.
        // journeyLength = Vector3.Distance(startMarker.position, endMarker);
        int indexInAllMushroomsList = GameManager.AllMushrooms.FindIndex(obj=>obj==pointedMushroom);
            
            if(!_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended){
               
                if(!AimAnimator.GetBool("Locked")){
                    SelectedObject=GameManager.AllMushrooms[indexInAllMushroomsList];
                    selectedComponentMushroonInfoScript = SelectedObject.GetComponent<MushroomInfo>();
                    StartCoroutine(WaitFunction());
                    GameManager.objectIsSelected=true;
                    AimAnimator.SetBool("Locked",true);
                    AimAnimator.SetBool("Rotate",false);
                    //Debug.Log("Animation is Done");
                   
                    //Debug.Log(Time.time);
                }
            }
        //AimedObject=GameManager.AllMushrooms[indexInAllMushroomsList];
    }

IEnumerator WaitFunction(){
    //Debug.Log("Hello:"+Time.time);
    yield return new WaitForSeconds(2.0f);
    //Debug.Log(Time.time);
}
}
