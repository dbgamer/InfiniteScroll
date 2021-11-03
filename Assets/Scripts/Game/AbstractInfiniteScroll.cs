using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractInfiniteScroll : MonoBehaviour
{
  [SerializeField, Tooltip( "Required: a reference to the scroll rect intended to be infinitely scrolling" )]
  protected ScrollRect scrollRect;

  [SerializeField, Tooltip( "Required: Random data generator" )]
  protected RandomData randomData;

  [SerializeField, Tooltip( "Requried: Prefab that will be generated as item for the list" )]
  private InfiniteScrollElement newElement;

  [SerializeField, Range( -0.5f, 0.5f ), Tooltip( "Amount of space on top and bottom past the bounds of the viewport before shifting elements." )]
  private float rangeBeforeShift;

  /// <summary>
  /// `true` if the size of the content is smaller than the viewport and range before shift.
  /// </summary>
  public bool ShouldGenerateElement => scrollRect.content.rect.height < scrollRect.viewport.rect.height * ( 1 + rangeBeforeShift * 2 );
  
  /// <summary>
  /// `true` if the amount of space at the top is small enough that we should add a new element above.
  /// </summary>
  public bool WithinRangeTop => scrollRect.verticalNormalizedPosition > 1 - rangeBeforeShift;
  
  /// <summary>
  /// `true` if the amount of space at the bottom is small enough that we should add a new element below.
  /// </summary>
  public bool WithinRangeBottom => scrollRect.verticalNormalizedPosition < rangeBeforeShift;

  private void OnEnable() => scrollRect.onValueChanged.AddListener( OnScrollMoved );

  private void OnDisable() => scrollRect.onValueChanged.RemoveListener( OnScrollMoved );

  protected virtual (InfiniteScrollElement, ElementData) GenerateNewElement()
  {
    var element = Instantiate( newElement, scrollRect.content.transform );
    var data = randomData.Get();
    element.Setup( data );
    return (element, data);
  } 

  private void OnScrollMoved( Vector2 newPos ) => Evaluate();

  protected abstract void Evaluate();

  private void OnDrawGizmosSelected()
  {
    if ( scrollRect == null )
    {
      return;
    }

    var corners = new Vector3[4];
    scrollRect.viewport.GetWorldCorners( corners );
    var delta = ( corners[ 1 ].y - corners[ 0 ].y ) * rangeBeforeShift;

    // draw bottom
    Gizmos.DrawLine( corners[ 0 ], corners[ 0 ] + Vector3.down * delta );
    Gizmos.DrawLine( corners[ 3 ], corners[ 3 ] + Vector3.down * delta );
    Gizmos.DrawLine( corners[ 0 ] + Vector3.down * delta, corners[ 3 ] + Vector3.down * delta );

    // draw top
    Gizmos.DrawLine( corners[ 1 ], corners[ 1 ] + Vector3.up * delta );
    Gizmos.DrawLine( corners[ 2 ], corners[ 2 ] + Vector3.up * delta );
    Gizmos.DrawLine( corners[ 1 ] + Vector3.up * delta, corners[ 2 ] + Vector3.up * delta );
  }
}
