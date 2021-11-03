using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrollElement : MonoBehaviour
{
  [SerializeField, Tooltip( "Component used to set the text." )]
  private Text paragraphText;

  [SerializeField, Tooltip( "Component used to set the shape on the sigil" )]
  private Image sigilShape;

  [SerializeField, Tooltip( "Component used to set the fill on the sigil" )]
  private Image sigilFill;

  [SerializeField, Tooltip( "Component used to set the graphic on the sigil" )]
  private Image sigilGraphic;

  public void Setup( ElementData sigilData )
  {
    paragraphText.text = sigilData.text;
    sigilShape.sprite = sigilData.shape;
    sigilShape.color = sigilData.primaryColor;
    sigilFill.sprite = sigilData.fill;
    sigilGraphic.sprite = sigilData.graphic;
    sigilGraphic.color = sigilData.secondaryColor;
  }
}
