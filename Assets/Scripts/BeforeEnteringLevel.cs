using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using TMPro;

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
  bool tutorialStarted2 = false;

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
        if (!GameManager.completedToggle.isOn)
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
            GameManager.movementAnimatorModule.SetBool("Fin", false);
            completionOfFinMovement();
          }
        }
      }
    }

    //Locking of the mushroom
    if (closeFistBool && !closeFistCompletedBool && (!_index.IsExtended && !_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended && _hand.GrabStrength > grabParam && _hand.PalmNormal.y < rotateParam))
    {
      if (!tutorialStarted2)
        completionOfcloseFist();
      GameManager.movementAnimatorModule.SetBool("CloseFist", false);
    }

    //should add pick and drop motion condition

    //fin movement calculation variable
    startPosfinMovementCheckValue = _hand.Direction.x;
  }

  void selectingTargetActivationFlow()
  {
    GameManager.TaskText.text = "Move between targets\nMotion - Fin Motion";
    GameManager.TutorialsScreen.SetActive(true);
    finMovementBool = true;
    tutorialStarted = true;
    //GameManager.tutorialsVideoPlayer.clip = GameManager.finMovementClip;
    GameManager.movementAnimatorModule.SetBool("Fin", true);
    GameManager.finMovementClip.SetActive(true);
  }

  void completionOfFinMovement()
  {
    GameManager.movementAnimatorModule.SetBool("Fin", false);
    GameManager.completedToggle.isOn = true;
    StartCoroutine(ChangeBool("fin"));
    GameManager.finMovementClip.SetActive(false);

  }

  void closeFistActivationFlow()
  {
    GameManager.TaskText.text = "Selecting target\nMotion - Close Fist";
    GameManager.TutorialsScreen.SetActive(true);
    closeFistBool = true;
    //GameManager.tutorialsVideoPlayer.clip = GameManager.closeFistClip;
    GameManager.movementAnimatorModule.SetBool("CloseFist", true);
    //GameManager.movementAnimatorModule.SetTrigger("Close");
    //GameManager.movementAnimatorModule.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(GameManager.closeFistAnim);
    //GameManager.tutorialsVideoPlayer.Play();
    GameManager.closeFistClip.SetActive(true);
  }

  void completionOfcloseFist()
  {
    if (!GameManager.completedToggle.isOn)
      GameManager.completedToggle.isOn = true;
    StartCoroutine(ChangeBool("close"));
    GameManager.closeFistClip.SetActive(false);
    tutorialStarted2 = true;
    GameManager.movementAnimatorModule.SetBool("CloseFist", false);
    GameManager.movementAnimatorModule.SetTrigger("Close");
  }

  void rotateFistActivationFlow()
  {
    GameManager.TaskText.text = "Bring the mushroom\nMotion - Close and Rotate your arm";
    GameManager.TutorialsScreen.SetActive(true);
    rotateFistBool = true;
    // GameManager.tutorialsVideoPlayer.clip = GameManager.rotateFistClip;

    GameManager.movementAnimatorModule.SetBool("Rotate", true);
    //GameManager.movementAnimatorModule.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(GameManager.rotateFistAnim);
    //GameManager.tutorialsVideoPlayer.Play();
    GameManager.rotateFistClip.SetActive(true);
  }

  void completionOfrotateFist()
  {

    GameManager.completedToggle.isOn = true;
    StartCoroutine(ChangeBool("rotate"));
    GameManager.rotateFistClip.SetActive(false);
    GameManager.movementAnimatorModule.SetBool("Rotate", false);
  }

  void pickAndDropActivationFlow()
  {
    GameManager.TutorialsScreen.SetActive(true);
    pickingDropBool = true;
    //GameManager.tutorialsVideoPlayer.clip = GameManager.pickAndGrabClip;
    //GameManager.movementAnimatorModule.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(GameManager.pickAndGrabAnim);

  }

  void completionOfpickAndDrop()
  {
    GameManager.completedToggle.isOn = true;
    StartCoroutine(ChangeBool("grab"));
  }

  void leavesHandMotionsActivationFlow()
  {
    GameManager.TaskText.text = "Removing leaves\nMotion - Lock and use Fin on other hand";
    GameManager.TutorialsScreen.SetActive(true);
    leavesMovementBool = true;
    tutorialStarted = true;
    //GameManager.tutorialsVideoPlayer.clip = GameManager.leavesMotionClip;
    GameManager.movementAnimatorModule.SetBool("Leaves", true);
    //GameManager.movementAnimatorModule.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(GameManager.leavesMotionAnim);
    //GameManager.tutorialsVideoPlayer.Play();
    GameManager.leavesMotionClip.SetActive(true);
  }

  void completionOfleavesHandMotions()
  {
    GameManager.completedToggle.isOn = true;
    StartCoroutine(ChangeBool("leaves"));
    GameManager.leavesMotionClip.SetActive(false);
    GameManager.movementAnimatorModule.SetBool("Leaves", false);
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
