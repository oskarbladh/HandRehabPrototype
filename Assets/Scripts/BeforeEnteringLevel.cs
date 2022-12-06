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
public class BeforeEnteringLevel : MonoBehaviour
{
  GameManagerScript GameManager;

  HandModelBase HandModelBase;
  private Hand _hand;
  Finger _index;
  Finger _middle;
  Finger _ring;
  Finger _pinky;

  private float startPosfinMovementCheckValue;
  Vector3 handPosition = new Vector3(0, 0, 0);


  public float cooldown = 2f;
  private float lastMovedTarget = 3f;

  public float rotateParam = 0.7f;

  public float finParam = 0.08f;

  public float grabParam = 0.8f;
  public bool isLeftHand;

  public bool isRightHand;

  bool leftHand;
  bool rightHand;

  bool finMovementBool = false;

  bool leavesMovementBool = false;

  bool closeFistBool = false;

  bool rotateFistBool = false;

  bool pickingDropBool = false;

  bool finMovementCompletedBool = false;

  bool leavesMovementCompletedBool = false;

  bool closeFistCompletedBool = false;

  bool rotateFistCompletedBool = false;

  bool pickingDropCompletedBool = false;

  bool tutorialStarted = false;

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
    leftHand = _hand.IsLeft && isLeftHand && GameManager.isLeft;
    rightHand = !_hand.IsLeft && isRightHand && !GameManager.isLeft;
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
          //show animations
          //have functions for each motion and execute them in order to complete
          //change over the scripts after completion
          if (!tutorialStarted)
          {
            selectingTargetActivationFlow();
          }
          if (rotateFistCompletedBool)
          {
            GameManager.TutorialsScreen.SetActive(false);
            GameManager.changeToMainScript();
            //turnOffTheTutScreen
            //change over the scripts after completion
          }
          else if (closeFistCompletedBool)
          {
            rotateFistActivationFlow();
            GameManager.completedToggle.isOn = false;
          }
          else if (finMovementCompletedBool)
          {
            closeFistActivationFlow();
            GameManager.completedToggle.isOn = false;
          }
        }
        break;
      // case "2":
      //   {
      //     if (!tutorialStarted)
      //     {
      //       pickAndDropActivationFlow();

      //     }
      //     if (pickingDropCompletedBool)
      //     {
      //       GameManager.TutorialsScreen.SetActive(false);
      //       GameManager.changeToMainScript();
      //       //turnOffTheTutScreen
      //       //change over the scripts after completion
      //     }
      //   }
      //   break;
      case "4":
        {
          if (!tutorialStarted)
          {
            leavesHandMotionsActivationFlow();
          }
          if (leavesMovementCompletedBool)
          {
            GameManager.TutorialsScreen.SetActive(false);
            GameManager.changeToMainScript();
            //turnOffTheTutScreen
            //change over the scripts after completion
          }
        }
        break;
      default:
        {
          GameManager.TutorialsScreen.SetActive(false);
          this.gameObject.GetComponent<MushroomPointAndPickup>().enabled = true;
          this.enabled = false;
          //turnOffTheTutScreen
          //change over the scripts after completion
        }
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
      if (leavesMovementBool && !leavesMovementCompletedBool)
      {
        float disDiffForOtherHand = Mathf.Abs(startPosfinMovementCheckValue - _hand.Direction.x);
        //Debug.Log("Other hand" + disDiffForOtherHand);
        if (_hand.PalmNormal.y > -0.2 && _hand.PalmNormal.y < 0.4)
        {
          if (_middle.IsExtended && _ring.IsExtended && _pinky.IsExtended && _index.IsExtended)
          {
            if (disDiffForOtherHand > 0.07) // checking if distance is less than required distance.
            {
              Debug.Log("Fin movement done on other hand");
              completionOfleavesHandMotions();
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

    //Code to be executed when the object is in locked state
    //Arm twist/rotate
    if (rotateFistBool && !rotateFistCompletedBool && _hand.PalmNormal.y > rotateParam)
    {
      if (!_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended && !_index.IsExtended)
      {
        Debug.Log("Object Selected inside");
        completionOfrotateFist();
      }
    }
    //Unlock target
    // if (_hand.GrabStrength < grabParam || (_middle.IsExtended && _ring.IsExtended && _pinky.IsExtended && _index.IsExtended))
    // {
    //   //completionOfOpenFist();
    // }

    //move between targets
    if (finMovementBool && !finMovementCompletedBool)
    {
      if (_hand.PalmNormal.y > -0.5 && _hand.PalmNormal.y < 0.5f)
      {
        if (_middle.IsExtended && _ring.IsExtended && _pinky.IsExtended && _index.IsExtended)
        {
          if (disdirect > finParam) // checking if distance is less than required distance.
          {
            Debug.Log("Fin movement done");
            completionOfFinMovement();
          }
        }
      }
    }

    //Locking of the mushroom
    if (closeFistBool && !closeFistCompletedBool && (!_index.IsExtended && !_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended && _hand.GrabStrength > grabParam && _hand.PalmNormal.y < rotateParam))
    {
      completionOfcloseFist();
    }

    //should add pick and drop motion condition

    //fin movement calculation variable
    startPosfinMovementCheckValue = _hand.Direction.x;
  }

  void selectingTargetActivationFlow()
  {
    GameManager.TutorialsScreen.SetActive(true);
    finMovementBool = true;
    GameManager.tutorialsVideoPlayer.clip = GameManager.finMovementClip;
    GameManager.movementAnimatorModule = GameManager.finMovementAnim;
  }

  void completionOfFinMovement()
  {
    GameManager.completedToggle.isOn = true;
    StartCoroutine(ChangeBool("fin"));
  }

  void closeFistActivationFlow()
  {
    GameManager.TutorialsScreen.SetActive(true);
    closeFistBool = true;
    GameManager.tutorialsVideoPlayer.clip = GameManager.closeFistClip;
    GameManager.movementAnimatorModule = GameManager.closeFistAnim;

  }

  void completionOfcloseFist()
  {
    GameManager.completedToggle.isOn = true;
    StartCoroutine(ChangeBool("close"));
  }

  void rotateFistActivationFlow()
  {
    GameManager.TutorialsScreen.SetActive(true);
    rotateFistBool = true;
    GameManager.tutorialsVideoPlayer.clip = GameManager.rotateFistClip;
    GameManager.movementAnimatorModule = GameManager.rotateFistAnim;

  }

  void completionOfrotateFist()
  {
    GameManager.completedToggle.isOn = true;
    StartCoroutine(ChangeBool("rotate"));
  }

  void pickAndDropActivationFlow()
  {
    GameManager.TutorialsScreen.SetActive(true);
    pickingDropBool = true;
    GameManager.tutorialsVideoPlayer.clip = GameManager.pickAndGrabClip;
    GameManager.movementAnimatorModule = GameManager.pickAndGrabAnim;

  }

  void completionOfpickAndDrop()
  {
    GameManager.completedToggle.isOn = true;
    StartCoroutine(ChangeBool("grab"));
  }

  void leavesHandMotionsActivationFlow()
  {
    GameManager.TutorialsScreen.SetActive(true);
    leavesMovementBool = true;
    GameManager.tutorialsVideoPlayer.clip = GameManager.leavesMotionClip;
    GameManager.movementAnimatorModule = GameManager.leavesMotionAnim;

  }

  void completionOfleavesHandMotions()
  {
    GameManager.completedToggle.isOn = true;
    StartCoroutine(ChangeBool("leaves"));
  }

  IEnumerator ChangeBool(string name)
  {
    yield return new WaitForSeconds(1.0f);
    switch (name)
    {
      case "fin":
        finMovementCompletedBool = true;
        break;
      case "close":
        closeFistCompletedBool = true;
        break;
      case "rotate":
        rotateFistCompletedBool = true;
        break;
      case "grab":
        pickingDropCompletedBool = true;
        break;
      case "leaves":
        leavesMovementCompletedBool = true;
        break;
    }
  }

}
