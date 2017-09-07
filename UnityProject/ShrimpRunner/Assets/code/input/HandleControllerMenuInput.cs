using UnityEngine;
using UnityEngine.EventSystems;

namespace shrimp.input
{
  public class HandleControllerMenuInput : MonoBehaviour
  {
    [SerializeField] GameObject[] items = null;
    [SerializeField] EventSystem eventSystem = null;
    readonly string moveRightInputName = "MoveRight";
    readonly string moveLeftInputName = "MoveLeft";
    readonly string moveJoystickAxisName = "MoveJoystick";
    bool moveLeftTriggered = false;
    bool moveRightTriggered = false;
    float cachedJoystick = 0;
    int selectedItemIndex = 0;

    void Update()
    {
      var moveJoystick = Input.GetAxis(moveJoystickAxisName);
      if(cachedJoystick != moveJoystick)
      {
        cachedJoystick = moveJoystick;
        moveLeftTriggered = moveJoystick < 0;
        moveRightTriggered = moveJoystick > 0;
      }

      if(Input.GetButtonDown(moveLeftInputName))
      {
        moveLeftTriggered = true;
        moveRightTriggered = false;
      }

      if(Input.GetButtonDown(moveRightInputName))
      {
        moveRightTriggered = true;
        moveLeftTriggered = false;
      }

      if(moveLeftTriggered)
      {
        selectedItemIndex--;
        if(selectedItemIndex <= 0)
        {
          selectedItemIndex = 0;
        }

        eventSystem.SetSelectedGameObject(items[selectedItemIndex]);
      }

      if(moveRightTriggered)
      {
        selectedItemIndex++;
        if(selectedItemIndex >= items.Length - 1)
        {
          selectedItemIndex = items.Length - 1;
        }

        eventSystem.SetSelectedGameObject(items[selectedItemIndex]);
      }
    }
  }
}