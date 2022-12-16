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
    AimAnimator = GameManager.AimUIDisplay.transform.Find("Image").GetComponent<Animator>();
    GameManager.AimUIDisplay.transform.position = GameManager.AllMushrooms[GameManager.currentIndex].transform.position + new Vector3(0, -0.1f, 0);
    totalCountOfMushrooms = GameManager.AllMushrooms.Count;
    GameManager.AimUIDisplay.SetActive(true);
    leftHand = _hand.IsLeft && isLeftHand && GameManager.isLeft;
    rightHand = !_hand.IsLeft && isRightHand && !GameManager.isLeft;
  }


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
    totalCountOfMushrooms = GameManager.AllMushrooms.Count;
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
    ////code that needs to be executed for the rehab hand which needs exercise
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
            if (GameManager.selectedComponentMushroonInfoScript.isCoveredByLeaves && _hand.PalmNormal.y > -0.2 && _hand.PalmNormal.y < 0.4)
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
    //given in start so that we can reduce a certain Time lag
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
          GameManager.AimUIDisplay.transform.position = GameManager.AllMushrooms[GameManager.currentIndex % GameManager.AllMushrooms.Count].transform.position + new Vector3(0, -0.1f, 0);
          if (lastMovedTarget > cooldown)
          {
            if (_hand.PalmNormal.y > -0.5 && _hand.PalmNormal.y < 0.5f && !AimAnimator.GetBool("Locked"))
            {
              if (_middle.IsExtended && _ring.IsExtended && _pinky.IsExtended && _index.IsExtended)
              {
                if (disdirect > finParam) // checking if distance is less than required distance.
                {
                  Debug.Log("Fin movement done" + pointedLeft + "Index:" + GameManager.currentIndex);
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
                    GameManager.AimUIDisplay.transform.position = GameManager.AllMushrooms[GameManager.currentIndex].transform.position + new Vector3(0, -0.1f, 0);
                    GameManager.AimUIDisplay.SetActive(true);
                    lastMovedTarget = 0;
                  }
                }
              }
            }
          }
          else
          {
            //delay time variable updated for seamless transition from one target to another so that it doesn't happen for every frame
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
            Debug.Log("Nidex:" + GameManager.currentIndex);
            if (GameManager.AllMushrooms.Count == 1 && GameManager.currentIndex == 1)
              GameManager.currentIndex = 0;
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

      GameManager.selectedComponentMushroonInfoScript = null;
      GameManager.AimUIDisplay.SetActive(false);
    }
    //fin movement calculation variable
    startPosfinMovementCheckValue = _hand.Direction.x;
  }

}
