using UnityEngine;

namespace shrimp.followPlayer
{
  public class FollowPlayerTransform : MonoBehaviour
  {
    [SerializeField] Transform playerTransform = null;
    [SerializeField] float offsetX = 0;

    void Update()
    {
      transform.position = new Vector3(playerTransform.position.x - offsetX, transform.position.y, transform.position.z);
    }
  }
}
