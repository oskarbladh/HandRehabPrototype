using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Makes the leaves object look at the camera[changes the orientation of the object]
///</summary>
public class LeavesLookAt : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    transform.LookAt(Camera.main.transform);
    //Debug.Log("transform:" + transform.rotation);
  }
}
