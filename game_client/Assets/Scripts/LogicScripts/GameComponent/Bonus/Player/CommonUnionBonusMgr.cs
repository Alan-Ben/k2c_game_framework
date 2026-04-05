using System;
using CommonEnum;
using GOE.BonusSpace;

namespace GOE
{
    /// <summary>
    /// 对外的每个系统的属性管理器
    /// </summary>
    public class CommonUnionBonusMgr : _AUnionBonusMgr
    {
        public CommonUnionBonusMgr(EUnionBonusMgrTag _tag) : base(_tag)
        {
        }
        
        //有需要可以单独监听变动回调
        public event Action<EBonusPropertyType> onPropertyChg;
        
        /// <summary>
        /// 属性变动通知
        /// </summary>
        /// <param name="_chgProperty"></param>
        protected internal override void _onPropertyChg(EBonusPropertyType _chgProperty)
        {
            if (onPropertyChg != null) 
                onPropertyChg(_chgProperty);
        }
    }
}