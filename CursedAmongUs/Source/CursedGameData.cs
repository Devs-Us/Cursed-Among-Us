using HarmonyLib;

namespace CursedAmongUs.Source
{
	public static class CursedGameData
	{
		public static int WiresNum = 0;

		[HarmonyPatch(typeof(ShipStatus))]
		static class ShipStatusPatch
		{
			[HarmonyPatch(nameof(ShipStatus.Start))]
			[HarmonyPrefix]
			static void StartPrefix()
			{
				WiresNum = 0;
			}
		}
	}
}