using PathologicalGames;
using shrimp.input;
using shrimp.scenes;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public class LevelSpawner : MonoBehaviour
  {
    [SerializeField] GameObject[] levelPrefabs = null;
    [SerializeField] float nonStartingLevelSpawnXPosition = 45; // where to spawn the next level in X, kind of magic number-y but should be doable if all levels have the same width
    [SerializeField] bool isRunnerSpawner = true;
    [SerializeField] HandlePlayerInput playerInput = null;
    [SerializeField] SpawnNextPlatformerLevel nextPlatformLevelSpawner = null;
    [SerializeField] LoadSceneOnInput inputSceneLoader = null;

    readonly string poolName = "levels";
    int spawnedLevels = 0;
    Transform lastSpawnedLevel = null;

    void Start()
    {
      SpawnLevel(levelPrefabs[0].transform);
    }

    public void DespawnLevel(Transform levelToDespawn)
    {
      if(!inputSceneLoader.Leaving)
      {
        if(PoolManager.Pools[poolName].IsSpawned(levelToDespawn))
        {
          PoolManager.Pools[poolName].Despawn(levelToDespawn);
        }

        spawnedLevels--;
      }
    }

    public void SpawnRandomLevel()
    {
      var levelIndex = Random.Range(1, levelPrefabs.Length - 1);
      var spawnXPosition = nonStartingLevelSpawnXPosition;
      var level = levelPrefabs[levelIndex].transform;
      var spawnPosition = new Vector3(spawnXPosition * spawnedLevels, transform.localPosition.y, transform.localPosition.z);
      var spawnedLevel = PoolManager.Pools[poolName].Spawn(level, spawnPosition, Quaternion.identity, transform);
      setupLevel(spawnedLevel);
      spawnedLevels++;
    }

    public Transform SpawnLevel(Transform levelToSpawn)
    {
      if(!inputSceneLoader.Leaving)
      {
        var spawnedLevel = PoolManager.Pools[poolName].Spawn(levelToSpawn, transform);
        setupLevel(spawnedLevel);
        spawnedLevels++;
        return spawnedLevel;
      }

      return null;
    }

    public void DespawnCurrentLevel()
    {
      DespawnLevel(lastSpawnedLevel);
    }

    public void ResetLevel()
    {
      if(!inputSceneLoader.Leaving)
      {
        PoolManager.Pools[poolName].DespawnAll();
        spawnedLevels = 0;
        SpawnLevel(lastSpawnedLevel);
      }
    }

    void setupLevel(Transform spawnedLevel)
    {
      if(isRunnerSpawner)
      {
        spawnedLevel.GetComponent<SpawnLevelOnTrigger>().Spawner = this;
        spawnedLevel.GetComponentInChildren<DespawnOnBecameInvisible>().Setup(this, playerInput);
      }
      else
      {
        spawnedLevel.GetComponentInChildren<EnterNextLevelOnInteract>().Setup(nextPlatformLevelSpawner);
      }

      foreach(var interactableItem in spawnedLevel.GetComponentsInChildren<InteractableItem>())
      {
        interactableItem.PlayerInput = playerInput;
        interactableItem.ResetItem();
      }

      lastSpawnedLevel = spawnedLevel;
    }
  }
}