using UnityEngine;

namespace shrimp.characterSelect
{
  public class SelectedCharacterData : MonoBehaviour
  {
    public enum Character
    {
      Foxy = 0,
      Blacky = 1
    }

    public Character SelectedCharacter;
  }
}