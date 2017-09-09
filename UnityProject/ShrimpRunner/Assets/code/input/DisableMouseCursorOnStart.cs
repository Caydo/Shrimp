using UnityEngine;

namespace shrimp.input
{
  public class DisableMouseCursorOnStart : MonoBehaviour
  {
    void Start()
    {
      Cursor.visible = false;
    }
  }
}