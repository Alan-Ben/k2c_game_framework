package NPUSServer.NPUSUserMgr.UserComp.TitleComp;

import Common.NpPlayerInfoObj.PlayerInfo_Title;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Title.RefPlayerTitle;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerTitleBO;

import java.util.ArrayList;
import java.util.List;


/****************************
 * 用户称号组件
 * @author Administrator
 *
 */
public class PlayerTitleComponent extends _AExpiredItemComponent<RefPlayerTitle, PlayerTitleInfo>
{
    public PlayerTitleComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.TITLE_COMP);
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerTitleBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerTitleBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Title Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerTitleBO> _list)
            {
                _initBo(_list);
            }
        });
    }
    //初始化数据
    private void _initBo(List<PlayerTitleBO> _list)
    {
        //获取当前时间
        for (int i = 0; i < _list.size(); i++)
        {
            PlayerTitleBO bo = _list.get(i);
            if (null == bo)
                continue;

            RefPlayerTitle ref = RefPlayerTitle.getMgr().get(bo.getTitleId());
            if (null == ref)
            {
                USLog.error(getUserData().getUSServer(), "Can not get title ref for bo[id:" + bo.getId() + ", refid:" + bo.getTitleId() + "]");
                continue;
            }

            PlayerTitleInfo info = new PlayerTitleInfo(getUserData(), ref, bo);
            _addItemToList(info);
        }

        setInited();
    }

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    }

	@Override
	public ENPItemType getItemType() 
	{
		return ENPItemType.TITLE;
	}

	@Override
	public RefPlayerTitle lookupRef(long _refId) 
	{
		return RefPlayerTitle.getMgr().get(_refId);
	}

	@Override
	protected PlayerTitleInfo _createExpiredItem(RefPlayerTitle _ref, int _expiredTimeS) 
	{
        BM bmObj = getUSServer().getBM();

        PlayerTitleBO bo = new PlayerTitleBO();
        bo.setTitleId(bmObj, _ref.id);
        bo.setCid(bmObj, getUserData().getCid());
        bo.setExpireTimeS(bmObj, _expiredTimeS);
        bo.setViewed(bmObj, false);
        bo.setLastGainTs(bmObj, CommonFunc.getNowTimeSec());
        bo.setGainCount(bmObj, 1);
        bo.insert(bmObj);

        return new PlayerTitleInfo(getUserData(), _ref, bo);
    }

	@Override
	public void onExpiredItemAdd(PlayerTitleInfo _info)
	{	
		getUserData().sendMsgToGC(US2GCWriter_018_PlayerSkinOp.make_050_OnCommTitleChg(_info));
	}

	@Override
	public void onExpiredItemChg(PlayerTitleInfo _info)
	{	
		getUserData().sendMsgToGC(US2GCWriter_018_PlayerSkinOp.make_050_OnCommTitleChg(_info));
	}

	@Override
	public void onExpiredItemDel(long _refId) 
	{
	}

    /////////////////////////////////////// 组件方法 ///////////////////////////////////////
	
    /**
     * 构造返回的协议数据
     * @param _list
     */
    public void makeProtocol(ArrayList<PlayerInfo_Title> _list)
    {
        getUserData().lockUser();
        
        try
        {
            for (PlayerTitleInfo info : getItemList())
            {
                _list.add(info.makeProto());
            }
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }
    
    /**
     * 获取有效称号
     * @param _titleId
     * @return
     */
    public PlayerTitleInfo getEnableTitle(long _titleId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		PlayerTitleInfo info = _lookupItem(_titleId);
    		
    		return info.enable() ? info : null;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
}
