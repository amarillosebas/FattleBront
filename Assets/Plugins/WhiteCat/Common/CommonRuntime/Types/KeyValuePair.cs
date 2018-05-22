
namespace WhiteCat
{
	/// <summary>
	/// 键值对
	/// </summary>
	public struct KeyValuePair<TKey, TValue>
	{
		public TKey key;
		public TValue value;


		public KeyValuePair(TKey key)
		{
			this.key = key;
			value = default(TValue);
		}


		public KeyValuePair(TKey key, TValue value)
		{
			this.key = key;
			this.value = value;
		}

	} // struct KeyValuePair

} // namespace WhiteCat