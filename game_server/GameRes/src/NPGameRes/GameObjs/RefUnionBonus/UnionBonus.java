package NPGameRes.GameObjs.RefUnionBonus;

import CommonEnum.EBonusFilterType;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;

import java.util.ArrayList;

/**
 * 加成配置混合读取容器
 */
public class UnionBonus implements _IParseFromStringable
{
    private final ArrayList<_IBonusReader> _m_bonusList;

    public UnionBonus()
    {
        this._m_bonusList = new ArrayList<>();
    }

    //region get&&set

    public ArrayList<_IBonusReader> getBonusReaderList()
    {
        return _m_bonusList;
    }

    //endregion
    @Override
    public boolean parseFromString(String sValue)
    {
        if (sValue == null || sValue.isEmpty())
            return true;
        readStr(sValue);

        return true;
    }

    /**
     * 读取字符串
     * 加成范围|加成过滤参数|属性枚举：数量；属性枚举：数量#加成范围||加成过滤参数|属性枚举：数量；属性枚举：数量
     * 加成范围： EPropBonusType 加成过滤类型
     * @param _sValue 配置字符串
     */
    private void readStr(String _sValue)
    {
        NPStringReader strR = new NPStringReader(_sValue);
        //按 ‘#’ 分割
        String s = strR.readItem('#');
        while (s != null)
        {
            NPStringReader insideStrR = new NPStringReader(s);
            //按'|'分割，获取bonusReader的类型
            String bonusTypeStr = insideStrR.readItem('|');
            if (bonusTypeStr == null)
            {
                s = strR.readItem('#');
                continue;
            }

            EBonusFilterType bonusType = EBonusFilterType.valueOf(bonusTypeStr);
            //获取对应的Reader
            _IBonusReader bonusReader = BonusReaderMgr.getInstance().createNew(bonusType);
            //将‘|’后的内容交由对应类型的Bonus自己解析
            try
            {
                boolean parseSucc = bonusReader.parseFromString(insideStrR.readItem('#'));
                if (!parseSucc)
                {
                    CommLog.error("UnionBonus readStr parse error: str={}", s);
                }
            } catch (Exception e)
            {
                CommLog.error("UnionBonus readStr error: str={}", s, e);
            }

            //非空则加入队列
            if(!bonusReader.getBonusPropertyModifier().isEmpty())
                _m_bonusList.add(bonusReader);

            //next
            s = strR.readItem('#');
        }
    }
}
