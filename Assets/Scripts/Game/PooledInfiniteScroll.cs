using System.Collections.Generic;
using UnityEngine;

public class PooledInfiniteScroll : AbstractInfiniteScroll
{
  /// <summary>
  /// list of generated game object views for element data.
  /// using this to ensure I don't have to make a ton of GetComponent calls.
  /// </summary>
  private readonly List<InfiniteScrollElement> elementList = new List<InfiniteScrollElement>();
  
  /// <summary>
  /// ordered list of all known element data
  /// </summary>
  private readonly List<ElementData> dataList = new List<ElementData>();
  
  /// <summary>
  /// ordered list of heights.
  /// This is helpful to know because we don't know the height of top elements when we need to place them back on top
  /// (note: in this example, the origin is at the top, so you need to know how much to shift the content of the scroll rect)
  /// </summary>
  private readonly List<float> heightList = new List<float>();
  
  /// <summary>
  /// current index of the top most view
  /// </summary>
  private int index = 0;

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

    if ( WithinRangeTop )
    {
      AddElementToTop();
    }
    if ( WithinRangeBottom )
    {
      AddElementToBottom();
    }
  }

  /// <summary>
  /// when generating new elements, add to our local array cache
  /// </summary>
  /// <returns></returns>
  protected override (InfiniteScrollElement, ElementData) GenerateNewElement()
  {
    var el = base.GenerateNewElement();
    elementList.Add( el.Item1 );
    dataList.Add( el.Item2 );
    heightList.Add( 0 );
    return el;
  }

  /// <summary>
  /// 1. if we're past our data limit, generate a new data (and height element for use later)
  /// 2. move the current top-most view to the bottom.
  /// 3. cache the height and increase the index.
  /// 4. setup the now-bottom view with the new data
  /// 5. shift the scroll rect down to compensate for the move
  /// ^^^ Important note about #5:
  ///     There's a little more "Unity" stuff to do here with internally offsetting the drag position of the scroll view
  ///     or making a spacer object to compensate. I'm acknowledging but ignoring that for this tech test.
  /// </summary>
  protected void AddElementToBottom()
  {
    int bottomIndex = index + elementList.Count;
    if ( bottomIndex >= dataList.Count )
    {
      // infinite data!
      dataList.Add( randomData.Get() );
      heightList.Add( 0 );
    }

    InfiniteScrollElement toBottom = elementList[ 0 ];
    float height = ( toBottom.transform as RectTransform ).sizeDelta.y;
    MoveElement( from: 0, to: elementList.Count - 1 );
    heightList[ index++ ] = height;
    toBottom.Setup( dataList[ bottomIndex - 1 ] );

    // this part needs unity specific handling
    scrollRect.content.position -= Vector3.up * height;
  }

  /// <summary>
  /// 1. if we're at the top, no more data. return!
  /// 2. move bottom most element to the top.
  /// 3. decrease index and setup the now-top view with the new data.
  /// 4. shift the scroll rect up to compensate for the move.
  /// ^^^ See important note from `AddElementToBottom` summary.
  /// </summary>
  protected void AddElementToTop()
  {
    if ( index == 0 )
    {
      return;
    }

    InfiniteScrollElement toTop = elementList[ elementList.Count - 1 ];
    MoveElement( from: elementList.Count - 1, to: 0 );
    toTop.Setup( dataList[ --index ] );

    // this part needs unity specific handling
    scrollRect.content.position += Vector3.up * heightList[ index ];
  }

  /// <summary>
  /// moves a game object view from one index to another.
  /// this method was created to ensure the element list
  /// stays correct and up to date.
  /// </summary>
  private void MoveElement( int from, int to )
  {
    if ( to > from )
    {
      to--;
    }

    var element = elementList[ from ];
    elementList.RemoveAt( from );
    elementList.Insert( to, element );
    element.transform.SetSiblingIndex( to );
  }
}