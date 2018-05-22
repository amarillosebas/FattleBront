using UnityEngine;

namespace WhiteCat.Tween
{
	/// <summary>
	/// sizeDelta 插值动画
	/// </summary>
	[AddComponentMenu("White Cat/Tween/Rect Transform/Tween Size Delta")]
	[RequireComponent(typeof(RectTransform))]
	public class TweenSizeDelta : TweenVector2
	{
		public override Vector2 current
		{
			get { return rectTransform.sizeDelta; }
		}


        public override void SetRawValue(Vector2 value)
        {
            rectTransform.sizeDelta = value;
        }

    } // class TweenSizeDelta

} // namespace WhiteCat.Tween