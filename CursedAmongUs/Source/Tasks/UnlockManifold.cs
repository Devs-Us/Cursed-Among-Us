using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Reactor.Extensions;
using UnhollowerBaseLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	[HarmonyPatch(typeof(UnlockManifoldsMinigame))]
	internal static class UnlockManifold
	{
		private delegate Boolean d_LoadImage(IntPtr texture, IntPtr data, Boolean markNonReadable);

		private static d_LoadImage ICallLoadImage;

		[HarmonyPatch(nameof(UnlockManifoldsMinigame.Begin))]
		[HarmonyPrefix]
		private static void BeginPrefix(UnlockManifoldsMinigame __instance)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream("CursedAmongUs.Resources.UnlockManifold.png");
			Byte[] textureBytes = stream.ReadFully();
			Texture2D texture = new(2, 2, TextureFormat.ARGB32, false);
			ICallLoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");
			Il2CppStructArray<Byte> il2CPPArray = textureBytes;
			_ = ICallLoadImage.Invoke(texture.Pointer, il2CPPArray.Pointer, false);
			foreach (SpriteRenderer button in __instance.Buttons)
			{
				button.sprite = Sprite.Create(texture, new Rect(0f, 0f, button.sprite.rect.width, button.sprite.rect.height), new Vector2(0.5f, 0.5f), 100f);
			}
		}
	}
}