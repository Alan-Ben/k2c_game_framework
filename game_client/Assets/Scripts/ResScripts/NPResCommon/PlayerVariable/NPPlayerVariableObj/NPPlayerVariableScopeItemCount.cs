using ALPackage;
using NPEnum;
using System;


namespace GOE
{
    public class NPPlayerVariableScopeItemCount : _ANPBasicPlayerVariableObj
    {
        private ENPItemType _m_eItemType;//类型
        private long _m_lStarSubId;
        private long _m_lEndSubId;


        protected NPPlayerVariableScopeItemCount()
        {
        }

        /******************
	   * 获取条件类型
	   */
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_SCOPE_ITEM_COUNT; } }

        public override long calPlayerValue(NPVarInfo _variableInfo)
        {
#if NP_GAME
            long startId;
            long endId;
            long itemCount = 0;
            if (_m_lStarSubId < _m_lEndSubId)
            {
                startId = _m_lStarSubId;
                endId = _m_lEndSubId;
            }
            else
            {
                startId = _m_lEndSubId;
                endId = _m_lStarSubId;
            }

            for (long i = startId; i <= endId; i++)
            {
                itemCount += GCommon.getItemCount(_m_eItemType, i);
            }

            return itemCount;
#else
	        return 0L;
#endif
        }

        public static NPPlayerVariableScopeItemCount readVariable(ALStringReader _reader)
        {
            NPPlayerVariableScopeItemCount variableObj = new NPPlayerVariableScopeItemCount();

            //解析字符串
            string itemTypeS = _reader.readItem('@');
            string subIdS = _reader.readItem('@');

            if (string.IsNullOrEmpty(subIdS))
            {
                UnityEngine.Debug.LogError("高级公式——物品数量 - scope_item_count example: CS_SCOPE_ITEM_COUNT@item_type@startSubIdS:endSubIdS Error Str: " + _reader.srcString);
                return null;
            }

            string[] subIdArr = subIdS.Split(':');
            if (subIdArr.Length != 2)
            {
                UnityEngine.Debug.LogError("高级公式——物品数量 - scope_item_count example: CS_SCOPE_ITEM_COUNT@item_type@startSubIdS:endSubIdS Error Str: " + _reader.srcString);
                return null;
            }

            string startSubIdS = subIdArr[0];
            string endSubIdS = subIdArr[1];
            //逐个判断
            if (null == itemTypeS || null == startSubIdS || null == endSubIdS)
            {
                UnityEngine.Debug.LogError("高级公式——物品数量 - scope_item_count example: CS_SCOPE_ITEM_COUNT@item_type@startSubIdS:endSubIdS Error Str: " + _reader.srcString);
                return null;
            }

            try
            {
                variableObj._m_eItemType = (NPEnum.ENPItemType)ALCommon.EnumParse(typeof(NPEnum.ENPItemType), itemTypeS, true);
                variableObj._m_lStarSubId = long.Parse(startSubIdS);
                variableObj._m_lEndSubId = long.Parse(endSubIdS);

                return variableObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("高级公式——物品数量 - scope_item_count example: CS_SCOPE_ITEM_COUNT@item_type@startSubIdS:endSubIdS Error Str: " + _reader.srcString);
                return null;
            }
        }
    }
}