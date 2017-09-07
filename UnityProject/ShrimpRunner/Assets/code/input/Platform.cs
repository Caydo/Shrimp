using UnityEngine;

namespace shrimp.platform
{
  public class Platform : MonoBehaviour
  {
    public enum PlatformType
    {
      None = 0,
      Wall = 1,
      Ground = 2
    };

    public PlatformType Type = PlatformType.None;

    public enum ContactSide
    {
      Top,
      Bottom,
      Left,
      Right,
      None
    }

    float leftSide;
    float rightSide;
    Vector2 center;

    void Start()
    {
      var bounds = GetComponent<BoxCollider2D>().bounds;
      var halfWidth = (bounds.size.x / 2);
      center = bounds.center;

      leftSide = (center.x - halfWidth);
      rightSide = (center.x + halfWidth);
    }

    public ContactSide GetContactSide(Vector2 contact)
    {
      ContactSide contactSide = ContactSide.None;
      if(contact.x > leftSide && contact.x < rightSide && contact.y > center.y)
      {
        contactSide = ContactSide.Top;
      }
      else if(contact.x > leftSide && contact.x < rightSide && contact.y < center.y)
      {
        contactSide = ContactSide.Bottom;
      }
      else if(contact.x >= leftSide)
      {
        contactSide = ContactSide.Left;
      }
      else if(contact.x <= rightSide)
      {
        contactSide = ContactSide.Right;
      }

      return contactSide;
    }
  }
}