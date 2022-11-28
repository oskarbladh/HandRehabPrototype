using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Outer bounds script when the mushroom is thrown it collides with the invisible walls in the scene and 
///gets destroyed when it collides
///</summary>
public class OutOfBoundsScript : MonoBehaviour
{
  GameManagerScript GameManager;

  void Start()
  {
    GameManager = GameManagerScript.instance;
  }
  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.tag == "Mushroom")
    {
      GameManager.currentIndex = GameManager.currentIndex + 1 % GameManager.AllMushrooms.Count;
      GameManager.AllMushrooms.Remove(other.gameObject);
      Destroy(other.gameObject);
    }
  }
}
