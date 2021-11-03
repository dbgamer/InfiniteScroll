using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class HiddenInfiniteScroll : AbstractInfiniteScroll
{
  /// <summary>
  /// holding on to a list of canvas groups so we don't have to get component each time
  /// </summary>
  private readonly List<CanvasGroup> canvasGroups = new List<CanvasGroup>();

  /// <summary>
  /// used as a cache for querying element corner information
  /// </summary>
  private readonly Vector3[] elementCorners = new Vector3[ 4 ];

  /// <summary>
  /// used as a cache for querying viewport corner info
  /// </summary>
  private readonly Vector3[] viewportCorners = new Vector3[ 4 ];

  private void Start()
  {
    scrollRect.viewport.GetWorldCorners( viewportCorners );
  }

  private void Update()
  {
    // quick way to generate element when we have plenty of space to do so
    // this could be optimized through a co-routine (waiting frames to avoid rebuilds)
    // or done all at once (at the cost of forced layout rebuilds for calculating sizes)
    if ( ShouldGenerateElement )
    {
      GenerateNewElement();
    }
  }

  protected override void Evaluate()
  {
    // this is handled by our update method
    if ( ShouldGenerateElement )
    {
      return;
    }

    if ( WithinRangeBottom )
    {
      GenerateNewElement();
    }
    
    UpdateVisibility();
  }

  /// <summary>
  /// called to cache the canvas groups
  /// </summary>
  protected override (InfiniteScrollElement, ElementData) GenerateNewElement()
  {
    var tuple = base.GenerateNewElement();
    var gameObj = tuple.Item1.gameObject;
    if ( !gameObj.TryGetComponent( out CanvasGroup canvasGroup ) )
    {
      canvasGroup = gameObj.AddComponent<CanvasGroup>();
    }
    canvasGroups.Add( canvasGroup );
    return tuple;
  }

  private void SetupObject( CanvasGroup canvasGroup, bool activate )
  {
    canvasGroup.alpha = activate ? 1 : 0;
    canvasGroup.interactable = activate;
    canvasGroup.blocksRaycasts = activate;
  }

  /// <summary>
  /// </summary>
  protected void UpdateVisibility()
  {
    Profiler.BeginSample( "update visibility" );
    foreach ( var canvasGroup in canvasGroups )
    {
      Profiler.BeginSample( "corners" );
      var canvasGroupTransform = canvasGroup.transform as RectTransform;
      canvasGroupTransform.GetWorldCorners( elementCorners );
      Profiler.EndSample();

      var contains = elementCorners[ 1 ].y > viewportCorners[ 0 ].y && elementCorners[ 0 ].y < viewportCorners[ 1 ].y;

      Profiler.BeginSample( "setup" );
      SetupObject( canvasGroup, contains );
      Profiler.EndSample();
    }

    Profiler.EndSample();
  }
}