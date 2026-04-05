using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 情人收集是否有指定目标情人 C_LOVER_COLLECT_HAS_TARGET:lover_id
    /// lover_id 为 0 时，未选择任何情人则条件成立
    /// lover_id 非 0 时，当前选中的情人 == lover_id 则条件成立
    /// </summary>
    public class NPPlayerCondition_C_LOVER_COLLECT_HAS_TARGET : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_LOVER_COLLECT_HAS_TARGET; } }

        private long _m_loverId;

        public static NPPlayerCondition_C_LOVER_COLLECT_HAS_TARGET readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_LOVER_COLLECT_HAS_TARGET cond = new NPPlayerCondition_C_LOVER_COLLECT_HAS_TARGET();

            string rawLoverId = _reader.readItem();
            if (null == rawLoverId)
            {
                UnityEngine.Debug.LogError($"Can not read str for C_LOVER_COLLECT_HAS_TARGET str:{_reader.srcString}");
                return null;
            }

            cond._m_loverId = long.Parse(rawLoverId);
            return cond;
        }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            long targetLoverId = NPPlayer.instance.loverCollectComp.targetLoverId;
            if (_m_loverId == 0)
                return targetLoverId == 0;
            return targetLoverId == _m_loverId;
#else
            return false;
#endif
        }
    }
}
