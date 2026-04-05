package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Weight;

import NPCommon.Game.WeightQualityValueList;
import NPCommon.Util.CommonFunc;
import NPEnum.EQuality;
import NPGameRes.Refs.AvatarGacha.RefGachaPoolWeight;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.GachaComponent;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.List;

public class GachaWeightList
{
    private List<GachaWeightInfo> _m_weightList;

    public GachaWeightList(List<RefGachaPoolWeight> _weightRefList)
    {
        _m_weightList = new ArrayList<>();
        for (RefGachaPoolWeight ref : _weightRefList)
        {
            _m_weightList.add(new GachaWeightInfo(ref));
        }
    }

    /**
     * 查找对应品质的权重信息
     * @param _quality 品质
     * @return 权重信息
     */
    public GachaWeightInfo lookupWeightInfo(EQuality _quality)
    {
        for (GachaWeightInfo _item : _m_weightList)
        {
            if (_item.getQuality() == _quality)
                return _item;
        }
        return null;
    }

    /**
     * 从数据库中初始化权重信息
     * @param _m_comp
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

            EQuality quality;
            try
            {
                quality = EQuality.valueOf(_itemList[0]);
            } catch (Exception e)
            {
                USLog.error(_comp.getUSServer(), "AvatarGachaWeightList initFromDb failed, quality not find for str:{}", _itemList[0]);
                continue;
            }

            GachaWeightInfo weightInfo = lookupWeightInfo(quality);
            if (weightInfo == null)
            {
                USLog.error(_comp.getUSServer(), "AvatarGachaWeightList initFromDb failed, weightInfo not find for quality:{}", quality);
                continue;
            }

            int rollIndex = Integer.parseInt(_itemList[1]);
            weightInfo._initRollIndex(rollIndex);
        }
    }

    /**
     * 随机一个品质
     * @return 品质
     */
    public WeightQualityValueList genWeightList()
    {
        //构造权重列表
        WeightQualityValueList weightList = new WeightQualityValueList();
        for (GachaWeightInfo weightInfo : _m_weightList)
        {
            weightList.add(weightInfo.getQuality(), weightInfo.getWeight());
        }

        //随机一个品质
        return weightList;
    }

    /**
     * 将没有随机到的品质的抽数加1，并重置抽中的品质的抽数
     */
    public void dealRollResult(EQuality _quality)
    {
        for (GachaWeightInfo weightInfo : _m_weightList)
        {
            if (weightInfo.getQuality() == _quality)
            {
                weightInfo.resetRollIndex();
                continue;
            }

            weightInfo.incRollIndex();
        }
    }

    /**
     * 获取保底待触发信息
     * @return 保底待触发信息
     */
    public String getWeightInfo()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (GachaWeightInfo weightInfo : _m_weightList)
        {
            stringBuilder.append(weightInfo.getQuality()).append(":").append(weightInfo.getWeight()).append(";");
        }
        return stringBuilder.toString();
    }

    @Override
    public String toString()
    {
        return CommonFunc.list2String(_m_weightList);
    }
}
