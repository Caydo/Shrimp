﻿using UnityEngine;

namespace shrimp.characterSelect
{
  public class SetPlayerSpriteFromCharacterSelected : MonoBehaviour
  {
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] SelectedCharacterColor[] characterColors = null;
    [SerializeField] ParticleSystem deathParticles = null;

    void Start()
    {
      SelectedCharacterData data = GameObject.FindGameObjectWithTag("Data").GetComponent<SelectedCharacterData>();
      ParticleSystem.MainModule deathParticlesModule = deathParticles.main;

      foreach(var selectedCharacterColor in characterColors)
      {
        if(data.SelectedCharacter == selectedCharacterColor.Character)
        {
          spriteRenderer.color = selectedCharacterColor.CharacterColor;
          deathParticlesModule.startColor = selectedCharacterColor.CharacterColor;
          spriteRenderer.enabled = true;
          break;
        }
      }

      // data object no longer needed so destroy it
      GameObject.Destroy(data.gameObject);
    }
  }
}