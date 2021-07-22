using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	static class CustomCardSwipe
	{
		static bool PrevState = false;

		[HarmonyPatch(typeof(CardSlideGame))]
		static class CardSlidePatch
		{
			[HarmonyPatch(nameof(CardSlideGame.Begin))]
			[HarmonyPrefix]
			static void BeginPrefix(CardSlideGame __instance)
			{	
				__instance.AcceptedTime = new FloatRange(0.5f, 0.5f);
			}

			[HarmonyPatch(nameof(CardSlideGame.Update))]
			[HarmonyPrefix]
			static void PutCardBackPrefix(CardSlideGame __instance)
			{
				bool CurrentState = __instance.redLight.color == Color.red;
				if (PrevState == CurrentState || !CurrentState) return;
				int randomNumber = new System.Random().Next(0, 50);
				if (randomNumber == 0) __instance.AcceptedTime = new FloatRange(0.25f, 2f);
				else __instance.AcceptedTime = new FloatRange(0.5f, 0.5f);
			}
		}
	}
}
