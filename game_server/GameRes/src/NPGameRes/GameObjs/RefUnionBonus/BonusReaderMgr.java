package NPGameRes.GameObjs.RefUnionBonus;

import CommonEnum.EBonusFilterType;
import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.RefUnionBonus.AttrByEnum.*;

import java.lang.reflect.Constructor;
import java.util.HashMap;
import java.util.Map;

/**
 * 加成配置读取策略管理
 */
public class BonusReaderMgr
{
    //////单例的//////
    private static final BonusReaderMgr _s_instance = new BonusReaderMgr();
    //读取策略表
    private final Map<EBonusFilterType, Class<? extends _IBonusReader>> _m_readerMap;

    private BonusReaderMgr()
    {
        _m_readerMap = new HashMap<>();

        //属性加成
        register(EBonusFilterType.NONE, AttrPropertyEnumBonusModifier_None.class);
        register(EBonusFilterType.HERO_ATTR, AttrPropertyEnumBonusModifier_HeroArrt.class);
        register(EBonusFilterType.BUILDING_ATTR, AttrPropertyIdBonusModifier_BuildingAttr.class);
        register(EBonusFilterType.BUILDING_ID, AttrPropertyIdBonusModifier_BuildingId.class);
        register(EBonusFilterType.CONSORT_ID, AttrPropertyIdBonusModifier_ConsortId.class);
        register(EBonusFilterType.STUDENT_SEX, AttrPropertyEnumBonusModifier_StudentSex.class);
        register(EBonusFilterType.STUDENT_ATTR, AttrPropertyEnumBonusModifier_StudentAttr.class);
        register(EBonusFilterType.HERO_ID, AttrPropertyIdBonusModifier_HeroId.class);
        register(EBonusFilterType.QUALITY, AttrPropertyEnumBonusModifier_Quality.class);
        register(EBonusFilterType.TREASURE_HUNT_TREASURE_ID, AttrPropertyIdBonusModifier_TreasureHuntTreasureId.class);
    }

    public static BonusReaderMgr getInstance()
    {
        return _s_instance;
    }

    public void register(EBonusFilterType _type, Class<? extends _IBonusReader> _cls)
    {
        _m_readerMap.put(_type, _cls);
    }

    /**
     * 根据类型创建一个加成读取对象类
     * @param _bonusType 加成类型
     */
    public _IBonusReader createNew(EBonusFilterType _bonusType)
    {
        //按类型选取创建策略
        Class<? extends _IBonusReader> aClass = _m_readerMap.get(_bonusType);

        try
        {
            //调用无参构造函数， _INPBonusReader 必须要有无参构造函数
            Constructor<? extends _IBonusReader> constructor = aClass.getConstructor();
            return constructor.newInstance();
        } catch (Exception e)
        {
            CommLog.error("NPBonusReaderMgr createNew get exception,e"
                    , e);
            return null;
        }
    }
}
