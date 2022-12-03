using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

///<summary>
///Loading functionalities are written here
///</summary>
public class LoadingFillScript : MonoBehaviour
{

  public GameObject LoadingScene;
  public Image LoadFill;
  public Slider SliderLoadFill;
  public TextMeshProUGUI PercentText;
  public void showLoadScreen(int sceneId)
  {
    StartCoroutine(LoadAsyncScene(sceneId));
  }

  IEnumerator LoadAsyncScene(int sceneId)
  {
    AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneId);
    LoadingScene.SetActive(true);
    while (!loadOperation.isDone)
    {
      float loadingValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
      LoadFill.fillAmount = loadingValue;
      SliderLoadFill.value = loadingValue;
      PercentText.text = "" + loadingValue * 100f;
      yield return null;
    }
  }
}
