using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	[HarmonyPatch(typeof(WireMinigame))]
	class WireMinigamePatch
	{
		public static int numWires = 40;

		public static float scalarY = ((1f - 2) / numWires) + 0.47f;

		[HarmonyPatch(nameof(WireMinigame.Begin))]
		[HarmonyPrefix]
		static void BeginPrefix(WireMinigame __instance)
		{
			Transform ParentAll = GameObject.Find("Main Camera/WireMinigame(Clone)").transform;
			Transform ParentLeftNode = ParentAll.FindChild("LeftWires").transform;
			Transform ParentRightNode = ParentAll.FindChild("RightWires").transform;
			__instance.ExpectedWires = new sbyte[numWires];
			WireMinigame.colors = new Color[numWires];
			__instance.Symbols = new Sprite[numWires];
			__instance.ActualWires = new sbyte[numWires];
			__instance.LeftLights = new SpriteRenderer[numWires];
			__instance.RightLights = new SpriteRenderer[numWires];
			__instance.LeftNodes = new Wire[numWires];
			__instance.RightNodes = new WireNode[numWires];
			Helpers.DestroyObjects(ParentLeftNode);
			float positionY = 2.25f;
			for (int i = 0; i < numWires; i++)
			{
				GameObject newGameObject = Helpers.BuildWire(ParentLeftNode.FindChild("LeftWireNode").gameObject, ref positionY);
				Transform headTransform = newGameObject.transform.FindChild("Head");
				headTransform.localPosition = new Vector3(0.235f, headTransform.localPosition.y, headTransform.localPosition.z);
				headTransform.GetComponent<CircleCollider2D>().enabled = true;
				headTransform.GetComponent<CircleCollider2D>().radius = 0.1f;
				Wire wireComponent = newGameObject.GetComponent<Wire>();
				wireComponent.enabled = true;
				__instance.LeftNodes[i] = wireComponent;
			}
			Helpers.DestroyObjects(ParentRightNode);
			positionY = 2.25f;
			for (int i = 0; i < numWires; i++)
			{
				GameObject newGameObject = Helpers.BuildWire(ParentRightNode.FindChild("RightWireNode").gameObject, ref positionY);
				Transform headTransform = newGameObject.transform.FindChild("electricity_wiresBase1");
				newGameObject.transform.FindChild("electricity_wiresConnectBase").localScale = new Vector3(8f, 0.5f, 1f);
				headTransform.localPosition = new Vector3(0.145f, 0f, headTransform.localPosition.z);
				newGameObject.transform.GetComponent<CircleCollider2D>().enabled = true;
				newGameObject.GetComponent<CircleCollider2D>().radius = 0.1f;
				WireNode wireComponent = newGameObject.GetComponent<WireNode>();
				wireComponent.enabled = true;
				__instance.RightNodes[i] = wireComponent;
			}
			ParentAll.FindChild("LeftLights").gameObject.active = false;
			ParentAll.FindChild("RightLights").gameObject.active = false;
			for (int i = 0; i < numWires; i++)
			{
				__instance.ExpectedWires[i] = (sbyte)i;
				WireMinigame.colors[i] = Color.HSVToRGB((float)i / numWires, 1f, 1f);
				__instance.ActualWires[i] = -1;
				__instance.Symbols[i] = new Sprite();
			}
		}

		[HarmonyPatch(nameof(WireMinigame.UpdateLights))]
		[HarmonyPrefix]
		static bool SetColorPrefix()
		{
			return false;
		}
	}

	[HarmonyPatch(typeof(Wire))]
	static class WirePatch
	{
		[HarmonyPatch(nameof(Wire.ResetLine))]
		[HarmonyPostfix]
		static void ResetLinePostfix(Wire __instance, [HarmonyArgument(1)] bool reset)
		{
			if (reset)
			{
				__instance.ColorBase.transform.localScale = new Vector3(5f, 0.5f, 1f);
				__instance.ColorBase.transform.localPosition = new Vector3(-0.3f, 0f, 1f);
				return;
			}
			__instance.ColorBase.transform.localScale = new Vector3(7.8f, 0.5f, 1f);
			__instance.ColorBase.transform.localPosition = new Vector3(-0.22f, 0f, 1f);
			__instance.Liner.transform.localScale = new Vector3(__instance.Liner.transform.localScale.x, 0.5f, 1f);
		}
	}
	
	class Helpers
	{
		public static GameObject BuildWire(GameObject prefab, ref float positionY)
		{
			positionY -= 4.6f / WireMinigamePatch.numWires;
			GameObject newGameObject = Object.Instantiate(prefab, prefab.transform.parent);
			newGameObject.transform.localPosition = new Vector3(newGameObject.transform.localPosition.x, positionY, newGameObject.transform.localPosition.z);
			for (int i = 0; i < newGameObject.transform.childCount; i++)
			{
				newGameObject.transform.GetChild(i).localScale = new Vector3(1f, 0.5f, 1f);
			}
			newGameObject.transform.FindChild("BaseSymbol").gameObject.active = false;
			return newGameObject;
		}

		public static void DestroyObjects(Transform parent)
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				GameObject childNode = parent.GetChild(i).gameObject;
				if (!parent.GetChild(i).gameObject.name.Contains("WireNode")) continue;
				Object.Destroy(childNode);
			}
		}
	}
}