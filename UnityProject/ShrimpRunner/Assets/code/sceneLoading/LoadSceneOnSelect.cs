using UnityEngine;
using UnityEngine.SceneManagement;

namespace shrimp.scenes
{
  public class LoadSceneOnSelect : MonoBehaviour
  {
    [SerializeField] string sceneToLoad = "characterSelect";
    public void LoadScene()
    {
      SceneManager.LoadScene(sceneToLoad);
    }
  }
}