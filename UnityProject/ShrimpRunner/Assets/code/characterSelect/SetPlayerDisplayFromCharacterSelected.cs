using UnityEngine;

namespace shrimp.characterSelect
{
  public class SetPlayerDisplayFromCharacterSelected : MonoBehaviour
  {
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
          deathParticlesModule.startColor = selectedCharacterColor.CharacterColor;
          break;
        }
      }

      // data object no longer needed so destroy it
      GameObject.Destroy(data.gameObject);
    }
  }
}