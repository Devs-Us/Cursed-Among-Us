using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace CursedAmongUs
{
	[BepInPlugin("DevsUs.CusedAmongUs", "CursedAmongUs", Version)]
	[BepInProcess("Among Us.exe")]
	[BepInDependency(ReactorPlugin.Id)]
	public class CursedAmongUs : BasePlugin
	{
		public const string Version = "v0.0.2";

		public Harmony Harmony { get; } = new Harmony("DevsUs.CusedAmongUs");

		public override void Load()
		{
			Harmony.PatchAll();
		}
	}

	[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
	public static class AmongUsClientPatch
	{
		public static void Postfix()
		{
			var gameObject = GameObject.Find("CursedAmongUs");
			if (gameObject != null) return;
			ClassInjector.RegisterTypeInIl2Cpp<Source.Tasks.UploadDataCustom>();
			GameObject cursedObject = new GameObject("CursedAmongUs"); // For Future, just make a component.cs and add it to this
			Object.DontDestroyOnLoad(cursedObject);
		}
	}
}
