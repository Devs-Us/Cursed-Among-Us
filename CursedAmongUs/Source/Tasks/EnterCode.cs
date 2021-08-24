using System;
using HarmonyLib;
using UnityEngine;
using Random = System.Random;

namespace CursedAmongUs.Source.Tasks
{
	internal static class CustomEnterCode
	{
		[HarmonyPatch(typeof(EnterCodeMinigame))]
		private static class EnterCodePatch
		{
			private static String _targetNumberString;

			[HarmonyPostfix]
			[HarmonyPatch(nameof(EnterCodeMinigame.Begin))]
			private static void BeginPostfix(EnterCodeMinigame __instance)
			{
				Random random = new();
				Int32 targetNumberFirst = random.Next(0x3B9AC9FF, Int32.MaxValue);
				Int32 targetNumberLast = random.Next(0x3B9AC9FF, Int32.MaxValue);

				_targetNumberString = $"{targetNumberFirst}{targetNumberLast}";
				__instance.targetNumber = BitConverter.ToInt32(__instance.MyNormTask.Data, 0);
				__instance.TargetText.text = _targetNumberString;
				__instance.TargetText.transform.localPosition += Vector3.down * 0.25f;
			}

			[HarmonyPrefix]
			[HarmonyPatch(nameof(EnterCodeMinigame.EnterDigit))]
			private static Boolean EnterDigitPrefix(EnterCodeMinigame __instance,
				[HarmonyArgument(0)] Int32 enteredDigit)
			{
				if (__instance.animating || __instance.done) return false;

				if (__instance.NumberText.text.Length >= __instance.TargetText.text.Length)
				{
					if (!Constants.ShouldPlaySfx()) return false;
					_ = SoundManager.Instance.PlaySound(__instance.RejectSound, false, 1f);
					return false;
				}

				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(__instance.NumberSound, false, 1f)
						.pitch = Mathf.Lerp(0.8f, 1.2f, enteredDigit / 9f);
				}

				__instance.numString += enteredDigit.ToString();

				if (__instance.numString == _targetNumberString)
					__instance.number = __instance.targetNumber;

				__instance.NumberText.text = new String('*', __instance.numString.Length);
				__instance.NumberText.enableAutoSizing = true;

				return false;
			}
		}
	}
}