using System;
using HarmonyLib;

namespace CursedAmongUs.Source
{
	public static class CursedGameData
	{
		public static Int32 WiresNum = 0;

		[HarmonyPatch(typeof(ShipStatus))]
		private static class ShipStatusPatch
		{
			[HarmonyPatch(nameof(ShipStatus.Start))]
			[HarmonyPrefix]
			private static void StartPrefix()
			{
				WiresNum = 0;
			}
		}
	}
}