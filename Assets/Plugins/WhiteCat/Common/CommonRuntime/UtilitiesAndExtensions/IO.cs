using System.Collections.Generic;
using UnityEngine;

namespace WhiteCat
{
	// <summary>
	// IO 工具
	// </summary>
	public struct IO
	{
		///// <summary> 对字节数组加密或解密 </summary>
		///// <param name="key"> 密钥. 同一个密钥使用偶数次可以消除这个密钥对数据的影响 </param>
		///// <param name="array"> 执行加密或解密的数组 </param>
		///// <param name="index"> 数组开始加密或解密的下标 </param>
		///// <param name="count"> 需要加密或解密的字节总数, 非正值表示直到数组尾部 </param>
		///// <returns> 校验码. 如果同一个密钥在两次处理数据之间其他密钥处理次数都是偶数, 那么这两次返回的校验码是相同的. 校验码可以用来判断数据是否被修改 </returns>
		//public static int EncryptDecrypt(uint key, byte[] array, int index = 0, int count = 0)
		//{
		//	Random random = new Random(key);

		//	byte byte0 = (byte)random.Range(0, 256);
		//	byte byte1 = (byte)random.Range(0, 256);
		//	byte byte2 = byte0;
		//	byte byte3 = byte1;

		//	if (count > 0) count += index;
		//	else count = array.Length;

		//	for (int i = index; i < count; i++)
		//	{
		//		byte0 += array[i];
		//		byte1 -= array[i];

		//		array[i] ^= (byte)random.Range(0, 256);

		//		byte2 += array[i];
		//		byte3 -= array[i];
		//	}

		//	if (byte0 > byte2) Swap(ref byte0, ref byte2);
		//	if (byte1 < byte3) Swap(ref byte1, ref byte3);

		//	return (byte0 << 24) | (byte1 << 16) | (byte2 << 8) | (int)byte3;
		//}


		/// <summary>
		/// 将 Vector2 值写入字节数组
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 写入字节数组的开始下标, 操作完成后增加 8 </param>
		/// <param name="value"> 被写入的值 </param>
		public static void Write(IList<byte> buffer, ref int offset, Vector2 value)
		{
			Union8 union = new Union8();

			union.floatValue = value.x;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.y;
			union.WriteFloat(buffer, ref offset);
		}


		/// <summary>
		/// 从字节数组里读取 Vector2
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 从字节数组里开始读取的下标, 操作完成后增加 8 </param>
		/// <returns> 读取的 Vector2 值 </returns>
		public static Vector2 ReadVector2(IList<byte> buffer, ref int offset)
		{
			var value = new Vector2();
			Union8 union = new Union8();

			union.ReadFloat(buffer, ref offset);
			value.x = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.y = union.floatValue;

			return value;
		}


		/// <summary>
		/// 将 Vector3 值写入字节数组
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 写入字节数组的开始下标, 操作完成后增加 12 </param>
		/// <param name="value"> 被写入的值 </param>
		public static void Write(IList<byte> buffer, ref int offset, Vector3 value)
		{
			Union8 union = new Union8();

			union.floatValue = value.x;
            union.WriteFloat(buffer, ref offset);

			union.floatValue = value.y;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.z;
			union.WriteFloat(buffer, ref offset);
		}


		/// <summary>
		/// 从字节数组里读取 Vector3
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 从字节数组里开始读取的下标, 操作完成后增加 12 </param>
		/// <returns> 读取的 Vector3 值 </returns>
		public static Vector3 ReadVector3(IList<byte> buffer, ref int offset)
		{
			var value = new Vector3();
			Union8 union = new Union8();

			union.ReadFloat(buffer, ref offset);
			value.x = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.y = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.z = union.floatValue;

			return value;
        }


		/// <summary>
		/// 将 Vector4 值写入字节数组
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 写入字节数组的开始下标, 操作完成后增加 16 </param>
		/// <param name="value"> 被写入的值 </param>
		public static void Write(IList<byte> buffer, ref int offset, Vector4 value)
		{
			Union8 union = new Union8();

			union.floatValue = value.x;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.y;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.z;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.w;
			union.WriteFloat(buffer, ref offset);
		}


		/// <summary>
		/// 从字节数组里读取 Vector4
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 从字节数组里开始读取的下标, 操作完成后增加 16 </param>
		/// <returns> 读取的 Vector4 值 </returns>
		public static Vector4 ReadVector4(IList<byte> buffer, ref int offset)
		{
			var value = new Vector4();
			Union8 union = new Union8();

			union.ReadFloat(buffer, ref offset);
			value.x = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.y = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.z = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.w = union.floatValue;

			return value;
		}


