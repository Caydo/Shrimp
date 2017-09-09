using UnityEngine;
using UnityEngine.SceneManagement;

namespace shrimp.scenes
{
  public class QuitGameOnInput : MonoBehaviour
  {
    [SerializeField] string startSceneToLoad = "startMenu";
    readonly string quitButtonInputName = "Quit";
    
    void Start()
    {
      DontDestroyOnLoad(gameObject);
      SceneManager.LoadScene(startSceneToLoad);
    }

    void Update()
    {
      if(Input.GetButtonUp(quitButtonInputName))
      {
        Application.Quit();
      }
    }
  }
}