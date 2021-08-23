using System;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	internal class CursedWeapons
	{
		[HarmonyPatch(typeof(WeaponsMinigame))]
		private static class WeaponsMinigamePatch
		{
			[HarmonyPatch(nameof(WeaponsMinigame.Begin))]
			[HarmonyPrefix]
			private static void BeginPrefix(WeaponsMinigame __instance)
			{
				GameObject cursor = new("cursor");
				cursor.transform.SetParent(__instance.transform);
				cursor.layer = 4;
				CircleCollider2D circleCollider2D = cursor.AddComponent<CircleCollider2D>();
				circleCollider2D.radius = 0.52f;
				WeaponsCustom weaponsCustom = cursor.AddComponent<WeaponsCustom>();
				weaponsCustom.weaponsMinigame = __instance;
			}
		}

		[HarmonyPatch(typeof(Asteroid))]
		private static class AsteroidPatch
		{
			[HarmonyPatch(nameof(Asteroid.Reset))]
			[HarmonyPostfix]
			private static void AwakePostfix(Asteroid __instance)
			{
				if (__instance.gameObject.GetComponent<Rigidbody2D>()) return;
				Rigidbody2D rigidbody2D = __instance.gameObject.AddComponent<Rigidbody2D>();
				rigidbody2D.gravityScale = 0f;
			}
		}

		internal class WeaponsCustom : MonoBehaviour
		{
			public WeaponsMinigame weaponsMinigame;
			public WeaponsCustom(IntPtr ptr) : base(ptr) { }

			public void Update()
			{
				if (weaponsMinigame) transform.position = weaponsMinigame.myController.HoverPosition;
			}
		}
	}
}