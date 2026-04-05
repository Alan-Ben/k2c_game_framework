using System;
using System.Collections.Generic;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    [Serializable]
    public struct JudgeUnionBonusPart
    {
        public EBonusFilterType filterType;
        public long id;
    }
    
    /// <summary>
    /// 从bonusmgr里面获取数据的结构
    /// 可以用heroinfo等写扩展函数直接转成这个结构获取
    /// </summary>
    public class JudgeUnionBonus
    {
        [NotNull]public List<JudgeUnionBonusPart> unionBonusParts = new List<JudgeUnionBonusPart>();
        
        public JudgeUnionBonus(params JudgeUnionBonusPart[] _parts)
        {
            if(null == _parts || _parts.Length == 0)
                return;

            unionBonusParts.AddRange(_parts);
        }
    }
}