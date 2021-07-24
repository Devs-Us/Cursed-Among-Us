using System;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source
{
	public class CursedGameData : MonoBehaviour
	{
		public CursedGameData(IntPtr ptr) : base(ptr) { }

		public static Int32 WiresNum = 0;

		public void Start()
		{
		}

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