using System;
using UnityEngine;

namespace WhiteCat.Tween
{
	/// <summary>
	/// 插值器
	/// </summary>
	[Serializable]
	public partial class Interpolator : SerializableClassWithEditor
	{
		// -1 使用自定义参数, -2 使用自定义曲线
		[SerializeField]
		int _index = 0;

		[SerializeField]
		int _adjustableMask = 0;

		[SerializeField]
		float _adjustableBalance = 0f;

		[SerializeField]
		float _adjustableFactor = 0f;

		[SerializeField]
		AnimationCurve _curve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0));


		/// <summary>
		/// 插值器类型
		/// </summary>
		public Type type
		{
			get { return (Type)_index; }
			set { _index = (int)value; }
		}


		/// <summary>
		/// 自定义曲线
		/// </summary>
		public AnimationCurve customCurve
		{
			get { return _curve; }
			set { _curve = value; }
		}


		// 内置的常用插值
		static readonly Func<float, float>[] _builtInInterpolators =
		{
			Linear,
			Accelerate,
			Decelerate,
			AccelerateDecelerate,
			Anticipate,
			Overshoot,
			AnticipateOvershoot,
			Bounce,
			Parabolic,
			Sine
		};


		/// <summary>
		/// 插值器类型
		/// </summary>
		public enum Type
		{
			Linear,
			Accelerate,
			Decelerate,
			AccelerateDecelerate,
			Anticipate,
			Overshoot,
			AnticipateOvershoot,
			Bounce,
			Parabolic,
			Sine,

			CustomParams = -1,
			CustomCurve = -2
		}


		// Adjustable Mask Bit Index:
		// Ease In			0
		// Ease Out			1
		// Back In			2
		// Back Out			3
		static readonly Func<float, float, float>[] _adjustableInterpolators =
		{
			// Bit Index				// 3210
			Symmetric,					// 0000
			EaseIn,						// 0001
			EaseOut,					// 0010
			EaseInOutSymmetric,			// 0011
			Asymmetric,					// 0100
			EaseIn,						// 0101
			EaseOut,					// 0110
			EaseInOutAsymmetric,		// 0111
			Asymmetric,					// 1000
			EaseIn,						// 1001
			EaseOut,					// 1010
			EaseInOutAsymmetric,		// 1011
			Symmetric,					// 1100
			EaseInBackInOut,			// 1101
			EaseOutBackInOut,			// 1110
			EaseInOutSymmetric			// 1111
		};


		/// <summary>
		/// 计算插值
		/// </summary>
		public float Evaluate(float t)
		{
			switch(_index)
			{
				case -1: return _adjustableInterpolators[_adjustableMask](_adjustableFactor, Asymmetric(_adjustableBalance, t));
				case -2: return _curve.Evaluate(t);
				default: return _builtInInterpolators[_index](t);
			}
		}

	} // class Interpolator

} // namespace WhiteCat.Tween