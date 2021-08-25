using CursedAmongUs.Source.Others;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	[HarmonyPatch(typeof(UnlockManifoldsMinigame))]
	internal static class UnlockManifold
	{
		[HarmonyPatch(nameof(UnlockManifoldsMinigame.Begin))]
		[HarmonyPrefix]
		private static void BeginPrefix(UnlockManifoldsMinigame __instance)
		{
			foreach (SpriteRenderer button in __instance.Buttons)
				button.sprite = SpriteHelper.CreateSpriteFromResources("UnlockManifold.png");
		}
	}
}