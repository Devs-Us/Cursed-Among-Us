using System;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Others
{
	internal static class CursedPalette
	{
		[HarmonyPatch(typeof(CursedGameData))]
		private static class PalettePatch
		{
			[HarmonyPatch(nameof(CursedGameData.Start))]
			[HarmonyPostfix]
			private static void StartPostfix()
			{
				for (Int32 i = 0; i < 3; i++)
				{
					Int32 from = UnityEngine.Random.RandomRangeInt(0, Palette.PlayerColors.Length);
					Int32 to = UnityEngine.Random.RandomRangeInt(0, Palette.PlayerColors.Length);
					(Color main, Color shadow, StringNames name) toProperties = (Palette.PlayerColors[to], Palette.ShadowColors[to], Palette.ColorNames[to]);
					Palette.PlayerColors[to] = Palette.PlayerColors[from];
					Palette.ShadowColors[to] = Palette.ShadowColors[from];
					Palette.ColorNames[to] = Palette.ColorNames[from];
					Palette.PlayerColors[from] = toProperties.main;
					Palette.ShadowColors[from] = toProperties.shadow;
					Palette.ColorNames[from] = toProperties.name;
				}
			}
		}
	}
}
