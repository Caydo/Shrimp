using UnityEngine;

namespace shrimp.followPlayer
{
  public class FollowPlayerAtThreshold : MonoBehaviour
  {
    [SerializeField] Transform playerTransform = null;
    [SerializeField] Vector2 differenceThreshold = Vector2.zero;
    [SerializeField] float moveSpeed = 3;
    Vector3 finalDestination = Vector3.zero;
    
    void Update()
    {
      var shouldRepositionX = getDifference(true) > differenceThreshold.x;
      if(shouldRepositionX)
      {
        finalDestination = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, finalDestination, Time.deltaTime * moveSpeed);
      }

      var shouldRepositionY = getDifference(false) > differenceThreshold.y;
      if(shouldRepositionY)
      {
        finalDestination = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, finalDestination, Time.deltaTime * moveSpeed);
      }
    }

    float getDifference(bool isXDifference)
    {
      var playerPosition = (isXDifference) ? playerTransform.position.x : playerTransform.position.y;
      var currentPosition = (isXDifference) ? transform.position.x : transform.position.y;

      return (playerPosition > currentPosition) ? (playerPosition - currentPosition)  : (currentPosition - playerPosition);
    }
  }
}