using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
	/// 妃子邀约对话是否不展示CG
	/// </summary>
    public class NPPlayerCondition_C_CONSORT_CALL_DIALOG_NOT_SHOW_CG : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_CONSORT_CALL_DIALOG_NOT_SHOW_CG; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_C_CONSORT_CALL_DIALOG_NOT_SHOW_CG readStr(ALStringReader _reader)
	    {
	        NPPlayerCondition_C_CONSORT_CALL_DIALOG_NOT_SHOW_CG cond = new NPPlayerCondition_C_CONSORT_CALL_DIALOG_NOT_SHOW_CG();
	        return cond;
	    }

	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
            return ConsortUtil.consortCallDialogNotShowCG;
#else
	        return false;
#endif
        }
	}
}