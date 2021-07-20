using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace CursedAmongUs.Source.Tasks
{
	// IDEA NOT FINALIZED, THIS WILL CHANGE
	[HarmonyPatch(typeof(CardSlideGame))]
	static class CardSlidePatch
	{
		[HarmonyPatch(nameof(CardSlideGame.InsertCard))]
		[HarmonyPrefix]
		static void BeginPrefix(CardSlideGame __instance)
		{
			float acceptedTime = (float)new Random().NextDouble() * 0.5f + 0.25f;
			__instance.AcceptedTime = new FloatRange(acceptedTime - 0.005f, acceptedTime + 0.005f);
		}
	}
}
