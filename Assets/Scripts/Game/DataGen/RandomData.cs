using UnityEngine;

/// <summary>
/// Generates random text and random sigil from serialized information
/// </summary>
[CreateAssetMenu( fileName = nameof( RandomData ), menuName = "Tools/" + nameof( RandomData ) )]
public class RandomData : ScriptableObject
{
  /// <summary>
  /// character responsible for seperating each paragraph
  /// </summary>
  private static readonly char[] seperators = new char[] { '\n' };

  [SerializeField, Tooltip( "The text document containing variable length text seperated by line break." )]
  private TextAsset asset;

  [SerializeField, Tooltip( "List of complimentary colors used to color sigil" )]
  private Color[] colors;
  
  [SerializeField, Tooltip( "List of shapes to contain the sigil" )]
  private Sprite[] shapes;
  
  [SerializeField, Tooltip( "List of fills that are contained within the sigil" )]
  private Sprite[] fills;

  [SerializeField, Tooltip( "List of graphics that are within the sigil" )]
  private Sprite[] graphics;

  /// <summary>
  /// Every paragraph currently found in the asset
  /// </summary>
  [System.NonSerialized]
  private string[] collection = null;

  /// <summary>
  /// Generates a random sigil
  /// </summary>
  /// <returns>All info to create sigil. Sprites will be null if none is provided.</returns>
  public ElementData Get()
  {
    if ( collection == null )
    {
      FillValues();
    }

    if ( collection.Length == 0 )
    {
      throw new System.IndexOutOfRangeException( "attempting to generate paragraphs but none exists" );
    }

    var color = colors[ Random.Range( 0, colors.Length ) ];
    var complimentaryColor = new Color( 255 - color.r, 255 - color.g, 255 - color.b, color.a );
    return new ElementData()
    {
      text = collection[ UnityEngine.Random.Range( 0, collection.Length ) ].Trim(),
      primaryColor = color,
      secondaryColor = complimentaryColor,
      shape = shapes?.Length > 0
        ? shapes[ Random.Range( 0, shapes.Length ) ]
        : null,
      fill = fills?.Length > 0
        ? fills[ Random.Range( 0, fills.Length ) ]
        : null,
      graphic = graphics?.Length > 0
        ? graphics[ Random.Range( 0, graphics.Length ) ]
        : null
    };
  }

  /// <summary>
  /// Loads all values from text asset into collection.
  /// Throws if no asset is available.
  /// </summary>
  private void FillValues()
  {
    if ( asset?.text == null )
    {
      throw new System.NullReferenceException( "attempting to generate paragraphs but no asset found" );
    }
    collection = asset.text.Split( seperators, System.StringSplitOptions.RemoveEmptyEntries );
  }

}
