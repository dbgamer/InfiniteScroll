using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SimpleInfiniteScroll : AbstractInfiniteScroll
{
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
    if ( WithinRangeBottom )
    {
      GenerateNewElement();
    }
  }
}