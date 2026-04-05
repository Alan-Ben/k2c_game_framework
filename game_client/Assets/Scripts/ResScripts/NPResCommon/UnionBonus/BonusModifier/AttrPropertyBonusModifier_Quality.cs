using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 指定品质
    /// </summary>
    public class AttrPropertyBonusModifier_Quality : _AAttrPropertyBonusModifier_Enum<EQuality>
    {
        /************
         * 获取加成类型
         * @return
         */
        public override EBonusFilterType getFilterType() { return EBonusFilterType.QUALITY; }
    }
}
