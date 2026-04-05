
using ALPackage;
using NPEnum;

namespace GOE
{
    
    public class NPPlayerCondition_C_ADD_PACK_NEED_DOWNLOAD : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_ADD_PACK_NEED_DOWNLOAD; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_C_ADD_PACK_NEED_DOWNLOAD readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_ADD_PACK_NEED_DOWNLOAD cond = new NPPlayerCondition_C_ADD_PACK_NEED_DOWNLOAD();
            return cond;
        }
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            return AddPackMgr.instance.needDownload();
#else
            return false;
#endif
        }
    }
}
