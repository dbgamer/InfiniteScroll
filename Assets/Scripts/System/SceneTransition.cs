using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
  public LoadSceneMode loadSceneMode = LoadSceneMode.Single;

  public void ChangeSceneByIndex( int index )
  {
    SceneManager.LoadScene( index, loadSceneMode );
  }

  public void ChangeSceneByName( string scene )
  {
    SceneManager.LoadScene( scene, loadSceneMode );
  }
}
