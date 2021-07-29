using System;
using UnityEngine;

namespace CursedAmongUs.Source
{
	public class CursedGameData : MonoBehaviour
	{
		public CursedGameData(IntPtr ptr) : base(ptr) { }

		public void Start()
		{
		}

		public void Update()
		{
			Others.CursedVent.Update();
		}
	}
}