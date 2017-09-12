using UnityEngine;

namespace shrimp.sceneObjects
{
  public class PlayerStartPosition : MonoBehaviour
  {
    [SerializeField] Transform playerStartTransform = null;

    public Vector3 StartPosition
    {
      get
      {
        return playerStartTransform.position;
      }
    }
  }
}