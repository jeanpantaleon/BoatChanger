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
            new Harmony("com.dredge.moredredge").PatchAll();
        }
	}
}
