/****************************************************************************
 * Copyright (c) 2017 ~ 2018.8 liangxie
 * 
 * http://qframework.io
 * https://github.com/liangxiegame/QFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


namespace GOE
{
	public static class FuncOrActionOrEventExtension
	{
	    private delegate void TestDelegate();

	    public static void Example()
	    {
	        // action
	        System.Action action = () => Debug.Log("action called");
	        action.InvokeGracefully(); // if (action != null) action();

	        // func
	        Func<int> func = () => 1;
	        func.InvokeGracefully();

	        /*
	        public static T InvokeGracefully<T>(this Func<T> selfFunc)
	        {
	            return null != selfFunc ? selfFunc() : default(T);
	        }
	        */

	        // delegate
	        TestDelegate testDelegate = () => { };
	        testDelegate.InvokeGracefully();
	    }


    #region Func Extension

	    public static T InvokeGracefully<T>(this Func<T> selfFunc)
	    {
	        return null != selfFunc ? selfFunc() : default(T);
	    }

    #endregion

    #region Action

	    /// <summary>
	    /// Call action
	    /// </summary>
	    /// <param name="selfAction"></param>
	    /// <returns> call succeed</returns>
	    public static bool InvokeGracefully(this System.Action selfAction)
	    {
	        if(null != selfAction)
	        {
	            selfAction();
	            return true;
	        }
	        return false;
	    }

	    /// <summary>
	    /// Call action
	    /// </summary>
	    /// <param name="selfAction"></param>
	    /// <typeparam name="T"></typeparam>
	    /// <returns></returns>
	    public static bool InvokeGracefully<T>(this Action<T> selfAction, T t)
	    {
	        if(null != selfAction)
	        {
	            selfAction(t);
	            return true;
	        }
	        return false;
	    }

	    /// <summary>
	    /// Call action
	    /// </summary>
	    /// <param name="selfAction"></param>
	    /// <returns> call succeed</returns>
	    public static bool InvokeGracefully<T, K>(this Action<T, K> selfAction, T t, K k)
	    {
	        if(null != selfAction)
	        {
	            selfAction(t, k);
	            return true;
	        }
	        return false;
	    }

	    /// <summary>
	    /// Call delegate
	    /// </summary>
	    /// <param name="selfAction"></param>
	    /// <returns> call suceed </returns>
	    public static bool InvokeGracefully(this Delegate selfAction, params object[] args)
	    {
	        if(null != selfAction)
	        {
	            selfAction.DynamicInvoke(args);
	            return true;
	        }
	        return false;
	    }

	    /// <summary>
	    /// 打印方便阅读的String
	    /// </summary>
	    /// <param name="selfDelegate"></param>
	    /// <returns></returns>
	    public static string ToReadableString(this Delegate selfDelegate)
	    {
	        if(selfDelegate != null)
	        {
	            string className = selfDelegate.Method.ReflectedType != null ? selfDelegate.Method.ReflectedType.FullName : "null";
	            string targetId = selfDelegate.Target != null ? selfDelegate.Target.GetHashCode().ToString() : "null";
	            return $"{className}  ->  {selfDelegate.Method.Name}  ->  {targetId}";
	        }
	        return "null";
	    }

    #endregion
	}


	public static class IEnumerableExtension
	{
	    public static void Example()
	    {
	        // Array
	        var testArray = new int[] { 1, 2, 3 };
	        testArray.ForEach(number => Debug.Log(number));

	        // IEnumerable<T>
	        IEnumerable<int> testIenumerable = new List<int> { 1, 2, 3 };
	        testIenumerable.ForEach(number => Debug.Log(number));
	        new Dictionary<string, string>()
	            .ForEach(keyValue => Debug.LogFormat("key:{0},value:{1}", keyValue.Key, keyValue.Value));

	        // testList
	        var testList = new List<int> { 1, 2, 3 };
	        testList.ForEach(number => Debug.Log(number));
	        testList.ForEachReverse(number => Debug.Log(number));

	        // merge
	        var dictionary1 = new Dictionary<string, string> { { "1", "2" } };
	        var dictionary2 = new Dictionary<string, string> { { "3", "4" } };
	        var dictionary3 = dictionary1.Merge(dictionary2);
	        dictionary3.ForEach(pair => Debug.LogFormat("{0}:{1}", pair.Key, pair.Value));
	    }

    #region Array Extension

	    /// <summary>
	    /// Fors the each.
	    /// </summary>
	    /// <returns>The each.</returns>
	    /// <param name="selfArray">Self array.</param>
	    /// <param name="action">Action.</param>
	    /// <typeparam name="T">The 1st type parameter.</typeparam>
	    public static T[] ForEach<T>(this T[] selfArray, Action<T> action)
	    {
	        Array.ForEach(selfArray, action);
	        return selfArray;
	    }

	    /// <summary>
	    /// Fors the each.
	    /// </summary>
	    /// <returns>The each.</returns>
	    /// <param name="selfArray">Self array.</param>
	    /// <param name="action">Action.</param>
	    /// <typeparam name="T">The 1st type parameter.</typeparam>
	    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> selfArray, Action<T> action)
	    {
	        if(action == null) throw new ArgumentException();
	        foreach(var item in selfArray)
	        {
	            action(item);
	        }

	        return selfArray;
	    }
	    public static T SafeGet<T>(this T[] _selfList, int _index)
	    {
		    if (_selfList == null)
			    return default;
		    
		    if(_index < 0 || _index >= _selfList.Length)
			    return default;

		    return _selfList[_index];
	    }

    #endregion

    #region List Extension

	    /// <summary>
	    /// Fors the each reverse.
	    /// </summary>
	    /// <returns>The each reverse.</returns>
	    /// <param name="selfList">Self list.</param>
	    /// <param name="action">Action.</param>
	    /// <typeparam name="T">The 1st type parameter.</typeparam>
	    public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T> action)
	    {
	        if(action == null) throw new ArgumentException();

	        for(var i = selfList.Count - 1; i >= 0; --i)
	            action(selfList[i]);

	        return selfList;
	    }

	    /// <summary>
	    /// Fors the each reverse.
	    /// </summary>
	    /// <returns>The each reverse.</returns>
	    /// <param name="selfList">Self list.</param>
	    /// <param name="action">Action.</param>
	    /// <typeparam name="T">The 1st type parameter.</typeparam>
	    public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T, int> action)
	    {
	        if(action == null) throw new ArgumentException();

	        for(var i = selfList.Count - 1; i >= 0; --i)
	            action(selfList[i], i);

	        return selfList;
	    }

	    /// <summary>
	    /// 遍历列表
	    /// </summary>
	    /// <typeparam name="T">列表类型</typeparam>
	    /// <param name="list">目标表</param>
	    /// <param name="action">行为</param>
	    public static void ForEach<T>(this List<T> list, Action<int, T> action)
	    {
	        for(var i = 0; i < list.Count; i++)
	        {
	            action(i, list[i]);
	        }
	    }

	    /// <summary>
	    /// 获得随机列表中元素
	    /// </summary>
	    /// <typeparam name="T">元素类型</typeparam>
	    /// <param name="list">列表</param>
	    /// <returns></returns>
	    public static T GetRandomItem<T>(this List<T> list)
	    {
	        if(list == null || list.Count == 0)
	            return default(T);

	        return list[UnityEngine.Random.Range(0, list.Count)];
	    }

	    /// <summary>
	    /// 获得随机列表中元素，并移除该元素
	    /// </summary>
	    public static T GetRandomItemAndRemove<T>(this List<T> list)
	    {
	        if(list == null || list.Count == 0)
	            return default(T);

	        int randomIndex = UnityEngine.Random.Range(0, list.Count);
	        T result = list[randomIndex];
	        list.RemoveAt(randomIndex);
	        return result;
	    }

	    /// <summary>
	    /// 根据权值来获取索引
	    /// </summary>
	    /// <param name="powers"></param>
	    /// <returns></returns>
	    public static int GetRandomWithPower(this List<int> powers)
	    {
	        var sum = 0;
	        foreach(var power in powers)
	        {
	            sum += power;
	        }

	        var randomNum = UnityEngine.Random.Range(0, sum);
	        var currentSum = 0;
	        for(var i = 0; i < powers.Count; i++)
	        {
	            var nextSum = currentSum + powers[i];
	            if(randomNum >= currentSum && randomNum <= nextSum)
	            {
	                return i;
	            }

	            currentSum = nextSum;
	        }

	        Debug.LogError("权值范围计算错误！");
	        return -1;
	    }

	    /// <summary>
	    /// 优先从 List 的中间取出若干个元素执行 action
	    /// </summary>
	    public static void ActionToMiddleElementSync<T>(this List<T> _list, Action<T, int> _action, int _count)
	    {
		    if (_list == null || _action == null)
			    return;
		    
		    int halfCount = _count / 2;
		    int midIndex = _list.Count / 2;
		    for (int i = 0; i < _count; i++)
		    {
			    T element = _list.SafeGet(midIndex - halfCount + i);
			    if (element == null)
				    return;
			    
			    _action.Invoke(element, i);
		    }
	    }

	    /// <summary>
	    /// 拷贝到
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="from"></param>
	    /// <param name="to"></param>
	    /// <param name="begin"></param>
	    /// <param name="end"></param>
	    public static void CopyTo<T>(this List<T> from, List<T> to, int begin = 0, int end = -1)
	    {
	        if(begin < 0)
	        {
	            begin = 0;
	        }

	        var endIndex = Mathf.Min(from.Count, to.Count) - 1;

	        if(end != -1 && end < endIndex)
	        {
	            endIndex = end;
	        }

	        for(var i = begin; i < end; i++)
	        {
	            to[i] = from[i];
	        }
	    }

	    /// <summary>
	    /// 将List转为Array
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="selfList"></param>
	    /// <returns></returns>
	    public static T[] ToArraySavely<T>(this List<T> selfList)
	    {
	        var res = new T[selfList.Count];

	        for(var i = 0; i < selfList.Count; i++)
	        {
	            res[i] = selfList[i];
	        }

	        return res;
	    }

	    /// <summary>
	    /// 尝试获取，如果没有该数则返回null
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="selfList"></param>
	    /// <param name="index"></param>
	    /// <returns></returns>
	    public static T TryGet<T>(this List<T> selfList, int index)
	    {
	        return selfList.Count > index ? selfList[index] : default(T);
	    }

	    public static void Remove<T>(this List<T> selfList, Func<T, bool> _removeCheck)
	    {
		    if (selfList == null || _removeCheck == null)
			    return;
		    
	        for(int i = 0; i < selfList.Count; i++)
	        {
	            if(_removeCheck(selfList[i]))
	            {
	                selfList.RemoveAt(i);
	                i--;
	            }
	        }
	    }

	    public static string ToStringList<T>(this List<T> _selfList, string _countFormat = "{0}:[", string _itemSplit = ", ", string _endString = "]")
	    {
	        if(null == _selfList)
	            return "Null";
	        StringBuilder builder = new StringBuilder();
	        builder.Append(string.Format(_countFormat, _selfList.Count));
	        for(int i = 0; i < _selfList.Count; i++)
	        {
	            builder.Append(_selfList[i]); // Append string to StringBuilder
	            if(i < _selfList.Count - 1)
	            {
	                builder.Append(_itemSplit);
	            }
	        }
	        builder.Append(_endString);
	        return builder.ToString();
	    }

	    public static string ToStringList<T>(this T[] _selfArray, string _itemSplit = ", ")
	    {
	        if(null == _selfArray)
	            return "Null";
	        StringBuilder builder = new StringBuilder();
	        builder.Append($"{_selfArray.Length}:[");
	        for(int i = 0; i < _selfArray.Length; i++)
	        {
	            builder.Append(_selfArray[i]); // Append string to StringBuilder
	            if(i < _selfArray.Length - 1)
	            {
	                builder.Append(_itemSplit);
	            }
	        }
	        builder.Append("]");
	        return builder.ToString();
	    }

	    public static T GetLast<T>(this List<T> _selfList)
	    {
	        if(_selfList.Count > 0)
	            return _selfList[_selfList.Count - 1];

	        return default;
	    }

	    public static T GetLastAndRemove<T>(this List<T> _selfList)
	    {
	        if (_selfList.Count > 0)
	        {
	            T item = _selfList[_selfList.Count - 1];
	            _selfList.RemoveLast();
	            return item;
	        }

	        return default;
	    }

	    public static T GetFirstAndRemove<T>(this List<T> _selfList)
	    {
	        if (_selfList.Count > 0)
	        {
	            T item = _selfList[0];
	            _selfList.RemoveAt(0);
	            return item;
	        }

	        return default;
	    }

	    public static void RemoveLast<T>(this List<T> _selfList)
	    {
	        if(_selfList.Count > 0)
	            _selfList.RemoveAt(_selfList.Count - 1);
	    }

	    public static T GetFirst<T>(this List<T> _selfList)
	    {
		    if(_selfList.Count > 0)
	            return _selfList[0];

	        return default;
	    }

	    public static bool AddIfNotContain<T>(this List<T> _selfList, T _value)
	    {
		    if (_selfList == null || _selfList.Contains(_value))
			    return false;
		 
		    _selfList.Add(_value);
		    return true;
	    }

	    public static T FindAndRemove<T>(this List<T> _selfList, Predicate<T> _finder)
	    {
	        if(_selfList.Count == 0 || _finder == null)
	            return default;

	        for(int i = 0; i < _selfList.Count; i++)
	        {
		        T result = _selfList[i];
	            if(_finder(result))
	            {
	                _selfList.RemoveAt(i);
	                return result;
	            }
	        }
	        return default;
	    }

	    public static void Remove<T>(this List<T> _selfList, List<T> _removeList)
	    {
		    if (_selfList == null || _removeList == null)
			    return;

		    foreach (T element in _removeList)
		    {
			    _selfList.Remove(element);
		    }
	    }

	    public static T GetFirst<T>(this List<T> _selfList, Predicate<T> _filter)
	    {
	        for(int i = 0; i < _selfList.Count; i++)
	        {
	            if(_filter != null && _filter(_selfList[i]))
	                continue;

	            return _selfList[i];
	        }

	        return default(T);
	    }

	    public static T SafeGet<T>(this List<T> _selfList, int _index)
	    {
		    if (_selfList == null)
			    return default;
		    
	        if(_index < 0 || _index >= _selfList.Count)
	            return default;

	        return _selfList[_index];
	    }

	    public static void SetCount<T>(this List<T> _selfList, int _count)
	    {
		    if (_selfList == null || _selfList.Count == _count)
			    return;
		    
		    int forCount = Mathf.Max(_selfList.Count, _count);
		    int startIndex = Mathf.Min(_selfList.Count, _count);
		    for (int i = startIndex - 1; i < forCount; i++)
		    {
			    if (i >= _selfList.Count)
				    _selfList.Add(default);
			    if (i >= _count)
				    _selfList.RemoveLast();
		    }
	    }

	    public static int indexOfMaxValue(this List<int> _selfList)
	    {
		    int maxValue = int.MinValue;
		    int maxValueIndex = -1;
		    for (int i = 0; i < _selfList.Count; i++)
		    {
			    int listValue = _selfList[i];
			    if (listValue > maxValue)
			    {
				    maxValue = listValue;
				    maxValueIndex = i;
			    }
		    }

		    return maxValueIndex;
	    }
	    public static int indexOfMaxValue(this List<long> _selfList)
	    {
		    long maxValue = long.MinValue;
		    int maxValueIndex = -1;
		    for (int i = 0; i < _selfList.Count; i++)
		    {
			    long listValue = _selfList[i];
			    if (listValue > maxValue)
			    {
				    maxValue = listValue;
				    maxValueIndex = i;
			    }
		    }

		    return maxValueIndex;
	    }
		/// <summary>
		/// 二分查找，返回小于等于 _value 的最大索引
		/// </summary>
		/// <remarks>
		/// 列表必须要是有序的
		/// </remarks>
	    public static int binarySearchFloor<T>(this List<T> _list, T _value) 
		    where T : IComparable<T>
	    {
		    int left = 0, right = _list.Count - 1;
		    int result = -1;
		    while (left <= right)
		    {
			    int mid = (left + right) / 2;
			    if (_list[mid].CompareTo(_value) <= 0)
			    {
				    result = mid;
				    left = mid + 1;
			    }
			    else
			    {
				    right = mid - 1;
			    }
		    }
		    return result;
	    }
		/// <summary>
		/// 二分查找，返回小于等于 _value 的最大索引
		/// </summary>
		/// <remarks>
		/// 列表必须要是有序的
		/// </remarks>
		public static int binarySearchFloor<T, TKey>(this List<T> _list, TKey _value, Func<T, TKey> _keySelector, IComparer<TKey> _comparer = null)
		{
			if (_keySelector == null)
				return -1;
			
			_comparer ??= Comparer<TKey>.Default;

			int left = 0, right = _list.Count - 1;
			int result = -1;

			while (left <= right)
			{
				int mid = (left + right) / 2;
				TKey key = _keySelector(_list[mid]);

				if (_comparer.Compare(key, _value) <= 0)
				{
					result = mid;
					left = mid + 1;
				}
				else
				{
					right = mid - 1;
				}
			}

			return result;
		}

    #endregion

    #region Dictionary Extension

	    /// <summary>
	    /// Merge the specified dictionary and dictionaries.
	    /// </summary>
	    /// <returns>The merge.</returns>
	    /// <param name="dictionary">Dictionary.</param>
	    /// <param name="dictionaries">Dictionaries.</param>
	    /// <typeparam name="TKey">The 1st type parameter.</typeparam>
	    /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
	    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
	        params Dictionary<TKey, TValue>[] dictionaries)
	    {
	        return dictionaries.Aggregate(dictionary,
	            (current, dict) => current.Union(dict).ToDictionary(kv => kv.Key, kv => kv.Value));
	    }

	    /// <summary>
	    /// 根据权值获取值，Key为值，Value为权值
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="powersDict"></param>
	    /// <returns></returns>
	    public static T GetRandomWithPower<T>(this Dictionary<T, int> powersDict)
	    {
	        var keys = new List<T>();
	        var values = new List<int>();

	        foreach(var key in powersDict.Keys)
	        {
	            keys.Add(key);
	            values.Add(powersDict[key]);
	        }

	        var finalKeyIndex = values.GetRandomWithPower();
	        return keys[finalKeyIndex];
	    }

	    /// <summary>
	    /// 遍历
	    /// </summary>
	    /// <typeparam name="K"></typeparam>
	    /// <typeparam name="V"></typeparam>
	    /// <param name="dict"></param>
	    /// <param name="action"></param>
	    public static void ForEach<K, V>(this Dictionary<K, V> dict, Action<K, V> action)
	    {
	        if(dict == null)
	        {
	            return;
	        }
	        var dictE = dict.GetEnumerator();

	        while(dictE.MoveNext())
	        {
	            var current = dictE.Current;
	            if(action != null)
	            {
	                action(current.Key, current.Value);
	            }
	        }

	        dictE.Dispose();
	    }

	    /// <summary>
	    /// 向其中添加新的词典
	    /// </summary>
	    /// <typeparam name="K"></typeparam>
	    /// <typeparam name="V"></typeparam>
	    /// <param name="dict"></param>
	    /// <param name="addInDict"></param>
	    /// <param name="isOverride"></param>
	    public static void AddRange<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> addInDict,
	        bool isOverride = false)
	    {
	        if(addInDict == null)
	        {
	            return;
	        }
	        var dictE = addInDict.GetEnumerator();

	        while(dictE.MoveNext())
	        {
	            var current = dictE.Current;
	            if(dict != null && dict.ContainsKey(current.Key))
	            {
	                if(isOverride)
	                    dict[current.Key] = current.Value;
	                continue;
	            }

	            if(dict != null)
	            {
	                dict.Add(current.Key, current.Value);
	            }
	        }

	        dictE.Dispose();
	    }

	    /// <summary>
	    /// 读取特殊list  3:300;4:600;5:1000;6:2000;7:3000;8:40000
	    /// </summary>
	    /// <param name="dict"></param>
	    /// <param name="_str"></param>
	    public static void realList(this Dictionary<long, long> dict, string _str)
	    {
	        if(null == _str || _str.Length <= 0)
	            return;

	        string[] strs = _str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
	        for(int i = 0; i < strs.Length; i++)
	        {
	            string[] str_item = strs[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
	            if(str_item.Length != 2)
	            {
	                continue;
	            }
	            dict.Add(long.Parse(str_item[0]), long.Parse(str_item[1]));
	        }
	    }

	    /// <summary>
	    /// 保证获取到 value，如果没有 value 会自动 new 一个
	    /// </summary>
	    [NotNull]
	    public static Y getValueDefinitely<T, Y>(this Dictionary<T, Y> _dictionary, T _key)
			where Y : new()
	    {
		    if (!_dictionary.TryGetValue(_key, out Y value) || value == null)
		    {
			    value = new Y();
			    _dictionary[_key] = value;
		    }

		    return value;
	    }

    #endregion

    #region Queue Extension
	    public static T SafePeek<T>(this Queue<T> _selfQueue)
	    {
	        if(_selfQueue == null || _selfQueue.Count == 0)
	            return default(T);
	        return _selfQueue.Peek();
	    }
    #endregion
	}


}