using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

///<summary>
///Use Fin motion to change targets and lock by clenching your fist and rotate your hand 
///and the mushroom comes to the grabbing range
///Leaves removal functionality is also done here
///Other dominent hand code is also written here
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
  Vector3 offsetForPointing = new Vector3(0f, 0f, 0f);
  GameObject SelectedObject = null;

  GameObject AimedObject = null;
  LineRenderer laserLineRenderer;
  //public bool objectIsWithinRadius=false;
  Animator AimAnimator;

  Transform startMarker;
  Vector3 endMarker;
  public float speed = 0.1f;
  private float startTime;
  private float journeyLength;
  //private bool journeyEnd=false;

  //private bool raycastOff=false;



  private float startPosfinMovementCheckValue;
  bool finMovementDone;
  Vector3 handPosition = new Vector3(0, 0, 0);

  int totalCountOfMushrooms = 0;

  public float cooldown = 2f;
  private float lastMovedTarget = 3f;

  public float rotateParam = 0.7f;

  public float finParam = 0.08f;

  public float grabParam = 0.8f;
  public bool isLeftHand;

  public bool isRightHand;

  bool leftHand;
  bool rightHand;

  private bool pointedLeft;

  bool finMovementBool;

  bool leavesMovementBool;

  bool targetMovementBool;

  bool comeToGrabRangeBool;

  bool pointedCenter = true;
  void Start()
  {
    GameManager = GameManagerScript.instance;
    _hand = this.GetComponent<HandModelBase>().GetLeapHand();
    _index = _hand.GetIndex();
    _middle = _hand.GetMiddle();
    _ring = _hand.GetRing();
    _pinky = _hand.GetPinky();
    startPosfinMovementCheckValue = _hand.Direction.x;
    handPosition = _hand.WristPosition;
    //have to change if the hand's location is decided to change
    // if(_hand.IsLeft)
    // {
    //     offsetForPointing= new Vector3(-0.1f,0.18f,0f);
    // }
    // else{
    //     offsetForPointing= new Vector3(0.11f,0.18f,0f);
    // }
    //laserLineRenderer=this.GetComponent<LineRenderer>();
    AimAnimator = GameManager.AimUIDisplay.transform.Find("Image").GetComponent<Animator>();
    GameManager.AimUIDisplay.transform.position = GameManager.AllMushrooms[GameManager.currentIndex].transform.position + new Vector3(0, -0.2f, 0);
    totalCountOfMushrooms = GameManager.AllMushrooms.Count;
    GameManager.AimUIDisplay.SetActive(true);
    leftHand = _hand.IsLeft && isLeftHand && GameManager.isLeft;
    rightHand = !_hand.IsLeft && isRightHand && !GameManager.isLeft;
  }


  // void Update()
  // {
  //     // if(SelectedObject && !journeyEnd){
  //     //     float distCovered = (Time.time - startTime) * speed;
  //     //     float fractionOfJourney = distCovered / journeyLength;
  //     //     Debug.Log("Fraction:"+fractionOfJourney +"\nStartPos:"+startMarker.position+endMarker);
  //     //     SelectedObject.transform.position = Vector3.Lerp(startMarker.position, endMarker, fractionOfJourney);
  //     //     if(fractionOfJourney==1){
  //     //         journeyEnd=true;
  //     //     }
  //     // }
  //     //Debug.Log("Boolean:"+GameManager.objectIsSelected);
  //     // if(GameManager.isLeft){
  //     //     if(!_hand.IsLeft)
  //     //         raycastOff=true;
  //     // }
  //     // else
  //     // {
  //     //     if(_hand.IsLeft){
  //     //         //offsetForPointing= new Vector3(0.11f,0.18f,0f);
  //     //         raycastOff=true;
  //     //     }
  //     // }
  // }


  //      void FixedUpdate()
  //     {
  //        float disdirect = Mathf.Abs(startPosfinMovementCheckValue - _hand.Direction.x);

  //        Debug.Log("Fin movement distance:"+disdirect);
  //         ///<summary>
  //         ///if the object comes near the hands the mushroom will fall on the floor
  //         ///the player has to pick/grab it(2 motions - pinch/grab and also any hand)
  //         ///</summary>
  //         if(GameManager.explorationMode){
  //             //journeyEnd=true;
  //         if(GameManager.objectIsSelected && selectedComponentMushroonInfoScript!=null){
  //              //ObjectIsPointed=null;
  //             // Debug.Log("Object Selected");
  //             if(AimAnimator.GetBool("Locked")){
  //                  if(selectedComponentMushroonInfoScript.isCoveredByLeaves && _hand.PalmNormal.y>0.25 && _hand.PalmNormal.y<0.6){
  //                     if(_middle.IsExtended && _ring.IsExtended && _pinky.IsExtended && _index.IsExtended){
  //                        //

  //                             if(disdirect > 0.07) // checking if distance is less than required distance.
  //                             {
  //                                 //Debug.Log("Fin movement done");
  //                                 selectedComponentMushroonInfoScript.setLeavesAnimation();
  //                             }

  //                     }
  //                  } else if(_hand.PalmNormal.y>0.7 && !selectedComponentMushroonInfoScript.isCoveredByLeaves){
  //                     if(!_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended && !_index.IsExtended){
  //                         SelectedObject.transform.position = GameManager.MushroomTranslatePoint.position;
  //                         GameManager.AimUIDisplay.SetActive(false);
  //                         // GameManager.explorationMode=false;
  //                         // GameManager.AimUIDisplay.SetActive(false);
  //                         //  // Keep a note of the time the movement started.
  //                         // startTime = Time.time;
  //                         // startMarker=SelectedObject.transform;
  //                         // endMarker=GameManager.MushroomTranslatePoint.position;

  //                         // // // Calculate the journey length.
  //                         // journeyLength = Vector3.Distance(startMarker.position, endMarker);
  //                     }
  //                  }else if(!_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended && _index.IsExtended){
  //                     GameManager.objectIsSelected=false;
  //                     SelectedObject=null;
  //                     AimedObject=null;
  //                     AimAnimator.SetBool("Locked",false);
  //                     AimAnimator.SetBool("Rotate",true);
  //                 }

  //             }
  //         } else{
  //             //point and pull function
  //             if(!raycastOff)
  //              raycastFromTheIndexFinger();
  //         }
  //         }else{

  //         }
  //         startPosfinMovementCheckValue = _hand.Direction.x;

  //     }

  //     void raycastFromTheIndexFinger()
  //     {
  //         //RaycastObjects Layer
  //         int layerMask = 1 << 6;
  //         //check for pointing

  //         RaycastHit hit;
  //         Vector3 startPoint = transform.position+offsetForPointing;
  //         if (Physics.Raycast(startPoint, transform.TransformDirection(_index.Direction), out hit, Mathf.Infinity))
  //         {
  //             Debug.DrawRay(startPoint, transform.TransformDirection(_index.Direction) * hit.distance, Color.yellow);
  //             //Debug.Log("Did Hit");
  //             //Animate
  //             if(hit.transform.gameObject.tag=="Mushroom")
  //                 objectHasBeenDetected(hit,startPoint);
  //             else if(AimedObject){ objectHasBeenDetectedObjectVersion(AimedObject,startPoint);
  //             }
  //         }
  //         else
  //         {
  //             Debug.DrawRay(startPoint, transform.TransformDirection(_index.Direction) * 1000, Color.white);
  //             //Debug.Log("Did not Hit");
  //             if(AimedObject){
  //                  objectHasBeenDetectedObjectVersion(AimedObject,startPoint);
  //             }
  //             //ObjectIsPointed=null;
  //             //GameManager.AimUIDisplay.SetActive(false);
  //         }
  //     }

  //     void objectHasBeenDetected(RaycastHit pointedMushroom,Vector3 endMarkerVector)
  //     {
  //         // Keep a note of the time the movement started.
  //         // startTime = Time.time;
  //         // startMarker=pointedMushroom.transform;
  //         // endMarker=endMarkerVector;

  //         // // Calculate the journey length.
  //         // journeyLength = Vector3.Distance(startMarker.position, endMarker);
  //         int indexInAllMushroomsList = GameManager.AllMushrooms.FindIndex(obj=>obj==pointedMushroom.transform.gameObject);

  //             if(indexInAllMushroomsList>=0){
  //                 GameManager.AimUIDisplay.transform.position = GameManager.AllMushrooms[indexInAllMushroomsList].transform.position+new Vector3(0,-0.2f,0);
  //                 GameManager.AimUIDisplay.SetActive(true);
  //             }

  //             if(!_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended){

  //                 if(!AimAnimator.GetBool("Locked")){
  //                     SelectedObject=GameManager.AllMushrooms[indexInAllMushroomsList];
  //                     selectedComponentMushroonInfoScript = SelectedObject.GetComponent<MushroomInfo>();
  //                     StartCoroutine(WaitFunction());
  //                     GameManager.objectIsSelected=true;
  //                     AimAnimator.SetBool("Locked",true);
  //                     AimAnimator.SetBool("Rotate",false);
  //                     //Debug.Log("Animation is Done");


  //                     Debug.Log(Time.time);
  //                 }
  //             }
  //         AimedObject=GameManager.AllMushrooms[indexInAllMushroomsList];
  //     }

  //      void objectHasBeenDetectedObjectVersion(GameObject pointedMushroom,Vector3 endMarkerVector)
  //     {
  //         // Keep a note of the time the movement started.
  //         // startTime = Time.time;
  //         // startMarker=pointedMushroom.transform;
  //         // endMarker=endMarkerVector;
  //         Debug.Log("RaycastOut"+pointedMushroom.tag);
  //         // // Calculate the journey length.
  //         // journeyLength = Vector3.Distance(startMarker.position, endMarker);
  //         int indexInAllMushroomsList = GameManager.AllMushrooms.FindIndex(obj=>obj==pointedMushroom);

  //             if(!_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended){

  //                 if(!AimAnimator.GetBool("Locked")){
  //                     SelectedObject=GameManager.AllMushrooms[indexInAllMushroomsList];
  //                     selectedComponentMushroonInfoScript = SelectedObject.GetComponent<MushroomInfo>();
  //                     StartCoroutine(WaitFunction());
  //                     GameManager.objectIsSelected=true;
  //                     AimAnimator.SetBool("Locked",true);
  //                     AimAnimator.SetBool("Rotate",false);
  //                     //Debug.Log("Animation is Done");

  //                     //Debug.Log(Time.time);
  //                 }
  //             }
  //         //AimedObject=GameManager.AllMushrooms[indexInAllMushroomsList];
  //     }

  IEnumerator WaitFunction()
  {
    //Debug.Log("Hello:"+Time.time);
    yield return new WaitForSeconds(2.0f);
    //Debug.Log(Time.time);
  }

  void Update()
  {
    //change the functionality for left or right hand as important one
    leftHand = _hand.IsLeft && isLeftHand && GameManager.isLeft;
    rightHand = !_hand.IsLeft && isRightHand && !GameManager.isLeft;

    ////Game code and functionalities based on the levels
    switch (GameManager.levelName)
    {
      case "1":
        {
          targetMovementBool = true;
          finMovementBool = true;
          leavesMovementBool = false;
          comeToGrabRangeBool = true;
        }
        break;
      case "2":
        {
          targetMovementBool = false;
          finMovementBool = false;
          leavesMovementBool = false;
          comeToGrabRangeBool = false;
          //Debug.Log("Executed");
        }
        break;
      case "3":
        {
          targetMovementBool = true;
          finMovementBool = true;
          leavesMovementBool = false;
          comeToGrabRangeBool = true;
        }
        break;
      case "4":
        {
          targetMovementBool = true;
          finMovementBool = true;
          leavesMovementBool = true;
          comeToGrabRangeBool = false;
        }
        break;
      case "5":
        {
          targetMovementBool = true;
          finMovementBool = true;
          leavesMovementBool = true;
          comeToGrabRangeBool = true;
        }
        break;
      case "6":
        {
          targetMovementBool = true;
          finMovementBool = true;
          leavesMovementBool = true;
          comeToGrabRangeBool = true;
        }
        break;
      default:
        Debug.Log("Whatishappening");
        break;
    }
  }

  void FixedUpdate()
  {
    ////execute code for the rehab hand which needs exercise
    if (leftHand)
    {
      executeHandFuncitionalities();
    }
    else if (rightHand)
    {
      executeHandFuncitionalities();
    }
    else
    {
      ////if it is left hand -> write the functionality for right hand and vice versa
      if (leavesMovementBool)
      {
        float disDiffForOtherHand = Mathf.Abs(startPosfinMovementCheckValue - _hand.Direction.x);
        //Debug.Log("Other hand" + disDiffForOtherHand);
        if (GameManager.objectIsSelected && GameManager.selectedComponentMushroonInfoScript != null)
        {
          //ObjectIsPointed=null;
          Debug.Log("Object Selected");
          if (AimAnimator.GetBool("Locked"))
          {
            if (GameManager.selectedComponentMushroonInfoScript.isCoveredByLeaves && _hand.PalmNormal.y > 0.4 && _hand.PalmNormal.y < 0.8)
            {
              if (_middle.IsExtended && _ring.IsExtended && _pinky.IsExtended && _index.IsExtended)
              {
                if (disDiffForOtherHand > 0.07) // checking if distance is less than required distance.
                {
                  Debug.Log("Fin movement done");
                  GameManager.selectedComponentMushroonInfoScript.setLeavesAnimation();
                }
              }
            }
          }
        }
      }
    }
  }

  ///All the hand related functionalities are written here except the non important hand
  void executeHandFuncitionalities()
  {
    //to determine the distance between last checkposition and current hands position
    float disdirect = Mathf.Abs(startPosfinMovementCheckValue - _hand.Direction.x);

    //variable set for Fin movement for traversing target to left or right
    if (_hand.Direction.x < -0.4f)
    {
      pointedLeft = true;
      pointedCenter = false;
      Debug.Log("Fin Left movement done:" + _hand.Direction.x);
    }
    else if (_hand.Direction.x > 0.3f)
    {
      pointedLeft = false;
      pointedCenter = false;
      Debug.Log("Fin Right movement done:" + _hand.Direction.x);
    }
    else
    {
      pointedCenter = true;
    }


    if (GameManager.explorationMode)
    {
      //Code to be executed when the object is in locked state
      if (GameManager.objectIsSelected && GameManager.selectedComponentMushroonInfoScript != null)
      {
        //Debug.Log("Object Selected outside outside");
        //Arm twist/rotate
        if (comeToGrabRangeBool && _hand.PalmNormal.y > rotateParam)
        {
          Debug.Log("Object Selected outside");
          if (!_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended && !_index.IsExtended && !GameManager.selectedComponentMushroonInfoScript.isCoveredByLeaves)
          {
            Debug.Log("Object Selected inside");
            SelectedObject.transform.position = GameManager.MushroomTranslatePoint.position;
            GameManager.AimUIDisplay.SetActive(false);
            GameManager.explorationMode = false;
            lastMovedTarget = 0;
          }
        }
        //Unlock target
        else if (_hand.GrabStrength < grabParam || (_middle.IsExtended && _ring.IsExtended && _pinky.IsExtended && _index.IsExtended))
        {
          GameManager.objectIsSelected = false;
          SelectedObject = null;
          AimedObject = null;
          GameManager.explorationMode = true;
          AimAnimator.SetBool("Locked", false);
          GameManager.selectedComponentMushroonInfoScript = null;
          AimAnimator.SetBool("Rotate", true);
          lastMovedTarget = 0;
        }
      }
      else
      {
        //move between targets
        if (finMovementBool)
        {
          GameManager.AimUIDisplay.SetActive(true);
          GameManager.AimUIDisplay.transform.position = GameManager.AllMushrooms[GameManager.currentIndex % GameManager.AllMushrooms.Count].transform.position + new Vector3(0, -0.2f, 0);
          if (lastMovedTarget > cooldown)
          {
            if (_hand.PalmNormal.y > -0.5 && _hand.PalmNormal.y < 0.5f && !AimAnimator.GetBool("Locked"))
            {
              if (_middle.IsExtended && _ring.IsExtended && _pinky.IsExtended && _index.IsExtended)
              {
                if (disdirect > finParam) // checking if distance is less than required distance.
                {
                  Debug.Log("Fin movement done" + pointedLeft + "Index:" + GameManager.currentIndex);
                  //                  if(_hand.Direction.x<-0.5f)
                  // {
                  //     pointedLeft = true;
                  //     Debug.Log("Fin Left movement done:"+_hand.Direction.x);
                  // }
                  // else if(_hand.Direction.x>0.5f){
                  //     pointedLeft = false;
                  //     Debug.Log("Fin Right movement done:"+_hand.Direction.x);
                  // }
                  //                 //Debug.Log("Fin movement distance:"+disdirect);
                  if (!pointedCenter)
                  {
                    if (pointedLeft)
                    {
                      if (GameManager.currentIndex == 0)
                      {
                        GameManager.currentIndex = (totalCountOfMushrooms - 1) % totalCountOfMushrooms;
                      }
                      else
                      {
                        GameManager.currentIndex = (GameManager.currentIndex - 1) % totalCountOfMushrooms;
                      }
                    }
                    else
                    {
                      GameManager.currentIndex = (GameManager.currentIndex + 1) % totalCountOfMushrooms;
                    }

                    //GameManager.currentIndex = (GameManager.currentIndex + 1) % totalCountOfMushrooms;
                    GameManager.AimUIDisplay.transform.position = GameManager.AllMushrooms[GameManager.currentIndex].transform.position + new Vector3(0, -0.2f, 0);
                    GameManager.AimUIDisplay.SetActive(true);
                    lastMovedTarget = 0;
                  }
                }
              }
            }
          }
          else
          {
            //delay time variable updated for seamless transition from one target to another
            lastMovedTarget += Time.deltaTime;
          }
        }
        else
        {
          GameManager.AimUIDisplay.SetActive(false);
        }

        //Locking of the mushroom
        if (targetMovementBool && (!_index.IsExtended && !_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended && _hand.GrabStrength > grabParam && _hand.PalmNormal.y < rotateParam))
        {
          if (!AimAnimator.GetBool("Locked"))
          {
            SelectedObject = GameManager.AllMushrooms[GameManager.currentIndex];
            GameManager.selectedComponentMushroonInfoScript = SelectedObject.GetComponent<MushroomInfo>();
            GameManager.objectIsSelected = true;
            AimAnimator.SetBool("Locked", true);
            AimAnimator.SetBool("Rotate", false);
            //Debug.Log("Animation is Done");
          }
        }
      }
    }

    else
    {
      //examination mode code { zoomed in screen }
      //reseting values
      AimAnimator.SetBool("Locked", false);
      AimAnimator.SetBool("Rotate", true);

      //
      GameManager.selectedComponentMushroonInfoScript = null;
      GameManager.AimUIDisplay.SetActive(false);
    }
    startPosfinMovementCheckValue = _hand.Direction.x;
  }

}
