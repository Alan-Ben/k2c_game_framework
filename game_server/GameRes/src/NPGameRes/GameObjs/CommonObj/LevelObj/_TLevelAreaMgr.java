package NPGameRes.GameObjs.CommonObj.LevelObj;

import java.util.ArrayList;

/*************
 * 使用等级区域方式对等级信息进行管理的等级管理器
 * 根据数据进行有序排列，每个等级如果无法获取具体等级信息，将会获取最接近的最小等级信息
 */
public class _TLevelAreaMgr<T extends _ILevelBasicObj>
{
    /**
     * 存储等级数据的容器
     */
    private ArrayList<T> _m_levelDataList = new ArrayList<T>();

    public _TLevelAreaMgr()
    {
        _m_levelDataList = new ArrayList<T>();
    }

    /*******
     * 初始化添加等级信息，故意增加下划线前缀避免误调用
     * @param _data
     */
    public void _initAddLevelData(T _data)
    {
        if(null == _data)
            return ;

        //从后往前进行检索，按低到高排序
        T tmpObj = null;
        for(int i = _m_levelDataList.size() - 1; i >= 0; i--)
        {
            tmpObj = _m_levelDataList.get(i);
            if(null == tmpObj)
                continue;

            //判断等级是否比数据低，是则插入
            if(tmpObj.getLevel() < _data.getLevel())
            {
                //位置要加1才是正确位置
                _m_levelDataList.add(i + 1, _data);
                return ;
            }
        }

        //如循环无结果则插入第一个
        _m_levelDataList.add(0, _data);
    }

    /*********
     * 清空数据，故意增加下划线前缀避免误调用
     */
    public void _clear()
    {
        _m_levelDataList.clear();
    }

    /***********
     * 获取等级对应数据
     * @param _level
     * @return
     */
    public T getLevelData(int _level)
    {
        //从前往后检索，当遇到比等级高的则直接返回之前结果
        T tmpObj = null;
        for(int i = 0; i < _m_levelDataList.size(); i++)
        {
            tmpObj = _m_levelDataList.get(i);
            if(null == tmpObj)
                continue;

            if(tmpObj.getLevel() > _level)
            {
                //返回上一个
                return i > 0 ? _m_levelDataList.get(i - 1) : null;
            }
        }

        //直接返回最终值
        return tmpObj;
    }
}
