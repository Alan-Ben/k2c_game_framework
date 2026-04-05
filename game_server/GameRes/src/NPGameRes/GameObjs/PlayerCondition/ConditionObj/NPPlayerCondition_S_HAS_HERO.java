package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

import java.util.List;

/**
 * 拥有指定大臣中的n个 S_HAS_HERO:hero1&hero2:（n）
 * @author keith
 */
public class NPPlayerCondition_S_HAS_HERO extends _ANPBasicPlayerCondition
{
    private List<Long> _m_heroIdList;
    private int _m_needNum;

    public List<Long> getHeroIdList()
    {
        return _m_heroIdList;
    }

    public int getNeedNum()
    {
        return _m_needNum;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_HAS_HERO;
    }

    public static NPPlayerCondition_S_HAS_HERO readStr(NPStringReader _reader)
    {
        NPPlayerCondition_S_HAS_HERO cond = new NPPlayerCondition_S_HAS_HERO();

        //解析大臣列表
        String rawHeroList = _reader.readItem();
        if (null == rawHeroList)
        {
            CommLog.error("Can not read str for rawHeroList str:{}", _reader.getSrcString());
            return null;
        }
        cond._m_heroIdList = CommonFunc.listLongFromString(rawHeroList, '&');

        //解析需要的数量
        String rawNeedNum = _reader.readItem(':');
        if (null != rawNeedNum)
        {
            cond._m_needNum = Integer.parseInt(rawNeedNum);
        }else
        {
            cond._m_needNum = cond._m_heroIdList.size();
        }

        return cond;
    }
}
