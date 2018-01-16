using UnityEngine;

namespace shrimp.platform
{
  public class Collidable : MonoBehaviour
  {
    public enum CollidableType
    {
      None = 0,
      Platform = 1,
      Death = 2
    }

    public CollidableType Type = CollidableType.None;

    public enum ContactSide
    {
      Top,
      Bottom,
      Left,
      Right,
      Corner,
      TopRightCorner,
      TopLeftCorner
    }

    float leftSide;
    float rightSide;
    float topSide;
    float bottomSide;
    Vector2 center;

    // spawning/despawning means we'll potentially reuse this and if this moved somehow (e.g. it was reused by the infinite runner spawner) then we need to
    // redo this math
    void OnEnable()
    {
      var bounds = GetComponent<Collider2D>().bounds;
      var halfWidth = (bounds.size.x / 2);
      var halfHeight = (bounds.size.y / 2);

      center = bounds.center;
      leftSide = (center.x - halfWidth);
      rightSide = (center.x + halfWidth);
      topSide = (center.y + halfHeight);
      bottomSide = (center.y - halfHeight);
    }

    public ContactSide GetContactSide(Vector2 contact)
    {
      ContactSide contactSide = ContactSide.Corner;
      if(contact.x > leftSide && contact.x < rightSide && contact.y >= topSide)
      {
        contactSide = ContactSide.Top;
      }
      else if(contact.x > leftSide && contact.x < rightSide && contact.y <= bottomSide)
      {
        contactSide = ContactSide.Bottom;
      }
      else if(contact.x >= leftSide && contact.y < topSide && contact.y > bottomSide)
      {
        contactSide = ContactSide.Left;
      }
      else if(contact.x <= rightSide && contact.y < topSide && contact.y > bottomSide)
      {
        contactSide = ContactSide.Right;
      }
      else if(contact.x >= rightSide && contact.y >= topSide)
      {
        contactSide = ContactSide.TopRightCorner;
      }
      else if(contact.x <= leftSide && contact.y >= topSide)
      {
        contactSide = ContactSide.TopLeftCorner;
      }

      return contactSide;
    }
  }
}
