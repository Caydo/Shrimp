using System.Collections.Generic;
using UnityEngine;

namespace shrimp.platform
{
  public class Platform : MonoBehaviour
  {
    [SerializeField] PlatformCollider[] colliders = null;
    public Dictionary<PlatformCollider.ColliderSide, BoxCollider2D> ColliderLookup
    {
      get;
      private set;
    }

    void Start()
    {
      ColliderLookup = new Dictionary<PlatformCollider.ColliderSide, BoxCollider2D>();
      foreach(var platformCollider in colliders)
      {
        ColliderLookup.Add(platformCollider.Side, platformCollider.Collider);
      }
    }
  }
}