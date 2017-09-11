using shrimp.input;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public class SpawnLevelOnTrigger : MonoBehaviour
  {
    public LevelSpawner Spawner = null;
    void OnTriggerEnter2D(Collider2D collision)
    {
      // a player hit our trigger so create the next level
      if(collision.GetComponent<HandlePlayerInput>() != null)
      {
        Spawner.SpawnRandomLevel();
      }
    }
  }
}