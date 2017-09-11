using UnityEngine;

namespace shrimp.sceneObjects
{
  public class EnterNextLevelOnInteract : InteractableItem
  {
    [SerializeField] Transform levelToSpawn = null;
    SpawnNextPlatformerLevel nextLevelSpawner;

    public void Setup(SpawnNextPlatformerLevel nextLevelSpawner)
    {
      this.nextLevelSpawner = nextLevelSpawner;
    }

    public override void Interact()
    {
      nextLevelSpawner.SpawnLevel(levelToSpawn);
    }
  }
}