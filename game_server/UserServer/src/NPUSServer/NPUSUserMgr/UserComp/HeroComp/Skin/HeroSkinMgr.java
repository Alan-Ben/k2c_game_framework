package NPUSServer.NPUSUserMgr.UserComp.HeroComp.Skin;

import Common.HeroObj.Hero_SkinInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Log.CommLog;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Hero.RefHeroSkin;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroComponent;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import USDB.Bo.PlayerHeroSkinBO;
import USLOGDB.Bo.LogHeroSkinBO;

import java.util.ArrayList;
import java.util.List;

/*******
 * 大臣皮肤管理器
 */
public class HeroSkinMgr
{
    private HeroInfo _m_heroInfo;
    private List<HeroSkinInfo> _m_skinList;

    public HeroSkinMgr(HeroInfo _heroInfo)
    {
        _m_heroInfo = _heroInfo;
        _m_skinList = new ArrayList<>();

        //调用初始化处理
        _initSkinInfo();
    }

    /**************
     * 初始化皮肤信息，主要是放入默认皮肤数据
     */
    private void _initSkinInfo()
    {
        //数据非法判断
        if (null == _m_heroInfo || null == _m_heroInfo.getRef())
            return;

        //放入初始默认皮肤数据
        RefHeroSkin skinRef = RefHeroSkin.getMgr().get(_m_heroInfo.getRef().default_skin_id);
        if (null == skinRef)
        {
            CommLog.error("HeroSkinMgr _initSkillInfo skinRef not found, skinId:{}", _m_heroInfo.getRef().default_skin_id);
        } else
        {
            _m_skinList.add(new HeroSkinInfo(this, skinRef));
        }
    }

    public NPUSUserData getUserData()
    {
        return _m_heroInfo.getComp().getUserData();
    }

    public HeroComponent getComp()
    {
        return _m_heroInfo.getComp();
    }

    public List<HeroSkinInfo> getSkinList()
    {
        return _m_skinList;
    }

    /**
     * 数据库初始化技能数据
     * @param _skinBo
     */
    public void initSkin(PlayerHeroSkinBO _skinBo)
    {
        RefHeroSkin refHeroSkin = RefHeroSkin.getMgr().get(_skinBo.getSkinId());
        if (null == refHeroSkin)
        {
            CommLog.error("HeroSkinMgr initSkin refHeroSkin not found, skinId:{}", _skinBo.getSkinId());
            return;
        }

        //创建数据并放入队列
        HeroSkinInfo skinInfo = new HeroSkinInfo(_skinBo, this, refHeroSkin);
        _addSkinToList(skinInfo);
    }

