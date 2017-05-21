using UnityEngine;
using System.Collections;
using AssetBundles.Manager;


public class LoadScenes : MonoBehaviour
{
	public string sceneAssetBundle;
	public string sceneName;
	
	// Use this for initialization
	IEnumerator Start ()
	{	
		yield return StartCoroutine(Initialize() );
		
		// Load level.
		yield return StartCoroutine(InitializeLevelAsync (sceneName, true) );
	}

	// Initialize the downloading url and AssetBundleManifest object.
	protected IEnumerator Initialize()
	{
		// Don't destroy this gameObject as we depend on it to run the loading script.
		DontDestroyOnLoad(gameObject);
				
		// Initialize AssetBundleManifest which loads the AssetBundleManifest object.
		var request = AssetBundleManager.Initialize();
		
		if (request != null)
			yield return StartCoroutine(request);
	}

	protected IEnumerator InitializeLevelAsync (string levelName, bool isAdditive)
	{
		// This is simply to get the elapsed time for this phase of AssetLoading.
		float startTime = Time.realtimeSinceStartup;

		// Load level from assetBundle.
		AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(sceneAssetBundle, levelName, isAdditive);
		if (request == null)
			yield break;
		yield return StartCoroutine(request);

		// Calculate and display the elapsed time.
		float elapsedTime = Time.realtimeSinceStartup - startTime;
		Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds" );
	}
}
