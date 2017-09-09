using shrimp.ui;
using UnityEngine;

namespace shrimp.characterSelect
{
  public class SetCharacterOnSelect : MonoBehaviour
  {
    [SerializeField] SelectedCharacterData.Character CharacterToSelect = SelectedCharacterData.Character.Foxy;
    [SerializeField] string sceneToLoad = "runner";
    [SerializeField] FadeCanvasGroupInThenOutOnInput fader = null;
    bool processingSelection = false;

    void Start()
    {
      // destroy any leftover data objects in case they weren't actually used in a game mode scene
      var characterDataObjects = GameObject.FindGameObjectsWithTag("Data");
      foreach(var item in characterDataObjects)
      {
        GameObject.Destroy(item);
      }
    }

    public void SelectCharacter()
    {
      var characterGO = new GameObject();
      var characterData = characterGO.AddComponent<SelectedCharacterData>();
      characterData.SelectedCharacter = CharacterToSelect;
      characterGO.name = "CharacterData";
      characterGO.tag = "Data";
      DontDestroyOnLoad(characterGO);

      fader.SceneToLoad = sceneToLoad;
      fader.Dismiss();
    }
  }
}