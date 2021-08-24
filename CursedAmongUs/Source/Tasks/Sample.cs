using System;
using System.Linq;
using HarmonyLib;
using Il2CppSystem.Text;
using Reactor;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using Object = System.Object;

namespace CursedAmongUs.Source.Tasks
{
	internal class CursedSample
	{
		[HarmonyPatch(typeof(SampleMinigame))]
		private static class SampleMinigamePatch
		{
			[HarmonyPostfix]
			[HarmonyPatch(nameof(SampleMinigame.Begin))]
			private static void BeginPostfix(SampleMinigame __instance)
			{
				SampleMinigame.ProcessingStrings = new StringNames[]
				{
					CustomStringName.Register("THIS WILL TAKE FOREVER")
				};
				
				__instance.TimePerStep = 3640f;
			}
		}

		[HarmonyPatch(typeof(NormalPlayerTask))]
		private static class NormalPlayerTaskPatch
		{
			[HarmonyPrefix]
			[HarmonyPatch(nameof(NormalPlayerTask.AppendTaskText))]
			private static Boolean BeginPrefix(NormalPlayerTask __instance, [HarmonyArgument(0)] StringBuilder sb)
			{
				if (__instance.TaskType != TaskTypes.InspectSample) return true;
				if (!__instance.ShowTaskTimer || __instance.TimerStarted != NormalPlayerTask.TimerState.Started)
					return true;
				
				String startAt = DestroyableSingleton<TranslationController>.Instance.GetString(__instance.StartAt);
				String taskType = DestroyableSingleton<TranslationController>.Instance.GetString(__instance.TaskType);

				TimeSpan time = TimeSpan.FromSeconds((Int32)__instance.TaskTimer);
				
				String painfulCounter = (Int32)__instance.TaskTimer switch
				{
					>= 3600 => $"{time.Hours}h {time.Seconds}s",
					>= 60 => $"{time.Minutes}m {time.Seconds}s",
					_ => $"{time.Seconds}s"
				};
				
				_ = sb.AppendLine($"<color=yellow>{startAt}: {taskType} " +
				                  $"({painfulCounter})</color>");
				
				return false;
			}

			[HarmonyPostfix]
			[HarmonyPatch(nameof(NormalPlayerTask.FixedUpdate))]
			private static void FixedUpdatePostfix(NormalPlayerTask __instance)
			{
				if (__instance.TaskType != TaskTypes.InspectSample) return;

				__instance.TaskTimer -= (Int32)__instance.TaskTimer switch
				{
					>= 3455 => 0,
					>= 2600 => 1.8f,
					>= 2400 => 2.2f,
					>= 1700 => 2.7f,
					>= 1000 => 3.4f,
					>= 15 => 3.7f,
					_ => 0
				};
			}
		}
	}
}