using System;
using UnityEngine;

namespace shrimp.characterSelect
{
  [Serializable]
  public struct SelectedCharacterColor
  {
    public SelectedCharacterData.Character Character;
    public Color CharacterColor;
  }
}