package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Guarantee;

import Common.GachaObj.Gacha_GuaranteeInfo;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.AvatarGacha.RefGachaGuarantee;
import NPGameRes.Refs.AvatarGacha.RefGachaItem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.GachaComponent;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.List;

public class GachaGuaranteeList
{
    private List<GachaGuaranteeInfo> _m_guaranteeList;

    public GachaGuaranteeList(List<RefGachaGuarantee> _guaranteeRefList)
    {
        _m_guaranteeList = new ArrayList<>();
        for (RefGachaGuarantee ref : _guaranteeRefList)
        {
            _m_guaranteeList.add(new GachaGuaranteeInfo(ref));
        }
    }

    /**
     * 从数据库中初始化权重信息
     * @param _comp
     * @param _str    数据库中的字符串
     */
    public void initFromDb(GachaComponent _comp, String _str)
    {
        String[] _strList = _str.split(";");
        for (String _item : _strList)
        {
            String[] _itemList = _item.split(":");
            if (_itemList.length != 2)
                continue;

            long guaranteeRuleId = Long.parseLong(_itemList[0]);
            GachaGuaranteeInfo guaranteeInfo = lookupGuaranteeInfo(guaranteeRuleId);
            if (guaranteeInfo == null)
            {
                USLog.error(_comp.getUSServer(), "AvatarGachaGuaranteeList initFromDb failed, guaranteeInfo not find for guaranteeRuleId:{}", guaranteeRuleId);
                continue;
            }

            int guaranteeTimes = Integer.parseInt(_itemList[1]);
            guaranteeInfo.initFromDb(guaranteeTimes);
        }
    }

    private GachaGuaranteeInfo lookupGuaranteeInfo(long _guaranteeRuleId)
    {
        for (GachaGuaranteeInfo _item : _m_guaranteeList)
        {
            if (_item.getGuaranteeRuleId() == _guaranteeRuleId)
                return _item;
        }
        return null;
    }

    /**
     * 获取需要走保底逻辑的保底规则
     * @return 保底规则
     */
    public GachaGuaranteeInfo getNeedOpGuaranteeRule()
    {
        for (GachaGuaranteeInfo guaranteeInfo : _m_guaranteeList)
        {
            if (guaranteeInfo.needOp())
            {
                return guaranteeInfo;
            }
        }
        return null;
    }

    /**
     * 处理保底逻辑
     * @param _rollResultItemRef 抽卡结果
     */
    public void dealRollResult(NPUSUserData _userdata, RefGachaItem _rollResultItemRef)
    {
        for (GachaGuaranteeInfo info : _m_guaranteeList)
        {
            info.dealRollResult(_userdata, _rollResultItemRef);
        }
    }

    /**
     * 获取保底待触发信息
     * @return 保底待触发信息
     */
    public String getPrepareTriggerInfo()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (GachaGuaranteeInfo guaranteeInfo : _m_guaranteeList)
        {
            stringBuilder.append(guaranteeInfo.getGuaranteeRuleId()).append(":").append(guaranteeInfo.getPrepareTriggerTimes()).append(";");
        }
        return stringBuilder.toString();
    }

    @Override
    public String toString()
    {
        return CommonFunc.list2String(_m_guaranteeList);
    }

    public void fillProto(ArrayList<Gacha_GuaranteeInfo> _guaranteeList)
    {
        for (GachaGuaranteeInfo guaranteeInfo : _m_guaranteeList)
        {
            _guaranteeList.add(guaranteeInfo.makeProto());
        }
    }
}
