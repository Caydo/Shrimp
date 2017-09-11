using UnityEngine;

namespace shrimp.sceneObjects
{
  public class SetCurrentLevelOnTrigger : InteractableItem
  {
    [SerializeField] Transform levelRoot = null;
    protected override void triggerEntered(Collider2D collision)
    {
      if(playerInput != null && collision.gameObject == playerInput.gameObject)
      {
        Interact();
      }
    }

    public override void Interact()
    {
      spawner.CurrentLevel = levelRoot;
    }
  }
}