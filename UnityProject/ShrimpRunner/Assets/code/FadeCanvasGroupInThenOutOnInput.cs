using shrimp.input;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace shrimp.ui
{
  public class FadeCanvasGroupInThenOutOnInput : MonoBehaviour
  {
    [SerializeField] CanvasGroup groupToFade = null;
    [SerializeField] float alphaIncrementOverTime = 0.01f;
    [SerializeField] string continueInputName = "Continue";
    [SerializeField] HandlePlayerInput playerInput = null;
    [SerializeField] bool externalDismiss = false;
    [SerializeField] GameObject[] itemsToSetActiveBeforeFadeIn = null;

    public string SceneToLoad = "";
    bool dismissed = false;

    IEnumerator Start()
    {
      groupToFade.alpha = 0;

      if(playerInput != null)
      {
        playerInput.AllowInput = false;
      }

      while(groupToFade.alpha < 1)
      {
        groupToFade.alpha += alphaIncrementOverTime;
        yield return null;
      }

      while(!dismissed)
      {
        yield return null;
      }

      if(itemsToSetActiveBeforeFadeIn != null)
      {
        foreach(var item in itemsToSetActiveBeforeFadeIn)
        {
          item.SetActive(true);
        }
      }

      while(groupToFade.alpha > 0)
      {
        groupToFade.alpha -= alphaIncrementOverTime;
        yield return null;
      }

      if(playerInput != null)
      {
        playerInput.AllowInput = true;
      }

      if(!string.IsNullOrEmpty(SceneToLoad))
      {
        SceneManager.LoadScene(SceneToLoad);
      }
    }

    public void Dismiss()
    {
      dismissed = true;
    }

    void Update()
    {
      if(!externalDismiss)
      {
        if(Input.GetButtonUp(continueInputName))
        {
          dismissed = true;
        }
      }
    }
  }
}