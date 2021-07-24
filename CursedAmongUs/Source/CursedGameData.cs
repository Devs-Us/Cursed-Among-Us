using System;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source
{
	public class CursedGameData : MonoBehaviour
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