    /**
     * 查找皮肤数据
     * @param _skinId 皮肤id
     * @return 返回皮肤对象
     */
    public HeroSkinInfo lookupSkin(long _skinId)
    {
        _m_heroInfo.lock();
        try
        {
            for (HeroSkinInfo skinInfo : _m_skinList)
            {
                if (skinInfo.getRef().Id() == _skinId)
                    return skinInfo;
            }
            return null;
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 是否拥有皮肤
     * @param _skinId 皮肤id
     * @return 是否拥有皮肤
     */
    public boolean hasSkin(long _skinId)
    {
        return lookupSkin(_skinId) != null;
    }

    /**
     * 解锁皮肤
     * @param _skinId  皮肤id
     * @param _context 上下文
     * @return 结果
     */
    public Result unlockSkin(long _skinId, NPPlayerContext _context)
    {
        //查找配置
        RefHeroSkin ref = RefHeroSkin.getMgr().get(_skinId);
        if (null == ref)
            return CommErr.REF_NOT_FOUND;

        //检查是否已经拥有
        HeroSkinInfo skinInfo = lookupSkin(_skinId);
        if (skinInfo != null)
            return HeroErr.HERO_SKIN_ALREADY_EXIST;

        //尝试消耗解锁道具
        if (!ref.unlock_item.isEmpty() && !getUserData().spendItem(ref.unlock_item, _context))
            return CommErr.CONSUME_FAIL;

        return _gainSkin(_skinId, false, _context).getResult();
    }

    /**
     * 获得皮肤
     * @param _skinId  皮肤id
     * @param _isInit
     * @param _context 上下文
     * @return 获得的皮肤对象
     */
    public ResultOne<HeroSkinInfo> gainSkin(long _skinId, boolean _isInit, NPPlayerContext _context)
    {
        return _gainSkin(_skinId, _isInit, _context);
    }

    /**
     * 构造皮肤数据
     * @param _skinId  皮肤id
     * @param _isInit
     * @param _context 上下文
     * @return 皮肤对象
     */
    private ResultOne<HeroSkinInfo> _gainSkin(long _skinId, boolean _isInit, NPPlayerContext _context)
    {
        _m_heroInfo.lock();
        try
        {
            //查找配置
            RefHeroSkin ref = RefHeroSkin.getMgr().get(_skinId);
            if (null == ref)
                return ResultOne.failed(CommErr.REF_NOT_FOUND);

            //检查是否已经拥有
            HeroSkinInfo skinInfo = lookupSkin(_skinId);
            if (skinInfo != null)
                return ResultOne.failed(HeroErr.HERO_SKIN_ALREADY_EXIST);

            BM bmObj = getUserData().getUSServer().getBM();

            PlayerHeroSkinBO bo = new PlayerHeroSkinBO();
            bo.setCid(bmObj, getUserData().getCid());
            bo.setSkinId(bmObj, _skinId);
            //默认解锁的都是1级
            bo.setSkinLevel(bmObj, 1);
            bo.insert(bmObj);

            //构造数据放入队列
            skinInfo = new HeroSkinInfo(bo, this, ref);
            _addSkinToList(skinInfo);

            //添加皮肤道具
            _context.getCollector().addItem(ENPItemType.HERO_SKIN, _skinId, 1);

            //获取皮肤关联道具
            getUserData().gainItemList(ref.unlock_gain_item_list, _context);

            //解锁皮肤关联资质技能
            _m_heroInfo.getTalentSkillMgr().unlockSkill(ref.unlock_gain_talent_skill_id_list, _context);

            //推送皮肤新增
            if (!_isInit)
            {
                skinInfo.onSkinChg();
            }

            //日志数据
            LogHeroSkinBO logBo = new LogHeroSkinBO();
            logBo.setCid(bmObj, bo.getCid());
            logBo.setSkinId(bmObj, bo.getSkinId());
            logBo.setCurLvl(bmObj, bo.getSkinLevel());
            CommLogDB.log(bmObj, logBo, _context);

            return ResultOne.succ(skinInfo);
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 加入皮肤列表
     * @param _skinInfo 皮肤信息
     */
    private void _addSkinToList(HeroSkinInfo _skinInfo)
    {
        _m_heroInfo.lock();
        try
        {
            _m_skinList.add(_skinInfo);
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 填充对应大臣的皮肤数据
     * @param _skinList 皮肤数据列表
     */
    public void fillSkinProto(ArrayList<Hero_SkinInfo> _skinList)
    {
        _m_heroInfo.lock();
        try
        {
            for (HeroSkinInfo skinInfo : _m_skinList)
            {
                _skinList.add(skinInfo.toProto());
            }
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 升级皮肤
     * @param _skinId  皮肤id
     * @param _context 上下文
     * @return 结果
     */
    public Result upgrade(long _skinId, NPPlayerContext _context)
    {
        _m_heroInfo.lock();
        try
        {
            HeroSkinInfo skinInfo = lookupSkin(_skinId);
            if (skinInfo == null)
                return HeroErr.HERO_SKIN_NOT_EXIST;

            return skinInfo.upgrade(_context);
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    public void removeAll()
    {
        _m_heroInfo.lock();
        try
        {
            for (HeroSkinInfo heroSkinInfo : _m_skinList)
            {
                heroSkinInfo.dispose();
            }
            _m_skinList.clear();
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    @Override
    public String toString()
    {
        _m_heroInfo.lock();
        try
        {
            StringBuilder sb = new StringBuilder();
            for (HeroSkinInfo skinInfo : _m_skinList)
            {
                sb.append(skinInfo.getSkinId()).append(":").append(skinInfo.getSkinLevel()).append("\n");
            }
            return sb.toString();
        } finally
        {
            _m_heroInfo.unlock();
        }
    }
}
