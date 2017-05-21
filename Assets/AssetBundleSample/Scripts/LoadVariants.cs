using UnityEngine;
using System.Collections;
using AssetBundles.Manager;


public class LoadVariants : MonoBehaviour
{
	const string variantSceneAssetBundle = "variants/variant-scene";
	const string variantSceneName = "VariantScene";
	private string[] activeVariants;
	private bool bundlesLoaded;				// used to remove the loading buttons

	void Awake ()
	{
		activeVariants = new string[1];
		bundlesLoaded = false;
	}

	void OnGUI ()
	{
		if (!bundlesLoaded)
		{
			GUILayout.Space (20);
			GUILayout.BeginHorizontal ();
			GUILayout.Space (20);
			GUILayout.BeginVertical ();
			if (GUILayout.Button ("Load SD"))
			{
				activeVariants[0] = "sd";
				bundlesLoaded = true;
				StartCoroutine (BeginExample ());
				BeginExample ();
			}
			GUILayout.Space (5);
			if (GUILayout.Button ("Load HD"))
			{
				activeVariants[0] = "hd";
				bundlesLoaded = true;
				StartCoroutine (BeginExample ());
				Debug.Log ("Loading HD");
			}
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();
		}
	}
	
	// Use this for initialization
	IEnumerator BeginExample ()
	{
		yield return StartCoroutine(Initialize() );
		
		// Set active variants.
		AssetBundleManager.ActiveVariants = activeVariants;
		
		// Load variant level which depends on variants.
		yield return StartCoroutine(InitializeLevelAsync (variantSceneName, true) );
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
		AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(variantSceneAssetBundle, levelName, isAdditive);
		if (request == null)
			yield break;

		yield return StartCoroutine(request);

		// Calculate and display the elapsed time.
		float elapsedTime = Time.realtimeSinceStartup - startTime;
		Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds" );
	}
}
