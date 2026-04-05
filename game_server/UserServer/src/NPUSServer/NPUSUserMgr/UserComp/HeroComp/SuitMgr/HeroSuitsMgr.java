package NPUSServer.NPUSUserMgr.UserComp.HeroComp.SuitMgr;

import GS2GC.p002_InitOp.GS2GC_002_012_RetHeroInit;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.Hero.RefHero;
import NPGameRes.Refs.Hero.RefHeroSuit;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroComponent;

import java.util.ArrayList;
import java.util.List;

/**********
 * 大臣套系的管理器
 * 本管理器从属于HeroComp
 */
public class HeroSuitsMgr
{
    private HeroComponent _m_comp;

    //技能列表
    private List<HeroSuitInfo> _m_lHaloList;

    public HeroSuitsMgr(HeroComponent _comp)
    {
        _m_comp = _comp;

        _m_lHaloList = new ArrayList<>();

        //初始化技能等级信息
        _initSuitInfo();
    }

    public HeroComponent getComp()
    {
        return _m_comp;
    }

    /**************
     * 初始化皮肤信息，主要是放入默认皮肤数据
     */
    private void _initSuitInfo()
    {
        //数据非法判断
        if (null == _m_comp)
            return;

        //遍历大臣基本数据，判断是否有套件，有则加入套件信息
        for (RefHero heroRef : RefHero.getMgr().getList())
        {
            if (heroRef.suit_id != 0)
            {
                //查询数据，如无数据则创建
                if (null != lookupSuitInfo(heroRef.suit_id))
                    continue;

                //查询套件信息
                RefHeroSuit suitRef = RefHeroSuit.getMgr().get(heroRef.suit_id);
                if (null == suitRef)
                {
                    CommLog.error("HeroSuitInfo suitRef not found, suitId:{}", heroRef.suit_id);
                    continue;
                }

                //创建新数据
                HeroSuitInfo suitInfo = new HeroSuitInfo(this, suitRef);
                _m_lHaloList.add(suitInfo);
            }
        }
    }

    /*********
     * 初始化每个套件的属性变化处理对象
     */
    public void _initSuitPropertyChgDealer()
    {
        for (HeroSuitInfo suitInfo : _m_lHaloList)
        {
            suitInfo._initSuitPropertyChgDealer();
        }
    }

    /**
     * 查询星级技能对象
     * @return
     */
    public HeroSuitInfo lookupSuitInfo(long _suitId)
    {
        for (HeroSuitInfo suitInfo : _m_lHaloList)
        {
            if (_suitId == suitInfo.getSuitRef().id)
                return suitInfo;
        }
        return null;
    }

    /**
     * 打印所有套系调试信息
     */
    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("=== HeroSuits totalCount=").append(_m_lHaloList.size()).append(" ===\n");
        for (HeroSuitInfo suitInfo : _m_lHaloList)
        {
            sb.append(suitInfo.toString());
        }
        return sb.toString();
    }

    /**
     * 构造初始化协议
     * @param _proto
     */
    public void makeInitProto(GS2GC_002_012_RetHeroInit _proto)
    {
        for (HeroSuitInfo heroSuitInfo : _m_lHaloList)
        {
            _proto.getSuitList().add(heroSuitInfo.toProto());
        }
    }
}
