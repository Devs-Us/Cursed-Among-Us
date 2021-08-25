using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Reactor.Extensions;
using UnhollowerBaseLib;
using UnityEngine;

namespace CursedAmongUs.Source.Others
{
	public static class SpriteHelper
	{
		private static DLoadImage _icallLoadImage;
		private delegate Boolean DLoadImage(IntPtr texture, IntPtr data, Boolean markNonReadable);
		
		public static Sprite CreateSpriteFromResources(String path)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			String streamPath = assembly.GetManifestResourceNames()
				.FirstOrDefault(x => x.StartsWith(assembly.GetName().Name) && x.EndsWith(path));
			
			Stream stream = assembly.GetManifestResourceStream(streamPath);
			Byte[] textureBytes = stream.ReadFully();
			Texture2D texture = new(2, 2, TextureFormat.ARGB32, false);
			
			_icallLoadImage = IL2CPP.ResolveICall<DLoadImage>("UnityEngine.ImageConversion::LoadImage");
			Il2CppStructArray<Byte> il2CPPArray = textureBytes;
			_ = _icallLoadImage.Invoke(texture.Pointer, il2CPPArray.Pointer, false);

			return texture.CreateSprite();
		}
	}
}