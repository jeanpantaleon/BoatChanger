using HarmonyLib;
using UnityEngine;

namespace BoatChanger
{
	public class Loader
	{
		/// <summary>
		/// This method is run by Winch to initialize your mod
		/// </summary>
		public static void Initialize()
		{
			var gameObject = new GameObject(nameof(BoatChanger));
			gameObject.AddComponent<BoatChanger>();
            GameObject.DontDestroyOnLoad(gameObject);
			GameManager.Instance._prodGameConfigData.hourDurationInSeconds = 24;
            new Harmony("com.dredge.moredredge").PatchAll();
        }
	}
}