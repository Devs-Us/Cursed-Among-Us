//using System;
//using System.Collections.Generic;
//using System.Text;
//using HarmonyLib;
//
//namespace CursedAmongUs.Source.Tasks
//{
//	[HarmonyPatch(typeof(UploadDataGame))]
//	class UploadDataPatch
//	{
//		[HarmonyPatch(nameof(UploadDataGame.DoRun))]
//		[HarmonyPrefix]
//		static void DoRunPrefix(UploadDataGame __instance)
//		{
//			UnityEngine.Debug.logger.Log("waypoint 1");
//		}
//	}
//}
