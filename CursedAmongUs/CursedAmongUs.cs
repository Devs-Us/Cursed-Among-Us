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
		public const System.String Version = "v1.0.0";

		public Harmony Harmony { get; } = new Harmony("DevsUs.CursedAmongUs");

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
			ClassInjector.RegisterTypeInIl2Cpp<Source.CursedGameData>();
			ClassInjector.RegisterTypeInIl2Cpp<Source.Tasks.CursedWeapons.WeaponsCustom>();
			ClassInjector.RegisterTypeInIl2Cpp<Source.Tasks.UploadDataCustom>();
			GameObject cursedObject = new("CursedAmongUs");
			Object.DontDestroyOnLoad(cursedObject);
			_ = cursedObject.AddComponent<Source.CursedGameData>();
		}
	}
}
