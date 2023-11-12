using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

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
				for (int i = 0; i < 100; i++)
				{
					__instance.SpawnDirt();
				}
				__instance.numberOfDirts = 102;
				__instance.transform.GetChild(5).gameObject.SetActive(false);
				__instance.transform.GetChild(2).gameObject.SetActive(false);
			}

			[HarmonyPatch(nameof(VentCleaningMinigame.CleanUp))]
			[HarmonyPostfix]
			private static void CleanUpPostfix()
			{
			}

			[HarmonyPatch(nameof(VentCleaningMinigame.OpenVent))]
			[HarmonyPostfix]
			private static void OpenVentPostfix(VentCleaningMinigame __instance)
			{
			}
		}

		[HarmonyPatch(typeof(VentCleaningMinigame), nameof(VentCleaningMinigame.Close))]
		private static class CantClose
		{
			public static bool Prefix(VentCleaningMinigame __instance)
			{
				if (__instance.numberOfDirtsCleanedUp >= __instance.numberOfDirts)
				{
					return true;
				}
				return false;
			}
		}

		[HarmonyPatch(typeof(VentDirt))]
		private static class VentDirtPatch
		{
			[HarmonyPatch(nameof(VentDirt.Reset))]
			[HarmonyPostfix]	
			private static void AwakePostfix(VentDirt __instance)
			{
				__instance.GetComponent<BoxCollider2D>().size = new Vector2(0.8f, 0.8f);
			}
		}
	}
}