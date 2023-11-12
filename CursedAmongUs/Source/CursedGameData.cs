using System;
using CursedAmongUs.Source.Others;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace CursedAmongUs.Source
{
	[RegisterInIl2Cpp]
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