using System;
using CursedAmongUs.Source.Others;
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
			CursedVent.Update();
		}
	}
}