﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace shrimp.scenes
{
  public class LoadSceneOnInput : MonoBehaviour
  {
    [SerializeField] string sceneToLoad = "";
    [SerializeField] CanvasGroup groupToFade = null;
    [SerializeField] float fadeSpeed = 0.01f;
    readonly string inputName = "LeaveScene";
    public bool Leaving = false;
    
    void Update()
    {
      if(!Leaving && Input.GetButtonUp(inputName))
      {
        Leaving = true;
        StartCoroutine(leaveScene());
      }
    }

    IEnumerator leaveScene()
    {
      if(groupToFade != null)
      {
        groupToFade.interactable = false;
        if(groupToFade.alpha == 1)
        {
          while(groupToFade.alpha > 0)
          {
            groupToFade.alpha -= fadeSpeed;
            yield return null;
          }
        }
      }

      SceneManager.LoadScene(sceneToLoad);
    }
  }
}