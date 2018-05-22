using System;
using UnityEngine;

namespace WhiteCat
{
	/// <summary>
	/// 二维范围
	/// </summary>
	[Serializable]
	public struct Bound2
	{
		/// <summary>
		/// x 轴范围
		/// </summary>
		public Bound1 x;


		/// <summary>
		/// y 轴范围
		/// </summary>
		public Bound1 y;


		/// <summary>
		/// 使用两个轴的范围构造二维范围
		/// </summary>
		public Bound2(Bound1 x, Bound1 y)
		{
			this.x = x;
			this.y = y;
		}


		/// <summary>
		/// 使用 min max 顶点构造二维范围
		/// </summary>
		public Bound2(Vector2 min, Vector2 max)
		{
			x.min = min.x;
			x.max = max.x;
			y.min = min.y;
			y.max = max.y;
		}


		/// <summary>
		/// min 顶点
		/// </summary>
		public Vector2 min
		{
			get { return new Vector2(x.min, y.min); }
			set
			{
				x.min = value.x;
				y.min = value.y;
			}
		}


		/// <summary>
		/// max 顶点
		/// </summary>
		public Vector2 max
		{
			get { return new Vector2(x.max, y.max); }
			set
			{
				x.max = value.x;
				y.max = value.y;
			}
		}


		/// <summary>
		/// 大小
		/// 修改大小时维持中心不变
		/// </summary>
		public Vector2 size
		{
			get { return new Vector2(x.size, y.size); }
			set
			{
				x.size = value.x;
				y.size = value.y;
			}
		}


		/// <summary>
		/// 中心
		/// 修改中心时维持大小不变
		/// </summary>
		public Vector2 center
		{
			get { return new Vector2(x.center, y.center); }
			set
			{
				x.center = value.x;
				y.center = value.y;
			}
		}


		/// <summary>
		/// 确保最小最大值关系正确
		/// </summary>
		public void OrderMinMax()
		{
			x.OrderMinMax();
			y.OrderMinMax();
		}


		/// <summary>
		/// 判断是否包含某个点
		/// </summary>
		public bool Contains(Vector2 point)
		{
			return x.Contains(point.x) && y.Contains(point.y);
		}


		/// <summary>
		/// 获取范围内最接近给定点的点
		/// </summary>
		public Vector2 Closest(Vector2 point)
		{
			point.x = x.Closest(point.x);
			point.y = y.Closest(point.y);
			return point;
		}


		/// <summary>
		/// 判断是否与另一范围有交集
		/// </summary>
		public bool Intersects(Bound2 other)
		{
			return x.Intersects(other.x) && y.Intersects(other.y);
		}


		/// <summary>
		/// 获取两个范围的交集
		/// </summary>
		public Bound2 GetIntersection(Bound2 other)
		{
			other.x = x.GetIntersection(other.x);
			other.y = y.GetIntersection(other.y);
			return other;
		}


		/// <summary>
		/// 获取有符号的距离向量. 负值表示目标小于最小值, 正值表示目标大于最大值
		/// </summary>
		public Vector2 SignedDistanceVector(Vector2 point)
		{
			point.x = x.SignedDistance(point.x);
			point.y = y.SignedDistance(point.y);
			return point;
		}


		/// <summary>
		/// 获取点相对于此范围两个轴向上的距离
		/// </summary>
		public Vector2 DistanceVector(Vector2 point)
		{
			point.x = x.Distance(point.x);
			point.y = y.Distance(point.y);
			return point;
		}


		/// <summary>
		/// 获取点相对于范围的距离的平方
		/// </summary>
		public float SqrDistance(Vector2 point)
		{
			return DistanceVector(point).sqrMagnitude;
		}


		/// <summary>
		/// 获取点相对于范围的距离
		/// </summary>
		public float Distance(Vector2 point)
		{
			return DistanceVector(point).magnitude;
		}


		/// <summary>
		/// 扩展以包含指定点
		/// </summary>
		public void Encapsulate(Vector2 point)
		{
			x.Encapsulate(point.x);
			y.Encapsulate(point.y);
		}


		/// <summary>
		/// 扩展以包含指定范围
		/// </summary>
		public void Encapsulate(Bound2 other)
		{
			x.Encapsulate(other.x);
			y.Encapsulate(other.y);
		}


		/// <summary>
		/// 扩展范围, 负值代表收缩
		/// </summary>
		public void Expand(Vector2 delta)
		{
			x.Expand(delta.x);
			y.Expand(delta.y);
		}


		/// <summary>
		/// 各方向等量扩展范围, 负值代表收缩
		/// </summary>
		public void Expand(float delta)
		{
			x.Expand(delta);
			y.Expand(delta);
		}

	} // struct Bound2

} // namespace WhiteCat