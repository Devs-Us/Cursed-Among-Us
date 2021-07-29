using System;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	internal static class CustomCardSwipe
	{
		private static readonly Boolean PrevState = false;

		[HarmonyPatch(typeof(CardSlideGame))]
		private static class CardSlidePatch
		{
			[HarmonyPatch(nameof(CardSlideGame.Begin))]
			[HarmonyPrefix]
			private static void BeginPrefix(CardSlideGame __instance)
			{
				__instance.AcceptedTime = new FloatRange(0.5f, 0.5f);
			}

			[HarmonyPatch(nameof(CardSlideGame.Update))]
			[HarmonyPrefix]
			private static void PutCardBackPrefix(CardSlideGame __instance)
			{
				Boolean CurrentState = __instance.redLight.color == Color.red;
				if (PrevState == CurrentState || !CurrentState) return;
				Int32 randomNumber = UnityEngine.Random.RandomRangeInt(0, 40);
				if (randomNumber == 0) __instance.AcceptedTime = new FloatRange(0.25f, 2f);
				else __instance.AcceptedTime = new FloatRange(0.5f, 0.5f);
			}
		}
	}
}
