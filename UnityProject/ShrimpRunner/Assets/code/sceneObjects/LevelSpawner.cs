using PathologicalGames;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public class LevelSpawner : MonoBehaviour
  {
    [SerializeField] GameObject[] levelPrefabs = null;
    [SerializeField] float nonStartingLevelSpawnXPosition = 45; // where to spawn the next level after the starting one, kind of magic number-y but should be doable if all levels have the same width
    [SerializeField] bool isRunnerSpawner = true;
    readonly string poolName = "levels";
    int spawnedLevels = 0;

    public void DespawnLevel(Transform levelToDespawn)
    {
      spawnedLevels--;
      if(PoolManager.Pools[poolName].IsSpawned(levelToDespawn))
      {
        PoolManager.Pools[poolName].Despawn(levelToDespawn);
      }
    }

    public void SpawnLevel()
    {
      spawnedLevels++;
      var levelIndex = Random.Range(0, levelPrefabs.Length - 1);
      var spawnXPosition = nonStartingLevelSpawnXPosition;
      
      var level = levelPrefabs[levelIndex].transform;
      var spawnPosition = new Vector3(spawnXPosition * spawnedLevels, transform.localPosition.y, transform.localPosition.z);
      var spawnedLevel = PoolManager.Pools[poolName].Spawn(level, spawnPosition, Quaternion.identity, transform);

      if(isRunnerSpawner)
      {
        spawnedLevel.GetComponent<SpawnLevelOnTrigger>().Spawner = this;
        spawnedLevel.GetComponentInChildren<DespawnOnBecameInvisible>().Spawner = this;
      }
    }

    public Transform SpawnLevel(Transform levelToSpawn)
    {
      var spawnedLevel = PoolManager.Pools[poolName].Spawn(levelToSpawn, transform);

      if(isRunnerSpawner)
      {
        spawnedLevel.GetComponent<SpawnLevelOnTrigger>().Spawner = this;
        spawnedLevel.GetComponentInChildren<DespawnOnBecameInvisible>().Spawner = this;
      }

      return spawnedLevel;
    }
  }
}