using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;
public class WinningScreenHandControllerScript : MonoBehaviour
{
  GameManagerScript GameManager;
  int currentActiveButton = 1;
  private Hand _hand;
  Finger _index;
  Finger _middle;
  Finger _ring;
  Finger _pinky;
  public float finParam = 0.08f;

  public float grabParam = 0.8f;
  public bool isLeftHand;

  public bool isRightHand;

  bool leftHand;
  bool rightHand;
  private bool pointedLeft;
  bool pointedCenter = true;
  public float cooldown = 2f;
  private float lastMovedTarget = 3f;
  private float startPosfinMovementCheckValue;

  Vector2 targetPos;

  float timer = 0.0f;

  void Start()
  {
    GameManager = GameManagerScript.instance;
    _hand = this.GetComponent<HandModelBase>().GetLeapHand();
    _index = _hand.GetIndex();
    _middle = _hand.GetMiddle();
    _ring = _hand.GetRing();
    _pinky = _hand.GetPinky();
    startPosfinMovementCheckValue = _hand.Direction.x;
    leftHand = _hand.IsLeft && isLeftHand && GameManager.isLeft;
    rightHand = !_hand.IsLeft && isRightHand && !GameManager.isLeft;
    foreach (ButtonPosition button in GameManager.ButtonsAndTheirPositions)
    {
      if (button.id == currentActiveButton)
        button.button.GetComponent<Button>().image.color = new Color(255f, 255f, 255f, 1f);
      else
        button.button.GetComponent<Button>().image.color = new Color(255f, 255f, 255f, 0.5f);
    }
  }


  void Update()
  {
    //change the functionality for left or right hand as important one
    leftHand = _hand.IsLeft && isLeftHand && GameManager.isLeft;
    rightHand = !_hand.IsLeft && isRightHand && !GameManager.isLeft;
    if (timer < 1.0f)
    {
      timer += Time.deltaTime * 2;
      GameManager.AllButtons.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GameManager.AllButtons.GetComponent<RectTransform>().anchoredPosition, targetPos, timer);
    }
    else
    {
      GameManager.ButtonsAndTheirPositions[currentActiveButton].button.GetComponent<Button>().image.color = new Color(255f, 255f, 255f, 1f); ;
    }
    //GameManager.AllButtons.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GameManager.AllButtons.GetComponent<RectTransform>().anchoredPosition, targetPos, Time.deltaTime * 0.1f);
  }

  private void FixedUpdate()
  {
    //to determine the distance between last checkposition and current hands position
    float disdirect = Mathf.Abs(startPosfinMovementCheckValue - _hand.Direction.x);

    //variable set for Fin movement for traversing target to left or right
    //given in start so that we can reduce a certain Time lag
    if (_hand.Direction.x < -0.4f)
    {
      pointedLeft = true;
      pointedCenter = false;
    }
    else if (_hand.Direction.x > 0.3f)
    {
      pointedLeft = false;
      pointedCenter = false;
    }
    else
    {
      pointedCenter = true;
    }
    if (lastMovedTarget > cooldown)
    {
      if (_hand.PalmNormal.y > -0.5 && _hand.PalmNormal.y < 0.5f)
      {
        if (_middle.IsExtended && _ring.IsExtended && _pinky.IsExtended && _index.IsExtended)
        {
          if (disdirect > finParam) // checking if distance is less than required distance.
          {

            if (!pointedCenter)
            {
              if (pointedLeft)
              {
                Debug.Log("Fin movement done towards left");
                if (currentActiveButton == 0)
                {
                  currentActiveButton = 3;
                }
                else
                {
                  currentActiveButton = (currentActiveButton - 1) % 4;
                }
              }
              else
              {
                Debug.Log("Fin movement done towards right");
                currentActiveButton = (currentActiveButton + 1) % 4;
              }
              lastMovedTarget = 0;
              lerpTheButton();
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

    //Locking of the mushroom
    if (!_index.IsExtended && !_middle.IsExtended && !_ring.IsExtended && !_pinky.IsExtended && _hand.GrabStrength > grabParam)
    {
      Debug.Log("Selection");
      executeButtonsFunctionality();
    }
    //fin movement calculation variable
    startPosfinMovementCheckValue = _hand.Direction.x;
  }

  void executeButtonsFunctionality()
  {
    switch (currentActiveButton)
    {
      case 0:
        {
          GameManager.menuOnClick();
        }
        break;
      case 1:
        {
          GameManager.nextLevelOnClick();
        }
        break;
      case 2:
        {
          GameManager.retryLevelOnClick();
        }
        break;
      case 3:
        {
          GameManager.quitOnClick();
        }
        break;

    }
  }

  void lerpTheButton()
  {
    foreach (ButtonPosition button in GameManager.ButtonsAndTheirPositions)
    {
      button.button.GetComponent<Button>().image.color = new Color(255f, 255f, 255f, 0.5f);
    }

    targetPos = GameManager.ButtonsAndTheirPositions[currentActiveButton].postion;
    if (targetPos == null)
    {
      Debug.LogWarning(currentActiveButton + "Button index not found");
      return;
    }
    Debug.Log("Number:" + currentActiveButton + "  Position:" + targetPos + " TIme:" + Time.deltaTime);
    //GameManager.AllButtons.transform.position = Vector3.Lerp(GameManager.AllButtons.transform.position, targetPos, 1f * Time.deltaTime);
    timer = 0.0f;
  }

}