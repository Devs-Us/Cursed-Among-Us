using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	static class LeafPatch
	{
		[HarmonyPatch(typeof(Minigame))]
		static class MinigamePatch
		{
			[HarmonyPatch(nameof(Minigame.Begin), typeof(PlayerTask))]
			[HarmonyPostfix]
			static void BeginPostfix(Minigame __instance, PlayerTask task)
			{
				if (task.name == "CleanO2Filter(Clone)")
				{
					__instance.MyNormTask.taskStep = 0;
					__instance.MyNormTask.MaxStep = 1000;
				}
			}
		}

		[HarmonyPatch(typeof(LeafMinigame))]
		private static class LeafMinigamePatch
		{
			[HarmonyPatch(nameof(LeafMinigame.Begin))]
			[HarmonyPostfix]
			private static void BeginPostfix(LeafMinigame __instance)
			{
				GameObject pointer = new GameObject("Pointer");
				pointer.transform.SetParent(__instance.transform);
				pointer.layer = 4;
				CircleCollider2D collider2D = pointer.AddComponent<CircleCollider2D>();
				collider2D.radius = 1f;
			}

			[HarmonyPatch(nameof(LeafMinigame.FixedUpdate))]
			[HarmonyPostfix]
			private static void FixedUpdatePostfix(LeafMinigame __instance)
			{
				__instance.transform.FindChild("Pointer").position = __instance.myController.HoverPosition;
			}
		}
	}
}