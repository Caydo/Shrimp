using UnityEngine;
using UnityEngine.UI;

namespace shrimp.player
{
  public class SetTextFromPlayerScore : MonoBehaviour
  {
    [SerializeField] TrackPlayerScore scoreTracker = null;
    [SerializeField] Text scoreLabel = null;
    int cachedScore = -1;

    void Update()
    {
      if(cachedScore != scoreTracker.Score)
      {
        cachedScore = scoreTracker.Score;
        scoreLabel.text = string.Format("Score: {0}", cachedScore);
      }
    }
  }
}