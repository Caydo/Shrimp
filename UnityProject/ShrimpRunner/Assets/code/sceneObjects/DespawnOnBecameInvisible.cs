using shrimp.input;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public class DespawnOnBecameInvisible : MonoBehaviour
  {
    [SerializeField] Transform levelRoot = null;
    LevelSpawner spawner;
    HandlePlayerInput playerInput;

    public void Setup(LevelSpawner spawner, HandlePlayerInput playerInput)
    {
      this.spawner = spawner;
      this.playerInput = playerInput;
    }

    void OnBecameInvisible()
    {
      if(!playerInput.Dead)
      {
        spawner.DespawnLevel(levelRoot);
      }
    }
  }
}