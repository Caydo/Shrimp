using UnityEngine;

namespace shrimp.camera
{
  public class FollowPlayerAtCameraThreshold : MonoBehaviour
  {
    [SerializeField] Transform playerTransform = null;
    [SerializeField] Vector2 differenceThreshold = Vector2.zero;
    [SerializeField] float cameraMoveSpeed = 3;
    Vector2 differenceVector = Vector2.zero;
    Vector3 finalDestination = Vector3.zero;
    
    void Update()
    {
      var shouldRepositionX = getDifference(true) > differenceThreshold.x;
      if(shouldRepositionX)
      {
        finalDestination = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, finalDestination, Time.deltaTime * cameraMoveSpeed);
      }

      var shouldRepositionY = getDifference(false) > differenceThreshold.y;
      if(shouldRepositionY)
      {
        finalDestination = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, finalDestination, Time.deltaTime * cameraMoveSpeed);
      }
    }

    float getDifference(bool isXDifference)
    {
      var playerPosition = (isXDifference) ? playerTransform.position.x : playerTransform.position.y;
      var cameraPosition = (isXDifference) ? transform.position.x : transform.position.y;

      return (playerPosition > cameraPosition) ? (playerPosition - cameraPosition)  : (cameraPosition - playerPosition);
    }
  }
}