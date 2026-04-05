using ALPackage;
using NPEnum;
using System;


namespace GOE
{
    /// <summary>
    /// 指定家人的羁绊等级 CS_CONSORT_FETTER_LEVLE@家人ID
    /// </summary>
    public class NPPlayerVariable_CS_CONSORT_FETTER_LEVLE : _ANPBasicPlayerVariableObj
    {
        private long _m_lConsortId;//家人ID
        
        protected NPPlayerVariable_CS_CONSORT_FETTER_LEVLE()
        {
        }

        /******************
       * 获取条件类型
       */
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_CONSORT_FETTER_LEVLE; } }

        public override long calPlayerValue(NPVarInfo _variableInfo)
        {
            long fetterLevel = 0;
#if NP_GAME
            // 获取家人信息
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp?.getConsortInfo(_m_lConsortId);
            
            // 空判断: 引用类型需要判空
            if (consortInfo != null && consortInfo.fetterInfo != null)
            {
                fetterLevel = consortInfo.fetterInfo.fetterLevel;
            }
#endif
            return fetterLevel;
        }

        public static NPPlayerVariable_CS_CONSORT_FETTER_LEVLE readVariable(ALStringReader _reader)
        {
            try
            {
                NPPlayerVariable_CS_CONSORT_FETTER_LEVLE variableObj = new NPPlayerVariable_CS_CONSORT_FETTER_LEVLE();
                string consortIdStr = _reader.readItem('@');

                variableObj._m_lConsortId = ALCommon.ParseLong(consortIdStr);
                
                return variableObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("指定家人的羁绊等级 CS_CONSORT_FETTER_LEVLE@家人ID Error Str: " + _reader.srcString);
                return null;
            }
        }
    }
}
