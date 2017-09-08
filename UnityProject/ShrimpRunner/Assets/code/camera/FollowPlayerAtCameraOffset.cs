using UnityEngine;

namespace shrimp.camera
{
  public class FollowPlayerAtCameraOffset : MonoBehaviour
  {
    [SerializeField] Transform playerTransform = null;
    [SerializeField] float offsetX = -4;

    void Update()
    {
      transform.position = new Vector3(playerTransform.position.x - offsetX, transform.position.y, transform.position.z);
    }
  }
}
