package NPGameRes;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;
import NPGameRes.GameObjs.Battle.*;
import NPGameRes.InitDealer._ABasicInitDealer;
import NPGameRes.Refs.RefGeneral;

import java.util.ArrayList;
import java.util.List;

public class NPGRefdataCoreMgr
{
    //是否已经初始化
    private boolean _m_bHasDealInit;

    private static NPGRefdataCoreMgr _instance = new NPGRefdataCoreMgr();

    public static NPGRefdataCoreMgr getInstance()
    {
        return _instance;
    }

    public static void replaceInstance(NPGRefdataCoreMgr _mgr)
    {
        _instance = _mgr;

    }

    public WCGRefMap_ActorRefObj actorRefCore = new WCGRefMap_ActorRefObj();
    public WCGRefMap_SkillRefObj skillMap = new WCGRefMap_SkillRefObj();
    public WCGRefMap_BuffRefObj buffMap = new WCGRefMap_BuffRefObj();
    public WCGRefMap_EffectRefObj effectMap = new WCGRefMap_EffectRefObj();
    public WCGRefMap_BulletRefObj bulletMap = new WCGRefMap_BulletRefObj();
    public WCGRefMap_MapRefObj mapMap = new WCGRefMap_MapRefObj();
    public WCGRefMap_DungeonRefObj dungeonMap = new WCGRefMap_DungeonRefObj();
    public WCGRefMap_MobRefObj mobRefMap = new WCGRefMap_MobRefObj();
    public WCGRefMap_PosEffectRefObj posEffectMap = new WCGRefMap_PosEffectRefObj();
    public WCGRefMap_TeamSkillObj teamSkillMap = new WCGRefMap_TeamSkillObj();
    //	public WCGRefMap_AddedRewardRefObj addedRewardMap = new WCGRefMap_AddedRewardRefObj();
    public WCGRefMap_TeamConfigObj teamConfigMap = new WCGRefMap_TeamConfigObj();

    public WCGRefMap_ControlStateObj controlStateMap = new WCGRefMap_ControlStateObj();

    /**
     * 初始化
     */
    public synchronized void init()
    {
        //这里不重复进行初始化
        if (_m_bHasDealInit)
            return;

        _m_bHasDealInit = true;

        //初始化所有配置表
        reload();
    }

    /**
     * 重新加载所有配置表
     */
    public void reload()
    {
        teamConfigMap.initData();
        actorRefCore.initData();
        skillMap.initData();
        buffMap.initData();
        effectMap.initData();
        bulletMap.initData();
        mapMap.initData();
        dungeonMap.initData();
        mobRefMap.initData();
        posEffectMap.initData();
        teamSkillMap.initData();
//		shopMainRefObj.initData();
//		shopGoodsRefObj.initData();
//		rewardMap.initData();
//		addedRewardMap.initData();
//		drawCardMap.initData();
//		drawCardStoreMap.initData();
//		activityMap.initData();
//		achieveMap.initData();
//		matchBaseInfoMap.initData();
//		battleHonorInfoMap.initData();
//		growthMap.initData();
        controlStateMap.initData();

        //增加初始化处理，自动通过类检索，检索对应基类同路径下的所有子类并执行
        try
        {
            List<Class<?>> classes = CommClass.getAllClassByInterface(_ABasicInitDealer.class, _ABasicInitDealer.class.getPackage().getName());

            //初始将对象都添加到队列。之后进行优先级排序
            ArrayList<_ABasicInitDealer> dealerList = new ArrayList<_ABasicInitDealer>();
            for (Class<?> cs : classes)
            {
                _ABasicInitDealer newDealer = (_ABasicInitDealer) cs.newInstance();
                if (null != newDealer)
                    dealerList.add(newDealer);
            }

            //排序,大数字的在前
            dealerList.sort((_arg1, _arg2) -> _arg2.getPriority() - _arg1.getPriority());

            //优先处理优先的
            for (_ABasicInitDealer dealer : dealerList)
            {
                if (null != dealer)
                {
                    try
                    {
                        dealer.dealInit();
                    } catch (Exception e)
                    {
                        CommLog.error("deal Init dealer: " + dealer.getClass() + " failed ", e);
                    }
                }
            }

            //清空队列
            dealerList.clear();

        } catch (Exception e)
        {
            CommLog.error("deal Init dealer failed", e);
        }

    }

    /********************************
     * 获取通用配置表信息
     *
     * @return
     */
    public RefGeneral general()
    {
        return RefGeneral.Ref();
    }
}
