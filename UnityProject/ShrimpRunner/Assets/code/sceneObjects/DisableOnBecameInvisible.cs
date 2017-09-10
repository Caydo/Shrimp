using UnityEngine;

namespace shrimp.sceneObjects
{
  public class DisableOnBecameInvisible : MonoBehaviour
  {
    void OnBecameInvisible()
    {
      gameObject.SetActive(false);
    }
  }
}