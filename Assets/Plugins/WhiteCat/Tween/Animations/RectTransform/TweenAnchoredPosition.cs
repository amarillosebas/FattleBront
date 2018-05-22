using UnityEngine;

namespace WhiteCat.Tween
{
	/// <summary>
	/// 相对于锚点的位置插值动画
	/// </summary>
	[AddComponentMenu("White Cat/Tween/Rect Transform/Tween Anchored Position")]
	[RequireComponent(typeof(RectTransform))]
	public class TweenAnchoredPosition : TweenVector2
	{
		public override Vector2 current
		{
			get { return rectTransform.anchoredPosition; }
		}


        public override void SetRawValue(Vector2 value)
        {
            rectTransform.anchoredPosition = value;
        }

    } // class TweenAnchoredPosition

} // namespace WhiteCat.Tween