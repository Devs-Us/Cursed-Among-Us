using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Reactor.Extensions;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace CursedAmongUs.Source.Tasks
{
	[HarmonyPatch(typeof(UnlockManifoldsMinigame))]
	static class UnlockManifold
	{
		delegate bool d_LoadImage(IntPtr texture, IntPtr data, bool markNonReadable);
		static d_LoadImage ICallLoadImage;

		[HarmonyPatch(nameof(UnlockManifoldsMinigame.Begin))]
		[HarmonyPrefix]
		static void BeginPrefix(UnlockManifoldsMinigame __instance)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream("CursedAmongUs.Resources.UnlockManifold.png");
			byte[] textureBytes = stream.ReadFully();
			Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
			ICallLoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");
			Il2CppStructArray<byte> il2CPPArray = textureBytes;
			ICallLoadImage.Invoke(texture.Pointer, il2CPPArray.Pointer, false);
			foreach (SpriteRenderer button in __instance.Buttons)
			{
				button.sprite = Sprite.Create(texture, new Rect(0f, 0f, button.sprite.rect.width, button.sprite.rect.height), new Vector2(0.5f, 0.5f), 100f);
			}
		}
	}
}