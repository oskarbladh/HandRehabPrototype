using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///This is used to change the size of the collected mushrooms in the basket as the picked up mushrooms are made children of the basket
///</summary>
public class CollectedMushroomSizeChangeScript : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (transform.gameObject.activeSelf)
    {
      foreach (Transform child in transform)
      {
        child.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      }
    }
  }
}
