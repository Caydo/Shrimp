using shrimp.player;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public class ChangeScoreOnTrigger : InteractableSprite
  {
    [SerializeField] int pointValue = 5;

    protected override void triggerEntered(Collider2D collision)
    {
      var pointTracker = collision.GetComponent<TrackPlayerScore>();
      if(pointTracker != null)
      {
        pointTracker.Score += pointValue;
      }

      Interact();
    }
  }
}