using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// showcase the current fps averaged over provided time
/// </summary>
public class FpsUI : MonoBehaviour
{
  [SerializeField, Tooltip( "Component used to set the text." )]
  private Text fpsText;
  [SerializeField, Tooltip( "Amount of time to calculate average fps and update text" )]
  private float timeBeforeUpdate = 1f;
  
  /// <summary>
  /// frame counter used between interval
  /// </summary>
  private int frameCount;

  /// <summary>
  /// current elapsed time used to check the current interval
  /// </summary>
  private float elapsedTime;

  private void Update()
  {
    frameCount++;
    elapsedTime += Time.deltaTime;

    if ( elapsedTime > timeBeforeUpdate )
    {
      var frameRate = System.Math.Round( frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero );
      fpsText.text = string.Format( "FPS: {0}", frameRate );
      
      frameCount = 0;
      elapsedTime -= timeBeforeUpdate;
    }
  }
}