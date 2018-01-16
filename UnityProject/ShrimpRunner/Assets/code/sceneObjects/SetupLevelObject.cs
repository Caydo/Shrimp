using shrimp.input;
using shrimp.sceneObjects;
using UnityEngine;

public class SetupLevelObject : MonoBehaviour
{
  [SerializeField] SpawnLevelOnTrigger spawnOnTrigger = null;
  [SerializeField] DespawnOnBecameInvisible despawnOnInvisible = null;
  [SerializeField] EnterNextLevelOnInteract enterNextLevelInteract = null;

  public void Setup(LevelSpawner spawner, SpawnNextPlatformerLevel nextPlatformLevelSpawner, HandlePlayerInput playerInput)
  {
    if (nextPlatformLevelSpawner != null)
    {
      enterNextLevelInteract.Setup(nextPlatformLevelSpawner);
    }
    else
    {
      spawnOnTrigger.Spawner = spawner;
      despawnOnInvisible.Setup(spawner, playerInput);
    }

    foreach (var interactableItem in transform.GetComponentsInChildren<InteractableItem>())
    {
      interactableItem.Setup(playerInput, spawner);
      interactableItem.ResetItem();
    }
  }
}
