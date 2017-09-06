using System;
using UnityEngine;

namespace shrimp.platform
{
  [Serializable]
  public struct PlatformCollider
  {
    public enum ColliderSide
    {
      None = 0,
      Top = 1,
      Left = 2,
      Bottom = 3,
      Right = 4
    }

    public BoxCollider2D Collider;
    public ColliderSide Side;
  }
}