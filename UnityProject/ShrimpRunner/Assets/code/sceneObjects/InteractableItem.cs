using shrimp.input;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public abstract class InteractableItem : MonoBehaviour
  {
    public abstract void Interact();
    public virtual void ResetItem(){}
    protected HandlePlayerInput playerInput;
    protected LevelSpawner spawner;

    public void Setup(HandlePlayerInput playerInput, LevelSpawner spawner)
    {
      this.playerInput = playerInput;
      this.spawner = spawner;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
      triggerEntered(collision);
    }

    protected virtual void triggerEntered(Collider2D collision)
    {
      if(playerInput != null && collision.gameObject == playerInput.gameObject)
      {
        playerInput.CurrentInteractable = this;
      }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
      triggerExited(collision);
    }

    protected virtual void triggerExited(Collider2D collision)
    {
      if(playerInput != null && collision.gameObject == playerInput.gameObject)
      {
        playerInput.CurrentInteractable = null;
      }
    }
  }
}