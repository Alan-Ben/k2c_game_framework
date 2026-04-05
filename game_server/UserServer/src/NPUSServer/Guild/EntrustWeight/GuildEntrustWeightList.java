package NPUSServer.Guild.EntrustWeight;

import NPCommon.Game.WeightValueList;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Guild.RefGuildEntrustQuality;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.List;

public class GuildEntrustWeightList
{
    private GuildInfo _m_guildInfo;
    private List<GuildEntrustWeightInfo> _m_weightList;

    public GuildEntrustWeightList(GuildInfo _guildInfo)
    {
        _m_guildInfo = _guildInfo;
        _m_weightList = new ArrayList<>();
        RefGuildEntrustQuality.getMgr().getList().forEach(ref -> _m_weightList.add(new GuildEntrustWeightInfo(ref)));
    }

    public GuildEntrustWeightList()
    {
        _m_weightList = new ArrayList<>();
        RefGuildEntrustQuality.getMgr().getList().forEach(ref -> _m_weightList.add(new GuildEntrustWeightInfo(ref)));
    }

    /**
     * 从数据库中初始化权重信息
     * @param _str 数据库中的字符串
     */
    public void initFromDb(String _str)
    {
        String[] _strList = _str.split(";");
        for (String _item : _strList)
        {
            String[] _itemList = _item.split(":");
            if (_itemList.length != 2)
                continue;

            long refId = Long.parseLong(_itemList[0]);

            GuildEntrustWeightInfo weightInfo = lookupWeightInfo(refId);
            if (weightInfo == null)
            {
                if (_m_guildInfo != null)
                {
                    USLog.error(_m_guildInfo.getGuildMgr().getServer(), "GuildEntrustWeightList initFromDb failed, weightInfo not find for guildId:{} refId:{}"
                            , _m_guildInfo.getGuildId(), refId);
                } else
                {
                    USLog.error("GuildEntrustWeightList initFromDb failed, weightInfo not find for refId:{}", refId);
                }
                continue;
            }

            int index = Integer.parseInt(_itemList[1]);
            weightInfo._initIndex(index);
        }
    }

    /**
     * 查找权重信息
     * @param _refId
     * @return 权重信息
     */
    public GuildEntrustWeightInfo lookupWeightInfo(long _refId)
    {
        for (GuildEntrustWeightInfo _item : _m_weightList)
        {
            if (_item.getRefId() == _refId)
                return _item;
        }
        return null;
    }

    /**
     * 随机一个品质
     * @return 品质
     */
    public WeightValueList<RefGuildEntrustQuality> genWeightList()
    {
        //构造权重列表
        WeightValueList<RefGuildEntrustQuality> weightList = new WeightValueList<>();
        for (GuildEntrustWeightInfo weightInfo : _m_weightList)
        {
            weightList.add(weightInfo.getRef(), weightInfo.getWeight());
        }

        //随机一个品质
        return weightList;
    }

    /**
     * 将没有随机到的品质的抽数加1，并重置抽中的品质的抽数
     */
    public void dealRollResult(long _refId)
    {
        for (GuildEntrustWeightInfo weightInfo : _m_weightList)
        {
            if (weightInfo.getRefId() == _refId)
            {
                weightInfo.resetIndex();
                continue;
            }

            weightInfo.incIndex();
        }
    }

    /**
     * 随机配表
     * @return
     */
    public RefGuildEntrustQuality randomRef()
    {
        return genWeightList().random();
    }

    @Override
    public String toString()
    {
        return CommonFunc.list2String(_m_weightList);
    }
}
