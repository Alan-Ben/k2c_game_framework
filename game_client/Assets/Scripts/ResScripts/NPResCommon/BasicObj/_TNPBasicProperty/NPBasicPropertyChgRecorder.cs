using System.Collections.Generic;

namespace GOE
{
	/*************
	 * 属性修改的管理对象
	 **/
	public class NPBasicPropertyChgRecorder
	{
	    /** 修改的属性列表 */
	    private List<int> _m_lChgPropertyList;

	    /** 属性修改状态标记 */
	    private bool[] _m_lPropertyChgStatList;

	    public NPBasicPropertyChgRecorder(int _propertyMaxCount)
	    {
	        _m_lChgPropertyList = new List<int>();
	        _m_lPropertyChgStatList = new bool[_propertyMaxCount];

	        //逐个设置初始值
	        for (int i = 0; i < _propertyMaxCount; i++)
	        {
	            //初始化状态值
	            _m_lPropertyChgStatList[i] = false;
	        }
	    }

	    /*************
	     * 添加修改的属性
	     * 
	     * @author alzq.z
	     * @time   May 8, 2013 1:51:51 AM
	     */
	    public void addPropertyChg(int _type)
	    {
	        //设置属性被修改
	        _m_lPropertyChgStatList[(int)_type] = true;

	        //增加变更类型
	        _m_lChgPropertyList.Add(_type);
	    }

	    /**************
	     * 取出修改的属性
	     * 
	     * @author alzq.z
	     * @time   May 8, 2013 1:53:44 AM
	     */
	    public int popChgProperty()
	    {
	        int chgType = -1;

	        int idx = 0;
	        try
	        {
	            while (true)
	            {
	                if (_m_lChgPropertyList.Count <= idx)
	                    return -1;

	                //取出第一个属性对象
	                chgType = _m_lChgPropertyList[idx];
	                idx++;

	                if (_m_lPropertyChgStatList[(int)chgType])
	                {
	                    //设置未修改
	                    _m_lPropertyChgStatList[(int)chgType] = false;

	                    return chgType;
	                }
	            }
	        }
	        finally
	        {
	            _m_lChgPropertyList.RemoveRange(0, idx);
	        }
	    }
	}
}