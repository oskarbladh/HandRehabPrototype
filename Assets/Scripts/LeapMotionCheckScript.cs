using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

///<summary>
///This script is used to check whether the leap motion is connected or not
///</summary>
public class LeapMotionCheckScript : MonoBehaviour
{

  public GameObject messageScreen;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    Controller controller = new Controller();
    if (controller.Devices.Count == 0)
    {
      Time.timeScale = 0;
      messageScreen.SetActive(true);
    }
    else
    {
      Time.timeScale = 1;
      Debug.Log("there is a device!!");
      messageScreen.SetActive(false);
    }
  }
}
