using System;
using UnityEngine;

namespace WhiteCat.Tween
{
	/// <summary>
	/// 渐变类型全局光上部 (或单色全局光) 颜色插值动画
	/// </summary>
	[AddComponentMenu("White Cat/Tween/Global/Tween Ambient Sky Color")]
	public class TweenAmbientSkyColor : TweenColor
	{
		public override Color current
		{
			get { return RenderSettings.ambientSkyColor; }
		}


        public override void SetRawValue(Color value)
        {
            RenderSettings.ambientSkyColor = value;
        }

    } // class TweenAmbientSkyColor

} // namespace WhiteCat.Tween