		/// <summary>
		/// 将 Quaternion 值写入字节数组
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 写入字节数组的开始下标, 操作完成后增加 16 </param>
		/// <param name="value"> 被写入的值 </param>
		public static void Write(IList<byte> buffer, ref int offset, Quaternion value)
		{
			Union8 union = new Union8();

			union.floatValue = value.x;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.y;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.z;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.w;
			union.WriteFloat(buffer, ref offset);
		}


		/// <summary>
		/// 从字节数组里读取 Quaternion
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 从字节数组里开始读取的下标, 操作完成后增加 16 </param>
		/// <returns> 读取的 Quaternion 值 </returns>
		public static Quaternion ReadQuaternion(IList<byte> buffer, ref int offset)
		{
			var value = new Quaternion();
			Union8 union = new Union8();

			union.ReadFloat(buffer, ref offset);
			value.x = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.y = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.z = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.w = union.floatValue;

			return value;
		}


		/// <summary>
		/// 将 Color 值写入字节数组
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 写入字节数组的开始下标, 操作完成后增加 16 </param>
		/// <param name="value"> 被写入的值 </param>
		public static void Write(IList<byte> buffer, ref int offset, Color value)
		{
			Union8 union = new Union8();

			union.floatValue = value.r;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.g;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.b;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.a;
			union.WriteFloat(buffer, ref offset);
		}


		/// <summary>
		/// 从字节数组里读取 Color
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 从字节数组里开始读取的下标, 操作完成后增加 16 </param>
		/// <returns> 读取的 Color 值 </returns>
		public static Color ReadColor(IList<byte> buffer, ref int offset)
		{
			var value = new Color();
			Union8 union = new Union8();

			union.ReadFloat(buffer, ref offset);
			value.r = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.g = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.b = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.a = union.floatValue;

			return value;
		}


		/// <summary>
		/// 将 Rect 值写入字节数组
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 写入字节数组的开始下标, 操作完成后增加 16 </param>
		/// <param name="value"> 被写入的值 </param>
		public static void Write(IList<byte> buffer, ref int offset, Rect value)
		{
			Union8 union = new Union8();

			union.floatValue = value.x;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.y;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.width;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = value.height;
			union.WriteFloat(buffer, ref offset);
		}


		/// <summary>
		/// 从字节数组里读取 Rect
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 从字节数组里开始读取的下标, 操作完成后增加 16 </param>
		/// <returns> 读取的 Rect 值 </returns>
		public static Rect ReadRect(IList<byte> buffer, ref int offset)
		{
			var value = new Rect();
			Union8 union = new Union8();

			union.ReadFloat(buffer, ref offset);
			value.x = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.y = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.width = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			value.height = union.floatValue;

			return value;
		}


		/// <summary>
		/// 将 Bounds 值写入字节数组
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 写入字节数组的开始下标, 操作完成后增加 24 </param>
		/// <param name="value"> 被写入的值 </param>
		public static void Write(IList<byte> buffer, ref int offset, Bounds value)
		{
			Union8 union = new Union8();

			var v3 = value.center;

			union.floatValue = v3.x;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = v3.y;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = v3.z;
			union.WriteFloat(buffer, ref offset);

			v3 = value.size;

			union.floatValue = v3.x;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = v3.y;
			union.WriteFloat(buffer, ref offset);

			union.floatValue = v3.z;
			union.WriteFloat(buffer, ref offset);
		}


		/// <summary>
		/// 从字节数组里读取 Bounds
		/// </summary>
		/// <param name="buffer"> 字节数组 </param>
		/// <param name="offset"> 从字节数组里开始读取的下标, 操作完成后增加 24 </param>
		/// <returns> 读取的 Bounds 值 </returns>
		public static Bounds ReadBounds(IList<byte> buffer, ref int offset)
		{
			Union8 union = new Union8();

			var center = new Vector3();

			union.ReadFloat(buffer, ref offset);
			center.x = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			center.y = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			center.z = union.floatValue;

			var size = new Vector3();

			union.ReadFloat(buffer, ref offset);
			size.x = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			size.y = union.floatValue;

			union.ReadFloat(buffer, ref offset);
			size.z = union.floatValue;

			return new Bounds(center, size);
		}

	} // struct IO

} // namespace WhiteCat