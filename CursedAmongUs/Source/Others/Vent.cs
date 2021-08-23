using System;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Others
{
	internal static class CursedVent
	{
		private static Single LastVent;

		public static void Update()
		{
			if (!PlayerControl.LocalPlayer || PlayerControl.LocalPlayer.inVent || LastVent <= 0f) return;
			LastVent -= Time.deltaTime;
		}

		[HarmonyPatch(typeof(Vent))]
		private static class VentPatch
		{
			[HarmonyPatch(nameof(Vent.SetOutline))]
			[HarmonyPostfix]
			private static void SetOutlinePostfix(Vent __instance, Boolean on, Boolean mainTarget)
			{
				if (!on || !mainTarget || PlayerControl.LocalPlayer.inVent || LastVent > 0f) return;
				__instance.Use();
				LastVent = 10f;
			}
		}
	}
}