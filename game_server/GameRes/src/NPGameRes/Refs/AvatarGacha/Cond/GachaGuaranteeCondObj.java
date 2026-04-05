package NPGameRes.Refs.AvatarGacha.Cond;

import ALServerLog.ALServerLog;
import Common.ClothesEnum.EAvatarGachaGuaranteeCondType;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.RefData._IParseFromStringable;
import NPGameRes.Refs.AvatarGacha.Cond.Impl.GachaCondition_DontHave;
import NPGameRes.Refs.AvatarGacha.Cond.Impl.GachaCondition_None;
import NPGameRes.Refs.AvatarGacha.Cond.Impl.GachaCondition_Quality;
import NPGameRes.Refs.AvatarGacha.Cond.Impl.GachaCondition_Tag;

import java.util.ArrayList;
import java.util.List;

public class GachaGuaranteeCondObj implements _IParseFromStringable
{
    private List<_IGachaCondition> _m_condList = new ArrayList<>();

    public List<_IGachaCondition> getCondList()
    {
        return _m_condList;
    }

    @Override
    public boolean parseFromString(String _str)
    {
        //构造读取对象
        NPStringReader stringReader = new NPStringReader(_str);
        do
        {
            String singleStr = stringReader.readItem('&');
            if (null != singleStr)
            {
                _IGachaCondition condition = _readConditionStr(singleStr);
                if (condition != null)
                {
                    _m_condList.add(condition);
                }
            }

        } while (!stringReader.isEmpty());

        return true;
    }

    private _IGachaCondition _readConditionStr(String _singleStr)
    {
        NPStringReader stringReader = new NPStringReader(_singleStr);
        //读取类型字符串
        String condTypeS = stringReader.readItem(':');
        if (null == condTypeS)
        {
            ALServerLog.Error("空的条件字符串:" + _singleStr);
            return null;
        }

        EAvatarGachaGuaranteeCondType condType = EAvatarGachaGuaranteeCondType.NONE;
        try
        {
            //读取类型枚举
            condType = EAvatarGachaGuaranteeCondType.valueOf(condTypeS.toUpperCase());
        } catch (Exception e)
        {

        }
        if (condType == EAvatarGachaGuaranteeCondType.NONE)
        {
            ALServerLog.Error("错误的条件类型:" + _singleStr);
            return null;
        }

        return _parseCondition(condType, stringReader);
    }

    /********************
     * 从节点中读取相关信息
     */
    public static _IGachaCondition _parseCondition(EAvatarGachaGuaranteeCondType _conditionType, NPStringReader _reader)
    {
        switch (_conditionType)
        {
            case NONE:
                return new GachaCondition_None();
            case QUALITY:
                return GachaCondition_Quality.readCond(_reader);
            case TAG:
                return GachaCondition_Tag.readCond(_reader);
            case DONT_HAVE:
                return new GachaCondition_DontHave();
            default:
                return new GachaCondition_None();
        }
    }
}
