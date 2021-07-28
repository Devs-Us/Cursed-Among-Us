using System;
using UnityEngine;
using HarmonyLib;

namespace CursedAmongUs.Source.Tasks
{
	class CursedWeapons
	{
		[HarmonyPatch(typeof(WeaponsMinigame))]
		static class WeaponsMinigamePatch
		{
			[HarmonyPatch(nameof(WeaponsMinigame.Begin))]
			[HarmonyPrefix]
			static void BeginPrefix(WeaponsMinigame __instance)
			{
				GameObject cursor = new GameObject("cursor");
				cursor.transform.SetParent(__instance.transform);
				cursor.layer = 4;
				CircleCollider2D circleCollider2D = cursor.AddComponent<CircleCollider2D>();
				circleCollider2D.radius = 0.4f;
				WeaponsCustom weaponsCustom = cursor.AddComponent<WeaponsCustom>();
				weaponsCustom.weaponsMinigame = __instance;
			}
		}

		[HarmonyPatch(typeof(Asteroid))]
		static class AsteroidPatch
		{
			[HarmonyPatch(nameof(Asteroid.Reset))]
			[HarmonyPostfix]
			static void AwakePostfix(Asteroid __instance)
			{
				if (__instance.gameObject.GetComponent<Rigidbody2D>()) return;
				Rigidbody2D rigidbody2D = __instance.gameObject.AddComponent<Rigidbody2D>();
				rigidbody2D.gravityScale = 0f;

			}
		}

		internal class WeaponsCustom : MonoBehaviour
		{
			public WeaponsCustom(IntPtr ptr) : base(ptr) { }
			public WeaponsMinigame weaponsMinigame;

			public void Update()
			{
				if (weaponsMinigame) transform.position = weaponsMinigame.myController.HoverPosition;
			}
		}
	}
}
