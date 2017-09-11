using UnityEngine;

namespace shrimp.sceneObjects
{
  public class InteractableSprite : InteractableItem
  {
    [SerializeField] Sprite nonInteractedSprite = null;
    [SerializeField] Sprite interactedSprite = null;
    [SerializeField] SpriteRenderer spriteRenderer = null;

    public override void Interact()
    {
      spriteRenderer.sprite = interactedSprite;
    }

    public override void ResetItem()
    {
      spriteRenderer.sprite = nonInteractedSprite;
    }
  }
}