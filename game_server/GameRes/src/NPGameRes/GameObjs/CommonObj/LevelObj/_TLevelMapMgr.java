package NPGameRes.GameObjs.CommonObj.LevelObj;

import NPCommon.Log.CommLog;

import java.util.Hashtable;

/*************
 * 使用等级映射方式对等级信息进行管理的等级管理器
 * 本管理器要求每个等级都必须有一条数据对应
 */
public class _TLevelMapMgr<T extends _ILevelBasicObj>
{
    /**
     * 存储等级数据的容器
     */
    private Hashtable<Integer, T> _m_map = new Hashtable<Integer, T>();

    public _TLevelMapMgr()
    {
        _m_map = new Hashtable<>();
    }

    /*******
     * 初始化添加等级信息
     * @param _data
     */
    public void _initAddLevelData(T _data)
    {
        if(null == _data)
            return ;

        if(_m_map.containsKey(_data.getLevel()))
        {
            CommLog.error("_TLevelMapMgr _initAddLevelData level data already exist,class:{} id:{} level:{}",_data.getClass().getSimpleName(), _data.getLevel());
            return;
        }

        _m_map.put(_data.getLevel(), _data);
    }

    /*********
     * 清空数据，故意增加下划线前缀避免误调用
     */
    public void _clear()
    {
        _m_map.clear();
    }

    /***********
     * 获取等级对应数据
     * @param _level
     * @return
     */
    public T getLevelData(int _level)
    {
        return _m_map.get(_level);
    }
}
