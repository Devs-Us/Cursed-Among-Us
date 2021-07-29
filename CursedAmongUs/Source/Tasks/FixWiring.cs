using System;
using HarmonyLib;
using UnityEngine;

namespace CursedAmongUs.Source.Tasks
{
	internal class CursedFixWiring
	{
		public static Int32 WiresNum = 0;

		public static Int32 NumWires = 4;

		public static Single ScalarY = 1f;

		[HarmonyPatch(typeof(ShipStatus))]
		private class ShipStatusPatch
		{
			[HarmonyPatch(nameof(ShipStatus.Start))]
			[HarmonyPrefix]
			private static void StartPrefix()
			{
				WiresNum = 0;
			}
		}

		[HarmonyPatch(typeof(WireMinigame))]
		internal class WireMinigamePatch
		{
			[HarmonyPatch(nameof(WireMinigame.Begin))]
			[HarmonyPrefix]
			private static void BeginPrefix(WireMinigame __instance)
			{
				Int32[] WiresOrder = new Int32[3] { 4, 6, 60 };
				NumWires = WiresOrder[WiresNum];
				ScalarY = NumWires < 12 ? 1f : (8f / NumWires) + 0.3f;
				Transform ParentAll = GameObject.Find("Main Camera/WireMinigame(Clone)").transform;
				__instance.ExpectedWires = new SByte[NumWires];
				WireMinigame.colors = new Color[NumWires];
				__instance.Symbols = new Sprite[NumWires];
				__instance.ActualWires = new SByte[NumWires];
				__instance.LeftLights = new SpriteRenderer[NumWires];
				__instance.RightLights = new SpriteRenderer[NumWires];
				__instance.LeftNodes = new Wire[NumWires];
				__instance.RightNodes = new WireNode[NumWires];
				Transform ParentLeftNode = ParentAll.FindChild("LeftWires").transform;
				Helpers.DestroyObjects(ParentLeftNode);
				Single positionY = 2.25f;
				for (Int32 i = 0; i < NumWires; i++)
				{
					GameObject newGameObject = Helpers.BuildWire(ParentLeftNode.FindChild("LeftWireNode").gameObject, ref positionY);
					for (Int32 j = 0; j < newGameObject.transform.childCount; j++) newGameObject.transform.GetChild(j).localScale = new Vector3(1f, ScalarY, 1f);
					Transform headTransform = newGameObject.transform.FindChild("Head");
					headTransform.localPosition = new Vector3(0.235f, headTransform.localPosition.y, headTransform.localPosition.z);
					headTransform.GetComponent<CircleCollider2D>().enabled = true;
					headTransform.GetComponent<CircleCollider2D>().radius = (1.5f / NumWires) + 0.1f;
					Wire wireComponent = newGameObject.GetComponent<Wire>();
					wireComponent.enabled = true;
					__instance.LeftNodes[i] = wireComponent;
				}
				Transform ParentRightNode = ParentAll.FindChild("RightWires").transform;
				Helpers.DestroyObjects(ParentRightNode);
				positionY = 2.25f;
				for (Int32 i = 0; i < NumWires; i++)
				{
					GameObject newGameObject = Helpers.BuildWire(ParentRightNode.FindChild("RightWireNode").gameObject, ref positionY);
					Transform headTransform = newGameObject.transform.FindChild("electricity_wiresBase1");
					newGameObject.transform.localScale = new Vector3(1f, ScalarY, 1f);
					headTransform.localPosition = new Vector3(0.145f, 0f, headTransform.localPosition.z);
					newGameObject.transform.GetComponent<CircleCollider2D>().enabled = true;
					newGameObject.GetComponent<CircleCollider2D>().radius = 0.45f;
					WireNode wireComponent = newGameObject.GetComponent<WireNode>();
					wireComponent.enabled = true;
					__instance.RightNodes[i] = wireComponent;
				}
				ParentAll.FindChild("LeftLights").gameObject.active = false;
				ParentAll.FindChild("RightLights").gameObject.active = false;
				for (Int32 i = 0; i < NumWires; i++)
				{
					__instance.ExpectedWires[i] = (SByte)i;
					WireMinigame.colors[i] = Color.HSVToRGB((Single)i / NumWires, 1f, 1f);
					__instance.ActualWires[i] = -1;
					__instance.Symbols[i] = new Sprite();
				}
			}

			[HarmonyPatch(nameof(WireMinigame.UpdateLights))]
			[HarmonyPrefix]
			private static Boolean SetColorPrefix()
			{
				return false;
			}

			[HarmonyPatch(nameof(WireMinigame.CheckTask))]
			[HarmonyPrefix]
			private static void CheckTaskPrefix(WireMinigame __instance)
			{
				Boolean flag = true;
				for (Int32 i = 0; i < __instance.ActualWires.Length; i++)
				{
					if (__instance.ActualWires[i] != __instance.ExpectedWires[i])
					{
						flag = false;
						break;
					}
				}
				if (flag) WiresNum++;
			}

			[HarmonyPatch(nameof(WireMinigame.CheckRightSide))]
			[HarmonyPrefix]
			private static Boolean CheckRightSidePrefix(WireMinigame __instance, ref WireNode __result, Vector2 pos)
			{
				Collider2D leftNode = __instance.myController.amTouching;
				Int32 leftId = leftNode.transform.parent.GetComponent<Wire>().WireId;
				for (Int32 i = 0; i < __instance.RightNodes.Length; i++)
				{
					WireNode wireNode = __instance.RightNodes[i];
					if (wireNode.hitbox.OverlapPoint(pos) && __instance.ExpectedWires[leftId] == wireNode.WireId) __result = wireNode;
				}
				if (!__result) __result = null;
				return false;
			}
		}

		[HarmonyPatch(typeof(Wire))]
		internal static class WirePatch
		{
			[HarmonyPatch(nameof(Wire.ResetLine))]
			[HarmonyPostfix]
			private static void ResetLinePostfix(Wire __instance, [HarmonyArgument(1)] Boolean reset)
			{
				if (reset)
				{
					__instance.ColorBase.transform.localScale = new Vector3(5f, ScalarY, 1f);
					__instance.ColorBase.transform.localPosition = new Vector3(-0.3f, 0f, 1f);
					return;
				}
				__instance.ColorBase.transform.localScale = new Vector3(7.8f, ScalarY, 1f);
				__instance.ColorBase.transform.localPosition = new Vector3(-0.22f, 0f, 1f);
				__instance.Liner.transform.localScale = new Vector3(__instance.Liner.transform.localScale.x, ScalarY, 1f);
			}
		}

		internal class Helpers
		{
			public static GameObject BuildWire(GameObject prefab, ref Single positionY)
			{
				positionY -= 4.6f / (NumWires + 1);
				GameObject newGameObject = UnityEngine.Object.Instantiate(prefab, prefab.transform.parent);
				newGameObject.transform.localPosition = new Vector3(newGameObject.transform.localPosition.x, positionY, newGameObject.transform.localPosition.z);
				newGameObject.transform.FindChild("BaseSymbol").gameObject.active = false;
				return newGameObject;
			}

			public static void DestroyObjects(Transform parent)
			{
				for (Int32 i = 0; i < parent.childCount; i++)
				{
					GameObject childNode = parent.GetChild(i).gameObject;
					if (!parent.GetChild(i).gameObject.name.Contains("WireNode")) continue;
					UnityEngine.Object.Destroy(childNode);
				}
			}
		}
	}
}