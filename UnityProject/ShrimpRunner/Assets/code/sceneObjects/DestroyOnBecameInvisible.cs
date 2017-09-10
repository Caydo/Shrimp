using UnityEngine;

namespace shrimp.sceneObjects
{
  public class DestroyOnBecameInvisible : MonoBehaviour
  {
    void OnBecameInvisible()
    {
      GameObject.Destroy(gameObject);
    }
  }
}