using ALPackage;
using Common.LevyEnum;
using NPEnum;
using System;
using Common.BagItemUseEnum;


namespace GOE
{
    /// <summary>
    /// 指定家人指定类型资源数量 CS_CONSORT_RES_COUNT@家人ID@EBagItemUse_ConsortDrawShowType
    /// </summary>
    public class NPPlayerVariable_CS_CONSORT_RES_COUNT : _ANPBasicPlayerVariableObj
    {
        private long _m_lConsortId;//家人ID
        private EBagItemUse_ConsortDrawShowType _m_consortResType;//家人资源类型
        
        protected NPPlayerVariable_CS_CONSORT_RES_COUNT()
        {
        }

        /******************
       * 获取条件类型
       */
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_CONSORT_RES_COUNT; } }

        public override long calPlayerValue(NPVarInfo _variableInfo)
        {
            long count = 0;
#if NP_GAME
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lConsortId);
            
            switch (_m_consortResType)
            {
                case EBagItemUse_ConsortDrawShowType.CHARM:
                    count = consortInfo?.charm ?? 0;
                    break;
                
                case EBagItemUse_ConsortDrawShowType.INTIMACY:
                    count = consortInfo?.intimacy ?? 0;
                    break;
                
                case EBagItemUse_ConsortDrawShowType.CHARM_POINT:
                    count = consortInfo?.charmPoint ?? 0;
                    break;
                
                default:
                    break;
            }
#endif
            return count;
        }

        public static NPPlayerVariable_CS_CONSORT_RES_COUNT readVariable(ALStringReader _reader)
        {
            try
            {
                NPPlayerVariable_CS_CONSORT_RES_COUNT variableObj = new NPPlayerVariable_CS_CONSORT_RES_COUNT();
                string consortIdStr = _reader.readItem('@');
                string resTypeStr = _reader.readItem('@');

                variableObj._m_lConsortId = ALCommon.ParseLong(consortIdStr);
                variableObj._m_consortResType = (EBagItemUse_ConsortDrawShowType)ALCommon.EnumParse(typeof(EBagItemUse_ConsortDrawShowType), resTypeStr, true);
                
                return variableObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("指定家人指定类型资源数量 CS_CONSORT_RES_COUNT@家人ID@EBagItemUse_ConsortDrawShowType Error Str: " + _reader.srcString);
                return null;
            }
        }
    }
}