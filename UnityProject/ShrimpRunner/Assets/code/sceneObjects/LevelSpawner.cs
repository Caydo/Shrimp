using PathologicalGames;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public class LevelSpawner : MonoBehaviour
  {
    [SerializeField] GameObject[] levelPrefabs = null;
    readonly string poolName = "levels";

    void Start()
    {
      SpawnLevel(0);
    }

    public void DespawnAll()
    {
      PoolManager.Pools[poolName].DespawnAll();
    }

    public void SpawnLevel(int levelIndex = -1)
    {
      if(levelIndex < 0)
      {
        levelIndex = Random.Range(1, levelPrefabs.Length - 1);
      }

      var level = levelPrefabs[levelIndex].transform;
      var position = Vector3.zero;

      PoolManager.Pools[poolName].Spawn(level, level.transform.localPosition, Quaternion.identity, transform);
    }
  }
}