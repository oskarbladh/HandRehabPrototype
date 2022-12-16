using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{

  public void Loadlevel(int number)
  {
    //Menuscreen transition
    SceneManager.LoadScene(number);
  }
  public void quitOnClick()
  {
    //Exit game 
    Application.Quit();
  }
}
