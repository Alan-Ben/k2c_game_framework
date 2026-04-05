using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

/*************
 * 属性修改的管理对象
 **/
public class NPPlayerPropertyChgRecorder
{
    /** 修改的属性列表 */
    private List<ENPPlayerPropertyType> _m_lChgPropertyList;

    /** 属性修改状态标记 */
    private bool[] _m_lPropertyChgStatList;

    public NPPlayerPropertyChgRecorder()
    {
        _m_lChgPropertyList = new List<ENPPlayerPropertyType>();
        _m_lPropertyChgStatList = new bool[NPPlayerPropertyMgr.g_iPropertyCount];

        //逐个设置初始值
        for (int i = 0; i < NPPlayerPropertyMgr.g_iPropertyCount; i++)
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
    public void addPropertyChg(ENPPlayerPropertyType _type)
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
    public ENPPlayerPropertyType popChgProperty()
    {
        ENPPlayerPropertyType chgType;

        int idx = 0;
        try
        {
            while (true)
            {
                if (_m_lChgPropertyList.Count <= idx)
                    return ENPPlayerPropertyType.NONE;

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
