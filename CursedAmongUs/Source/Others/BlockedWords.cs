using System;
using HarmonyLib;

namespace CursedAmongUs.Source.Others
{
	internal static class CursedBlockedWords
	{
		//Feel free to add to these lists!
		private static readonly String[] UnblockedAll = new String[]
		{
			"fuck", "assface", "blackcock", "blowjob", "bullshit", "c0ck~", "cunt", "dick", "hentai", "jackoff", "jackass", "nazi", "nlgger", "nlgga", "n1gga", "n1gger", "penis", "puta~", "puto~", "rape", "weiner", "fuc", "b!tch", "sex", "shit^", "p0rn"
		};
		private static readonly String[] BlockedAll = new String[]
		{
			"sus", "discord", "comma", "minecraft", "roblox", "cod", "poop", "red", "pgg", "cursed", "mod", "curse", "love", "trump", "biden", "donald", "joe", "hillary", "clinton", "barack", "obama", "germany", "german", "italy", "italian", "communist", "lmfao", "lmao", "fml", "gtfo", "omfg", "wtf", "ffs", "sext", "vented", "chasing", "nudes", "coll", "milk", "banana", "cnn", "fox", "npr", "msnbc", "ironman", "2024", "2020", "france", "french", "francaise", "francais", "fortnite", "brazil", "brasil", "brazilian", "amogus", "imposter", "christian", "jewish", "jew", "catholic", "sussy"
		};

		[HarmonyPatch(typeof(LetterTree))]
		private static class LetterTreePatch
		{
			[HarmonyPatch(nameof(LetterTree.AddWord))]
			[HarmonyPrefix]
			private static Boolean AddWordPrefix(String word)
			{
				if (word == BlockedWords.AllWords[0])
				{
					for (Int32 i = 0; i < BlockedAll.Length; i++) BlockedWords.SkipList.AddWord(BlockedAll[i]);
				}
				return !Array.Exists(UnblockedAll, ele => ele == word);
			}
		}
	}
}
