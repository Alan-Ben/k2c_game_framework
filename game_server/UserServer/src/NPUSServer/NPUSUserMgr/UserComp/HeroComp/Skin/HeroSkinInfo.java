package NPUSServer.NPUSUserMgr.UserComp.HeroComp.Skin;

import Common.HeroObj.Hero_SkinInfo;
import GS2GC.p013_HeroOp.GS2GC_013_057_OnHeroSkinChg;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.NPLogDB.CommLogDB;
import NPGameRes.Refs.Hero.RefHeroSkin;
import NPGameRes.Refs.Hero.RefHeroSkinLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import USDB.Bo.PlayerHeroSkinBO;
import USLOGDB.Bo.LogHeroSkinBO;

public class HeroSkinInfo
{
    private HeroSkinMgr _m_mgr;

    private PlayerHeroSkinBO _m_bo;
    private RefHeroSkin _m_ref;
    private RefHeroSkinLevel _m_levelRef;

    public HeroSkinInfo(HeroSkinMgr _mgr, RefHeroSkin _ref)
    {
        _m_mgr = _mgr;
        _m_ref = _ref;

        _m_bo = null;
        //默认设置为1级数据
        _m_levelRef = _m_ref.getLevelMapMgr().getLevelData(1);
    }

    public HeroSkinInfo(PlayerHeroSkinBO _bo, HeroSkinMgr _mgr, RefHeroSkin _ref)
    {
        _m_bo = _bo;
        _m_mgr = _mgr;
        _m_ref = _ref;
        _m_levelRef = _m_ref.getLevelMapMgr().getLevelData(_bo.getSkinLevel());

        if (null == _m_levelRef)
            CommLog.error("HeroSkinInfo _initBo levelRef not found, skinId:{}, level:{}", _bo.getSkinId(), _bo.getSkinLevel());
    }

    public HeroSkinMgr getMgr()
    {
        return _m_mgr;
    }

    public PlayerHeroSkinBO getBo()
    {
        return _m_bo;
    }

    public RefHeroSkin getRef()
    {
        return _m_ref;
    }

    public long getSkinId()
    {
        return _m_ref.Id();
    }

    public int getSkinLevel()
    {
        return _m_bo == null ? 1 : _m_bo.getSkinLevel();
    }

    /***********
     * 初始化Bo数据，部分皮肤数据可能会有额外等级信息的时候会调用本函数处理
     * @param _bo
     */
    protected void _initBo(PlayerHeroSkinBO _bo)
    {
        if (null == _bo)
            return;

        _m_bo = _bo;
        //设置等级数据
        _m_levelRef = _m_ref.getLevelMapMgr().getLevelData(_bo.getSkinLevel());
        if (null == _m_levelRef)
            CommLog.error("HeroSkinInfo _initBo levelRef not found, skinId:{}, level:{}", _bo.getSkinId(), _bo.getSkinLevel());
    }

    /**
     * 升级皮肤
     * @param _context 上下文
     * @return 结果
     */
    public Result upgrade(NPPlayerContext _context)
    {
        //获取目标等级配置
        RefHeroSkinLevel refUpgrade = _m_ref.getLevelMapMgr().getLevelData(_m_bo.getSkinLevel() + 1);
        if (refUpgrade == null)
            return CommErr.REF_NOT_FOUND;

        //消耗技能点
        boolean isSuccess = _m_mgr.getUserData().spendItem(_m_levelRef.upgrade_cost, _context);
        if (!isSuccess)
            return CommErr.CONSUME_FAIL;

        //设置数据
        _setLevel(refUpgrade, _context);

        return Result.SUCC;
    }

    /**
     * 设置皮肤等级
     * @param _levelRef 目标等级
     * @param _context  上下文
     */
    private void _setLevel(RefHeroSkinLevel _levelRef, NPPlayerContext _context)
    {
        if (null == _levelRef)
            return;

        _m_levelRef = _levelRef;

        BM bmObj = _m_mgr.getComp().getUSServer().getBM();
        //判断是否有Bo，如无Bo则生成一个，如有Bo则直接保存
        int oriLevel = 1;
        if (null == _m_bo)
        {
            _m_bo = new PlayerHeroSkinBO();

            _m_bo.setCid(bmObj, getMgr().getUserData().getCid());
            _m_bo.setSkinId(bmObj, _m_ref.id);
            _m_bo.setSkinLevel(bmObj, _m_levelRef.skin_level);
            _m_bo.insert(bmObj);
        } else
        {
            oriLevel = _m_bo.getSkinLevel();
            //保存等级
            _m_bo.saveSkinLevel(bmObj, _m_levelRef.skin_level);
        }

        //推送皮肤变更
        onSkinChg();

        //日志数据
        LogHeroSkinBO logBo = new LogHeroSkinBO();
        logBo.setCid(bmObj, _m_bo.getCid());
        logBo.setSkinId(bmObj, _m_bo.getSkinId());
        logBo.setOriLvl(bmObj, oriLevel);
        logBo.setCurLvl(bmObj, _m_bo.getSkinLevel());
        CommLogDB.log(bmObj, logBo, _context);
    }

    /**
     * 皮肤变更推送
     */
    public void onSkinChg()
    {
        GS2GC_013_057_OnHeroSkinChg pushProto = new GS2GC_013_057_OnHeroSkinChg();
        pushProto.setHeroId(_m_ref.hero_id);
        pushProto.setSkinInfo(toProto());
        getMgr().getUserData().sendMsgToGC(pushProto);
    }

    public Hero_SkinInfo toProto()
    {
        Hero_SkinInfo proto = new Hero_SkinInfo();
        proto.setSkinId(getSkinId());
        proto.setLevel(getSkinLevel());
        return proto;
    }

    public void dispose()
    {
        _m_bo.del(getMgr().getUserData().getUSServer().getBM());
    }
}
