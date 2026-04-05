package NPCrossGameServer.NPCrossGameCore;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Log.CommLog;
import NPEnum.ENPCrossGameCategoryEnum;

/**
 * 根据类型注册跨服游戏
 * @author mj
 */

@SuppressWarnings("rawtypes")
public class CrossGameCategoryMgr
{
    private static CrossGameCategoryMgr _g_instance = new CrossGameCategoryMgr();

    public static CrossGameCategoryMgr getInstance()
    {
        return _g_instance;
    }

    ////////////////////////////// 实例部分 //////////////////////////////

    //跨服游戏类型数据对象数组
    private _ACrossGameInstanceCategory[] _m_arrGameCategoryArr;

    public CrossGameCategoryMgr()
    {
        _m_arrGameCategoryArr = new _ACrossGameInstanceCategory[ENPCrossGameCategoryEnum.ENPCrossGameCategoryEnum_Length];

        //注册跨服类型
    }

    /**
     * 注册跨服游戏类型
     * @param _category
     */
    public void regCategory(_ACrossGameInstanceCategory _category)
    {
        if (null != _m_arrGameCategoryArr[_category.getCategory().ordinal()])
        {
            CommLog.error("CrossGame Category alread reg, enum:{}", _category.getCategory());
            return;
        }

        _m_arrGameCategoryArr[_category.getCategory().ordinal()] = _category;
    }

    /**
     * 获取对应类型的数据对象
     * @param _category
     * @return
     */
    public _ACrossGameInstanceCategory getCategory(int _category)
    {
        return _m_arrGameCategoryArr[_category];
    }

    /**
     * 初始化所有类别数据
     * @return
     */
    public boolean init()
    {
        for (int i = 0; i < _m_arrGameCategoryArr.length; i++)
        {
            _ACrossGameInstanceCategory obj = _m_arrGameCategoryArr[i];
            if (null == obj)
                continue;

            //数据表数据初始化
            if (!obj.initFromDB())
                return false;

            //初始化子项
            if (!obj.initSub())
                return false;

            //注册协议
            obj.initDealer();

            //注册成功后处理
            obj.initDone();

            //设置初始化完成
            obj.setInited();

            //设置异步tick任务
            ALSynTaskManager.getInstance().regTask(new SynTask_CrossGameCategoryMsgTick(obj));
        }

        return true;
    }

    /**
     * 计算权重
     * @return
     */
    public int calWeight()
    {
        int weight = 0;

        for (int i = 0; i < _m_arrGameCategoryArr.length; i++)
        {
            _ACrossGameInstanceCategory obj = _m_arrGameCategoryArr[i];
            if (null == obj)
                continue;

            weight += obj.calWeight();
        }

        return weight;
    }
}
