using System;
using HarmonyLib;

namespace CursedAmongUs.Source.Others
{
	static class CursedBlockedWords
	{

		//Feel free to add to these lists!
		static readonly String[] UnblockedAll = new String[]
		{
			"fuck", "assface", "blackcock", "blowjob", "bullshit", "c0ck~", "cunt", "dick", "hentai", "jackoff", "jackass", "nazi", "nlgger", "nlgga", "n1gga", "n1gger", "penis", "puta~", "puto~", "rape", "weiner", "fuc", "b!tch", "sex", "shit^", "p0rn"
		};

		static readonly String[] BlockedAll = new String[]
		{
			"sus", "discord", "comma", "minecraft", "roblox", "cod", "poop", "red", "pgg", "cursed", "mod", "curse", "love", "trump", "biden", "donald", "joe", "hillary", "clinton", "barack", "obama", "germany", "german", "italy", "italian", "communist", "lmfao", "lmao", "fml", "gtfo", "omfg", "wtf", "ffs", "sext", "vented", "chasing", "nudes", "coll", "milk", "banana", "cnn", "fox", "npr", "msnbc", "ironman", "2024", "2020", "france", "french", "francaise", "francais", "fortnite"
		};

		[HarmonyPatch(typeof(LetterTree))]
		static class LetterTreePatch
		{
			[HarmonyPatch(nameof(LetterTree.AddWord))]
			[HarmonyPrefix]
			static Boolean AddWordPrefix(String word)
			{
				// Feel free to add to these list
				if (word == BlockedWords.AllWords[0])
				{
					for (Int32 i = 0; i < BlockedAll.Length; i++) BlockedWords.SkipList.AddWord(BlockedAll[i]);
				}
				return !Array.Exists(UnblockedAll, ele => ele == word);
			}
		}
	}
}
