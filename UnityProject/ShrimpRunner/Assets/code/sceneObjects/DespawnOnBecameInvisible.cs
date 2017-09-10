using UnityEngine;

namespace shrimp.sceneObjects
{
  public class DespawnOnBecameInvisible : MonoBehaviour
  {
    [SerializeField] Transform levelRoot = null;
    [HideInInspector]
    public LevelSpawner Spawner;
    void OnBecameInvisible()
    {
      Spawner.DespawnLevel(levelRoot);
    }
  }
}