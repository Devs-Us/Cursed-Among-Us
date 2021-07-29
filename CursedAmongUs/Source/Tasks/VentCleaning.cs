using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	internal class CursedVentCleaning
	{
		[HarmonyPatch(typeof(VentCleaningMinigame))]
		private static class VentCleaningMinigamePatch
		{
			[HarmonyPatch(nameof(VentCleaningMinigame.Begin))]
			[HarmonyPostfix]
			private static void BeginPostfix(VentCleaningMinigame __instance)
			{
				__instance.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
				ShowPurchaseScreen();
			}

			[HarmonyPatch(nameof(VentCleaningMinigame.CleanUp))]
			[HarmonyPostfix]
			private static void CleanUpPostfix()
			{
				ShowPurchaseScreen();
			}

			[HarmonyPatch(nameof(VentCleaningMinigame.OpenVent))]
			[HarmonyPostfix]
			private static void OpenVentPostfix(VentCleaningMinigame __instance)
			{
				__instance.transform.localPosition = new Vector3(__instance.transform.localPosition.x, __instance.transform.localPosition.y, 15f);
				ShowPurchaseScreen();
			}

			[HarmonyPatch(nameof(VentCleaningMinigame.Close))]
			[HarmonyPostfix]
			private static void ClosePostfix()
			{
				ShowPurchaseScreen();
			}
		}

		private static void ShowPurchaseScreen()
		{
			MapBehaviour.Instance.gameObject.active = true;
			StoreMenu.Instance.Open();
			StoreMenu.Instance.BuyProduct();
			Transform allInner = StoreMenu.Instance.Scroller.transform.FindChild("Inner");
			List<PurchaseButton> allButtons = new();
			for (Int32 i = 0; i < allInner.childCount; i++)
			{
				PurchaseButton childButton = allInner.GetChild(i).gameObject.GetComponent<PurchaseButton>();
				if (childButton != null) allButtons.Add(childButton);
			}
			Int32 randomNumber = UnityEngine.Random.RandomRangeInt(0, allButtons.Count);
			allButtons[randomNumber].DoPurchase();
			allButtons[randomNumber].SetPurchased();
			StoreMenu.Instance.SetProduct(allButtons[randomNumber]);
			GameObject dialogueBox = GameObject.Find("Main Camera/Hud/GenericDialogue");
			if (dialogueBox != null)
			{
				dialogueBox.active = true;
				dialogueBox.GetComponent<DialogueBox>().target.text = "You're all set. Your purchase was successful.";
			}
			GameObject menuUI = GameObject.Find("Main Camera/Hud/Menu");
			if (menuUI != null) menuUI.active = true;
		}
	}
}
