package NPGameRes.GameObjs.CommonObj.VarInfo;

import java.util.ArrayList;

public class NPVarInfo
{
    private ArrayList<NPVarObj> _m_lObjList;

    public NPVarInfo()
    {
        _m_lObjList = new ArrayList<>();
    }

    /**************
     * 添加一个数据类型
     **/
    public NPVarObj addObj(int _type, long _value)
    {
        NPVarObj obj = new NPVarObj();
        obj.setInfo(_type, _value);

        _m_lObjList.add(obj);
        return obj;
    }

    public void addInfo(NPVarObj _obj)
    {
        NPVarObj obj = new NPVarObj();
        obj.setInfo(_obj);

        _m_lObjList.add(obj);
    }

    /*************
     * 拷贝对应的数据
     **/
    public void setInfo(NPVarInfo _variableInfo)
    {
        if (null == _variableInfo)
            return;

        for (int i = 0; i < _variableInfo._m_lObjList.size(); i++)
            addInfo(_variableInfo._m_lObjList.get(i));
    }

    /***********
     * 根据类型获取对应数值
     **/
    public long getValue(int _type)
    {
        for (int i = 0; i < _m_lObjList.size(); i++)
            if (_m_lObjList.get(i).type == _type)
                return _m_lObjList.get(i).value;

        return 0;
    }

    /*************
     * 重置列表并放回缓存
     **/
    public void reset()
    {
        _m_lObjList.clear();
    }
}
