using UnityEngine;


namespace GOE
{
	/// <summary>
	/// 浮点范围参数
	/// 一般用于Mono定义变量
	/// </summary>
	[System.Serializable]
	public class ComFloatRange
	{
	    public float min;
	    public float max;

	    public ComFloatRange()
	    {
	        min = 0f;
	        max = 0f;
	    }
	    public ComFloatRange(float _min, float _max)
	    {
	        min = _min;
	        max = _max;
	    }

	    /// <summary>
	    /// 获取范围内随机数
	    /// </summary>
	    /// <returns></returns>
	    public float rndValue()
	    {
	        return Random.Range(min, max);
	    }

	    /// <summary>
	    /// 判断是否在区域内
	    /// </summary>
	    /// <param name="_v"></param>
	    /// <returns></returns>
	    public bool isInRange(float _v)
	    {
		    if (_v < min)
			    return false;

		    if (_v > max)
			    return false;

		    return true;
	    }

	    /// <summary>
	    /// 限制在区域内
	    /// </summary>
	    /// <param name="_v"></param>
	    /// <returns></returns>
	    public float limitInRange(float _v)
	    {
		    if (_v < min)
			    return min;

		    if (_v > max)
			    return max;

		    return _v;
	    }
	}
}