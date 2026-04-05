package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

import java.util.List;

/**
 * 拥有指定妃子中的n个 S_HAS_Consort:Consort1&Consort2:（n）
 */
public class NPPlayerCondition_S_HAS_CONSORT extends _ANPBasicPlayerCondition
{
    private List<Long> _m_consortIdList;
    private int _m_needNum;

    public List<Long> getConsortIdList()
    {
        return _m_consortIdList;
    }

    public int getNeedNum()
    {
        return _m_needNum;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_HAS_CONSORT;
    }

    public static NPPlayerCondition_S_HAS_CONSORT readStr(NPStringReader _reader)
    {
        NPPlayerCondition_S_HAS_CONSORT cond = new NPPlayerCondition_S_HAS_CONSORT();

        //解析妃子列表
        String rawConsortList = _reader.readItem();
        if (null == rawConsortList)
        {
            CommLog.error("Can not read str for rawConsortList str:{}", _reader.getSrcString());
            return null;
        }
        cond._m_consortIdList = CommonFunc.listLongFromString(rawConsortList, '&');

        //解析需要的数量
        String rawNeedNum = _reader.readItem(':');
        if (null != rawNeedNum)
        {
            cond._m_needNum = Integer.parseInt(rawNeedNum);
        }else
        {
            cond._m_needNum = cond._m_consortIdList.size();
        }

        return cond;
    }
}
