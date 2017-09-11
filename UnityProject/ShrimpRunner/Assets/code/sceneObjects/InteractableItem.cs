using shrimp.input;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public abstract class InteractableItem : MonoBehaviour
  {
    public HandlePlayerInput PlayerInput = null;
    public abstract void Interact();
    public virtual void ResetItem(){}

    void OnTriggerEnter2D(Collider2D collision)
    {
      triggerEntered(collision);
    }

    protected virtual void triggerEntered(Collider2D collision)
    {
      if(PlayerInput != null && collision.gameObject == PlayerInput.gameObject)
      {
        PlayerInput.CurrentInteractable = this;
      }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
      triggerExited(collision);
    }

    protected virtual void triggerExited(Collider2D collision)
    {
      if(PlayerInput != null && collision.gameObject == PlayerInput.gameObject)
      {
        PlayerInput.CurrentInteractable = null;
      }
    }
  }
}