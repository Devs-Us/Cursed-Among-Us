using System;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using CursedAmongUs.Source;
using CursedAmongUs.Source.Tasks;
using HarmonyLib;
using Reactor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CursedAmongUs
{
	[BepInPlugin(Id, "Cursed Tasks", "1.0")]
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
			GameObject cursedObject = new("CursedAmongUs");
			Object.DontDestroyOnLoad(cursedObject);
		}
	}
}