package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;

@RefTable(tableName = "quality")
public class RefNPQuality extends RefBase
{
//    public class NPQualityRndRangeInfo
//    {
//        private int _m_iMin;
//        private int _m_iMax;
//        private int _m_iRndRange;
//        private int _m_iWeight;
//
//        public NPQualityRndRangeInfo(String _str)
//        {
//            String[] strings = _str.split("_");
//            if(strings.length < 3)
//            {
//                ALServerLog.Error("NPQualityRndRangeInfo读取格式错误！");
//                return;
//            }
//
//            _m_iMin = Integer.parseInt(strings[0]);
//            _m_iMax = Integer.parseInt(strings[1]);
//            _m_iRndRange = _m_iMax - _m_iMin;
//
//            _m_iWeight = Integer.parseInt(strings[2]);
//        }
//
//        public int getWeight() {return _m_iWeight;}
//
//        /** 获取随机值， min <= x <= max */
//        public int getRndV() {return _m_iMin + ALBasicCommonFun.getRandomInt(_m_iRndRange);}
//    }

    private static RefActorQualityMgr _g_mgr = new RefActorQualityMgr();

    public static RefActorQualityMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefActorQualityMgr extends RefTableContainer<RefNPQuality>
    {
        /*********
         * 初始化操作
         *
         * @author alzq.z
         * @time 2019年6月29日 下午12:04:27
         */
        public void init()
        {
        }

    }

    @Override
    public RefActorQualityMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActorQualityMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefNPQuality newRef = (RefNPQuality) _newRef;
        id = newRef.id;
        quality = newRef.quality;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }

    public int id;//
    public EQuality quality; //品质枚举

//    public int quality_lv_prop_rate;//品质对应卡牌等级属性加成万分比，不加成随机属性，越高品质的属性随机属性总占比越低
//
//    public String rnd_hero_prop_weight;//随机英雄成长值在本品质下的随机区间和概率
//
//    private ArrayList<NPQualityRndRangeInfo> _m_lRndRangeList;
//    private long _m_iRangeTotalWeight;


    public void init()
    {
//        _m_lRndRangeList = new ArrayList<NPQualityRndRangeInfo>();
//        String[] strs = rnd_hero_prop_weight.split(":");
//        for(int i = 0; i < strs.length; i++)
//        {
//            NPQualityRndRangeInfo info = new NPQualityRndRangeInfo(strs[i]);
//            _m_lRndRangeList.add(info);
//            _m_iRangeTotalWeight += info.getWeight();
//        }
    }

    /**************
     * 获取随机值
     *
     * @author alzq.z
     * @time 2019年6月29日 上午11:38:33
     */
//    public int getRndValue()
//    {
//        long rndV = (ALBasicCommonFun.getRandomLong() % _m_iRangeTotalWeight);
//        for(int i = 0; i < _m_lRndRangeList.size(); i++)
//        {
//            rndV -= _m_lRndRangeList.get(i).getWeight();
//            if(rndV <= 0)
//            {
//                return _m_lRndRangeList.get(i).getRndV();
//            }
//        }
//
//        return _m_lRndRangeList.get(_m_lRndRangeList.size() - 1).getRndV();
//    }
}
