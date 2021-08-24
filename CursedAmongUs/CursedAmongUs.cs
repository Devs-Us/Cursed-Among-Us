using System;
using BepInEx;
using BepInEx.IL2CPP;
using CursedAmongUs.Source;
using CursedAmongUs.Source.Tasks;
using HarmonyLib;
using Reactor;
using UnhollowerRuntimeLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CursedAmongUs
{
	[BepInPlugin(Id)]
	[BepInProcess("Among Us.exe")]
	[BepInDependency(ReactorPlugin.Id)]
	public class CursedAmongUs : BasePlugin
	{
		public const String Id = "DevsUs.CursedAmongUs";
		public Harmony Harmony { get; } = new(Id);

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
			ClassInjector.RegisterTypeInIl2Cpp<CursedGameData>();
			ClassInjector.RegisterTypeInIl2Cpp<CursedWeapons.WeaponsCustom>();
			ClassInjector.RegisterTypeInIl2Cpp<UploadDataCustom>();
			GameObject cursedObject = new("CursedAmongUs");
			Object.DontDestroyOnLoad(cursedObject);
			_ = cursedObject.AddComponent<CursedGameData>();
		}
	}
}