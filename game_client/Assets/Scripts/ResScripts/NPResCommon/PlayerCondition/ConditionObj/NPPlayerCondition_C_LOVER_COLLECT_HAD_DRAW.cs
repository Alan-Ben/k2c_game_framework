using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 情人收集是否已抽取指定情人 C_LOVER_COLLECT_HAD_DRAW:lover_id
    /// lover_id 为 0 时，已领取当前目标情人则条件成立
    /// lover_id 非 0 时，当前选中的情人 == lover_id 且已领取则条件成立
    /// </summary>
    public class NPPlayerCondition_C_LOVER_COLLECT_HAD_DRAW : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_LOVER_COLLECT_HAD_DRAW; } }

        private long _m_loverId;

        public static NPPlayerCondition_C_LOVER_COLLECT_HAD_DRAW readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_LOVER_COLLECT_HAD_DRAW cond = new NPPlayerCondition_C_LOVER_COLLECT_HAD_DRAW();

            string rawLoverId = _reader.readItem();
            if (null == rawLoverId)
            {
                UnityEngine.Debug.LogError($"Can not read str for C_LOVER_COLLECT_HAD_DRAW str:{_reader.srcString}");
                return null;
            }

            cond._m_loverId = long.Parse(rawLoverId);
            return cond;
        }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            LoverCollectComponent comp = NPPlayer.instance.loverCollectComp;
            if (!comp.isClaimed)
                return false;
            if (_m_loverId == 0)
                return true;
            return comp.targetLoverId == _m_loverId;
#else
            return false;
#endif
        }
    }
}
