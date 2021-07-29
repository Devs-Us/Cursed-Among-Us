using System;
using System.Linq;
using HarmonyLib;
using Il2CppSystem.Text;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	internal class DivertPower
	{
		private static Boolean IsIntermission = false;

		[HarmonyPatch(typeof(ShipStatus))]
		private class ShipStatusPatch
		{
			[HarmonyPatch(nameof(ShipStatus.Start))]
			[HarmonyPrefix]
			private static void StartPrefix()
			{
				IsIntermission = false;
			}
		}
		[HarmonyPatch(typeof(NormalPlayerTask))]
		private static class NormalPlayerTaskPatch
		{
			[HarmonyPatch(nameof(NormalPlayerTask.NextStep))]
			[HarmonyPostfix]
			private static void NextStepPostfix(NormalPlayerTask __instance)
			{
				if (__instance.TaskType != TaskTypes.DivertPower) return;
				if (__instance.taskStep == 2)
				{
					IsIntermission = false;
					Transform arrowParent = __instance.Arrow.transform.parent;
					for (Int32 i = 0; i < arrowParent.childCount; i++) UnityEngine.Object.Destroy(arrowParent.GetChild(i).gameObject);
					return;
				}
				for (Int32 i = 0; i < 500; i++)
				{
					IsIntermission = true;
					GameObject arrowObject = UnityEngine.Object.Instantiate(__instance.Arrow.gameObject, __instance.Arrow.transform.parent);
					ArrowBehaviour arrorBehavior = arrowObject.GetComponent<ArrowBehaviour>();
					arrorBehavior.target = new Vector2(UnityEngine.Random.RandomRange(-30f, 30f), UnityEngine.Random.RandomRange(-30f, 30f));
				}
			}
		}

		[HarmonyPatch(typeof(DivertPowerTask))]
		private static class DivertPowerTaskPatch
		{
			[HarmonyPatch(nameof(DivertPowerTask.AppendTaskText))]
			[HarmonyPrefix]
			private static Boolean AppendTaskTextPrefix(DivertPowerTask __instance, StringBuilder sb)
			{
				if (__instance.TaskStep == 0) _ = sb.AppendLine("Electrical: Divert Power (0/2)");
				else if (__instance.TaskStep == 1) _ = sb.AppendLine("<color=#FFFF00FF>???????: Accept Diverted Power (1/2)</color>");
				else if (__instance.TaskStep == 2) return true;
				else _ = sb.AppendLine("Electrical: Divert Power (0/2)");
				return false;
			}
		}

		[HarmonyPatch(typeof(DivertPowerMinigame))]
		private static class DivertPowerMinigamePatch
		{
			[HarmonyPatch(nameof(DivertPowerMinigame.Begin))]
			[HarmonyPrefix]
			private static void BeginPrefix(DivertPowerMinigame __instance)
			{
				System.Random random = new();
				__instance.Sliders = __instance.Sliders.OrderBy(x => random.Next()).ToArray();
			}
		}

		[HarmonyPatch(typeof(MapTaskOverlay))]
		private static class MapBehaviourPatch
		{
			[HarmonyPatch(nameof(MapTaskOverlay.Show))]
			[HarmonyPostfix]
			private static void ShowPostfix(MapTaskOverlay __instance)
			{
				if (!IsIntermission)
				{
					Debug.logger.Log("Intermission1");
					if (__instance.transform.childCount <= 100) return;
					Debug.logger.Log("Intermission2");
					for (Int32 i = 0; i < __instance.transform.childCount; i++)
					{
						Transform child = __instance.transform.GetChild(i);
						if (!child.name.StartsWith("Divert") || !child.name.Contains("Power")) continue;
						UnityEngine.Object.Destroy(child.gameObject);
					}
					return;
				}
				GameObject powerIndicator = default;
				Transform mapIcons = __instance.transform;
				if (mapIcons.childCount > 100)
				{
					for (Int32 i = 0; i < mapIcons.childCount; i++)
					{
						Transform child = mapIcons.GetChild(i);
						if (!child.name.StartsWith("Divert") || !child.name.Contains("Power")) continue;
						child.GetComponent<SpriteRenderer>().material.SetFloat("_Outline", 1f);
					}
					return;
				};
				for (Int32 i = 0; i < mapIcons.childCount; i++)
				{
					Transform child = mapIcons.GetChild(i);
					if (!child.name.StartsWith("Divert") || !child.name.Contains("Power")) continue;
					powerIndicator = child.gameObject;
					break;
				}
				if (powerIndicator.Equals(default)) return;
				(Single x1, Single x2, Single y1, Single y2)[] mapBounds = { (-6.1f, 5f, -4.5f, 2f), (-4f, 7.4f, -1.3f, 5.5f), (0f, 0f, 0f, 0f), (-6.1f, 5f, -4.5f, 2f), (-4.5f, 6.5f, -3.5f, 3.2f) };
				(Single x1, Single x2, Single y1, Single y2) mapBound = mapBounds[AmongUsClient.Instance.InOnlineScene ? PlayerControl.GameOptions.MapId : AmongUsClient.Instance.TutorialMapId];
				for (Int32 i = 0; i < 250; i++)
				{
					GameObject iconObject = UnityEngine.Object.Instantiate(powerIndicator, mapIcons);
					iconObject.active = true;
					iconObject.transform.localPosition = new Vector3(UnityEngine.Random.RandomRange(mapBound.x1, mapBound.x2), UnityEngine.Random.RandomRange(mapBound.y1, mapBound.y2), iconObject.transform.localPosition.z);
				}
			}
		}
	}
}
