using UnityEngine;
using UnityEngine.SceneManagement;

namespace shrimp.characterSelect
{
  public class SetCharacterOnSelect : MonoBehaviour
  {
    [SerializeField] SelectedCharacterData.Character CharacterToSelect = SelectedCharacterData.Character.Foxy;
    [SerializeField] string sceneToLoad = "runner";

    public void SelectCharacter()
    {
      var characterGO = new GameObject();
      var characterData = characterGO.AddComponent<SelectedCharacterData>();
      characterData.SelectedCharacter = CharacterToSelect;
      characterGO.name = "CharacterData";
      characterGO.tag = "Data";
      DontDestroyOnLoad(characterGO);

      SceneManager.LoadScene(sceneToLoad);
    }
  }
}