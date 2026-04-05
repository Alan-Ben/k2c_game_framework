using System;
using System.Collections.Generic;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 骑士加成配置读取策略管理
    /// </summary>
    public class BonusReaderMgr
    {
        private static BonusReaderMgr g_instance = new BonusReaderMgr();

        public static BonusReaderMgr instance
        {
            get
            {
                if (null == g_instance)
                    g_instance = new BonusReaderMgr();

                return g_instance;
            }
        }
        
        //读取策略表
        [NotNull]private Dictionary<EBonusFilterType, Type> _m_readerMap;

        private BonusReaderMgr() 
        {
            _m_readerMap = new Dictionary<EBonusFilterType, Type>();

            //属性加成
            _regist(EBonusFilterType.NONE, typeof(AttrPropertyBonusModifier_None));
            _regist(EBonusFilterType.HERO_ATTR, typeof(AttrPropertyBonusModifier_HeroArrt));
            _regist(EBonusFilterType.BUILDING_ID, typeof(AttrPropertyBonusModifier_BuildingId));
            _regist(EBonusFilterType.BUILDING_ATTR, typeof(AttrPropertyBonusModifier_BuildingAttr));
            _regist(EBonusFilterType.STUDENT_SEX, typeof(AttrPropertyBonusModifier_StudentSex));
            _regist(EBonusFilterType.STUDENT_ATTR, typeof(AttrPropertyBonusModifier_StudentAttr));
            _regist(EBonusFilterType.CONSORT_ID, typeof(AttrPropertyBonusModifier_ConsortId));
            _regist(EBonusFilterType.HERO_ID, typeof(AttrPropertyBonusModifier_HeroId));
            _regist(EBonusFilterType.QUALITY, typeof(AttrPropertyBonusModifier_Quality));
            _regist(EBonusFilterType.TREASURE_HUNT_TREASURE_ID, typeof(AttrPropertyBonusModifier_TreasureHuntTreasureId));
        }

        private void _regist(EBonusFilterType _type, Type _cls) 
        {
            _m_readerMap.Add(_type, _cls);
        }

        /// <summary>
        /// 根据类型创建一个加成读取对象类
        /// </summary>
        /// <param name="_bonusType">加成类型</param>
        /// <returns></returns>
        public _IBonusReader createNew(EBonusFilterType _bonusType) 
        {
            //按类型选取创建策略
            Type type = null;
            _m_readerMap.TryGetValue(_bonusType, out type);
            try 
            {
                return (_IBonusReader)Activator.CreateInstance(type);
            } 
            catch (Exception e) 
            {
                Debug.LogError($"BonusReaderMgr createNew get exception,e: {e}");
                return null;
            }
        }
    }
}