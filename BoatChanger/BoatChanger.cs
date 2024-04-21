using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Winch.Core;

namespace BoatChanger
{
	public class BoatChanger : MonoBehaviour
	{
		[NonSerialized]
		public int HullTier;

		public static BoatChanger Instance { get; private set; }
		public void Awake()
        {
			Instance = this;

			GameManager.Instance.OnGameStarted += GameStarted; // Add handler to OnGameStarted event

            WinchCore.Log.Debug($"{nameof(BoatChanger)} has loaded!");
        }

		// Update function
        public void Update()
		{
			if (Instance == null) return;

            if (GameManager.Instance == null || GameManager.Instance.DataLoader == null || GameManager.Instance.WorldEventManager == null)
                return;

            Mathf.Min(GameManager.Instance.Player._allBoatModelProxies.Count, HullTier);


            if (Input.GetKeyDown(KeyCode.PageUp) && HullTier < 4) // If the key pressed is PageUp (to increase hull tier)
			{
                HullTier++;

				GameManager.Instance.Player.AdjustHullToTier(Mathf.Min(GameManager.Instance.Player._allBoatModelProxies.Count, HullTier)); // Setting the wanted hull tier (operation with min to assure we are not rendering a hull tier too high)

				GameManager.Instance.UI.ShowNotification(NotificationType.ITEM_ADDED, "boatchanger.tier.up"); // Showing notification

                GameManager.Instance._saveManager.activeSaveData.SetIntVariable("custom.hulltier", HullTier); // Saving the hull tier in the variable

                WinchCore.Log.Info("Visual tier update (u). Now: " + HullTier);
            } else if (Input.GetKeyDown(KeyCode.PageDown) && HullTier > 1) // If the key is PageDown (to decrease hull tier)
            {
                HullTier--;

                GameManager.Instance.Player.AdjustHullToTier(Mathf.Max(1, HullTier)); // Setting the wanted hull tier (operation with min to assure we are not rendering a hull tier too low)

                GameManager.Instance.UI.ShowNotification(NotificationType.ITEM_REMOVED, "boatchanger.tier.down"); // Showing notification

                GameManager.Instance._saveManager.activeSaveData.SetIntVariable("custom.hulltier", HullTier); // Saving the hull tier in the variable

                WinchCore.Log.Info("Visual tier update (d). Now: " + HullTier);
            }

        }

		// GameStarted handler
		public void GameStarted()
		{
			if(GameManager.Instance._saveManager.activeSaveData.GetIntVariable("custom.hulltier", 10) == 10) // If no hulltier is saved in the config
			{
				GameManager.Instance._saveManager.activeSaveData.SetIntVariable("custom.hulltier", GameManager.Instance.SaveData.HullTier); // Setting the variable to the current hulltier

                WinchCore.Log.Info("No custom hull tier saved, creating one.");
            } 

			HullTier = GameManager.Instance._saveManager.activeSaveData.GetIntVariable("custom.hulltier", GameManager.Instance.SaveData.HullTier); // Setting the global variable to the good value (after operation

			GameManager.Instance.Player.AdjustHullToTier(HullTier);
        }
    }
}
