using UnityEngine;

namespace MalbersAnimations
{
	/// <summary> Used to store Transform pos, rot and scale values </summary>
	[System.Serializable]
	public struct TransformOffset
	{
		public Vector3 Position;
		public Vector3 Rotation;
		public Vector3 Scale;


		public TransformOffset(int def)
		{
			Position = Vector3.zero;
			Rotation = Vector3.zero;
			Scale = Vector3.one;
		}
	}
}