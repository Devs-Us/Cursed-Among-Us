using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	[HarmonyPatch(typeof(UploadDataGame))]
	class UploadDataPatch
	{
		// I must override the function I believe
		[HarmonyPatch(nameof(UploadDataGame.DoPercent))]
		[HarmonyPrefix]
		static bool DoPercentPrefix(UploadDataGame __instance)
		{
			return false;
		}

		[HarmonyPatch(nameof(UploadDataGame.DoText))]
		[HarmonyPrefix]
		static bool DoTextPrefix(UploadDataGame __instance)
		{
			UploadDataCustom customComponent = __instance.gameObject.AddComponent<UploadDataCustom>();
			__instance.gameObject.active = true;
			customComponent.enabled = true;
			return false;
		}
	}

	class UploadDataCustom : MonoBehaviour
	{
		private int StartTime = IntRange.Next(604800 / 6, 604800);
		private int TotalTime;
		private int TotalCounter;

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
			if (StartTime - TotalTime < 47) TotalTime--;
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
			int days = TotalTime / 86400;
			int hours = TotalTime / 3600 % 24;
			int minutes = TotalTime / 60 % 60;
			int seconds = TotalTime % 60;
			string dateString;
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
