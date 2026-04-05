using ALPackage;
using CommonEnum;

namespace GOE.BonusSpace
{
    /// <summary>
    /// 英雄重新计算实力的计算处理任务
    /// </summary>
    public class SyncUnionBonusMgrPropertyChgTask : _IALBaseMonoTask
    {
        private _AUnionBonusMgr _m_mgr;
        //变化的属性类型
        private EBonusPropertyType _m_eChgPropertyType;

        public SyncUnionBonusMgrPropertyChgTask(_AUnionBonusMgr _mgr, EBonusPropertyType _propertType)
        {
            _m_mgr = _mgr;
            _m_eChgPropertyType = _propertType;
        }

        public void deal()
        {
            //调用函数处理
            if (_m_mgr != null) 
                _m_mgr._onPropertyChg(_m_eChgPropertyType);
        }
    }
}