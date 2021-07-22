using System;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	[HarmonyPatch(typeof(UploadDataGame))]
	internal class UploadDataPatch
	{
		[HarmonyPatch(nameof(UploadDataGame.DoPercent))]
		[HarmonyPrefix]
		private static Boolean DoPercentPrefix()
		{
			return false;
		}

		[HarmonyPatch(nameof(UploadDataGame.DoText))]
		[HarmonyPrefix]
		private static Boolean DoTextPrefix(UploadDataGame __instance)
		{
			UploadDataCustom customComponent = __instance.gameObject.AddComponent<UploadDataCustom>();
			__instance.gameObject.active = true;
			customComponent.enabled = true;
			return false;
		}
	}

	internal class UploadDataCustom : MonoBehaviour
	{
		private readonly Int32 StartTime = IntRange.Next(604800 / 6, 604800);
		private Int32 TotalTime;
		private Int32 TotalCounter;

		public UploadDataCustom(IntPtr ptr) : base(ptr) { }

		public void Start()
		{
			TotalCounter = 8;
			TotalTime = StartTime;
			InvokeRepeating("UploadData", 0, 1f);
		}

		public void UploadData()
		{
			UploadDataGame uploadData = gameObject.GetComponent<UploadDataGame>();
			if (StartTime - TotalTime < 47)
			{
				TotalTime--;
			}
			else if (TotalCounter > 0)
			{
				TotalCounter--;
				TotalTime /= 5;
			}
			else
			{
				CancelInvoke();
				uploadData.running = false;
			}
			Int32 days = TotalTime / 86400;
			Int32 hours = TotalTime / 3600 % 24;
			Int32 minutes = TotalTime / 60 % 60;
			Int32 seconds = TotalTime % 60;
			String dateString;
			if (days > 0) dateString = $"{days}d {hours}hr {minutes}m {seconds}s";
			else if (hours > 0) dateString = $"{hours}hr {minutes}m {seconds}s";
			else if (minutes > 0) dateString = $"{minutes}m {seconds}s";
			else dateString = $"{seconds}s";
			uploadData.EstimatedText.text = dateString;
			uploadData.Gauge.Value = 1 - (TotalCounter / 8f);
			uploadData.PercentText.text = Mathf.RoundToInt(100 - (100 * TotalCounter / 8f)).ToString() + "%";
		}
	}
}
