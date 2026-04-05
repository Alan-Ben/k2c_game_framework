package NPCommon.Property;

import java.util.LinkedList;


public class NPBasicPropertyChgRecorder
{
    /**
     * 修改的属性列表
     */
    private LinkedList<Integer> _m_lChgPropertyList;

    /**
     * 属性修改状态标记
     */
    private boolean[] _m_lPropertyChgStatList;

    public NPBasicPropertyChgRecorder(int _propertyMaxCount)
    {
        _m_lChgPropertyList = new LinkedList<Integer>();
        _m_lPropertyChgStatList = new boolean[_propertyMaxCount];

        //逐个设置初始值
        for (int i = 0; i < _propertyMaxCount; i++)
        {
            _m_lPropertyChgStatList[i] = false;
        }
    }

    /*************
     * 添加修改的属性
     *
     * @author alzq.z
     * @time May 8, 2013 1:51:51 AM
     */
    public void addPropertyChg(int _type)
    {
        synchronized (this)
        {
            //设置属性被修改
            _m_lPropertyChgStatList[_type] = true;

            _m_lChgPropertyList.add(_type);
        }
    }

    /**************
     * 取出修改的属性
     *
     * @author alzq.z
     * @time May 8, 2013 1:53:44 AM
     */
    public int popChgProperty()
    {
        int chgType;

        int idx = 0;
        synchronized (this)
        {
            try
            {
                while (true)
                {
                    if (_m_lChgPropertyList.size() <= idx)
                        return -1;

                    //取出第一个属性对象
                    chgType = _m_lChgPropertyList.get(idx);
                    idx++;

                    if (_m_lPropertyChgStatList[chgType])
                    {
                        //设置未修改
                        _m_lPropertyChgStatList[chgType] = false;

                        return chgType;
                    }
                }
            } finally
            {
                for (int i = 0; i < idx; i++)
                {
                    _m_lChgPropertyList.remove(0);
                }
            }
        }
    }
}