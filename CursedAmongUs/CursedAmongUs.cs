using BepInEx;
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
		public const System.String Version = "v0.0.2";

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
			GameObject gameObject = GameObject.Find("CursedAmongUs");
			if (gameObject != null) return;
			ClassInjector.RegisterTypeInIl2Cpp<Source.Tasks.UploadDataCustom>();
			ClassInjector.RegisterTypeInIl2Cpp<Source.CursedGameData>();
			GameObject cursedObject = new GameObject("CursedAmongUs"); // For Future, just make a component.cs and add it to this
			Object.DontDestroyOnLoad(cursedObject);
			_ = cursedObject.AddComponent<Source.CursedGameData>();
		}
	}
}
