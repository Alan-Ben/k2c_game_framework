package NPUSServer.NPUSUserMgr.UserComp.TitleComp;

import Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerComboTitleType;
import NPGameRes.Refs.Title.RefPlayerTitleBg;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerComboTitleBO;

import java.util.List;

public class ComboTitleUnitBgMgr extends _AComboTitleUnitMgr <ComboTitleUnitBgInfo, PlayerInfo_ComboTitleBg>
{
	public ComboTitleUnitBgMgr(NPUSUserData _userData) 
	{
		super(_userData);
	}

	@Override
	public ENPPlayerComboTitleType getType() 
	{
		return ENPPlayerComboTitleType.BG;
	}

	@Override
	protected void _initCheck() 
	{
		getUserData().lockUser();
		
		try
		{
			List<RefPlayerTitleBg> refList = RefPlayerTitleBg.getMgr().getList();
			
			for(int i = 0; i < refList.size(); i++)
			{
				RefPlayerTitleBg ref = refList.get(i);
				if(null == ref)
					continue;
				
				//尚未拥有且已经满足解锁条件，则自动进行解锁
				if(!hasItem(ref.id, 1) && NPPlayerConditionDealerMgr.IsEnable(ref.unlock_condition, getUserData(), null))
				{
					PlayerComboTitleBO bo = new PlayerComboTitleBO();
					bo.setCid(getUSServer().getBM(), getUserData().getCid());
					bo.setUnitType(getUSServer().getBM(), getType().ordinal());
					bo.setUnitId(getUSServer().getBM(), ref.id);
					bo.insert(getUSServer().getBM());
				
					_AComboTitleUnit<PlayerInfo_ComboTitleBg> unit = _createUnit(bo);
					_m_alComboTitleUnitList.add(unit);
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	protected ComboTitleUnitBgInfo _createUnit(PlayerComboTitleBO _bo) 
	{
		ComboTitleUnitBgInfo info = new ComboTitleUnitBgInfo(getUserData(), _bo);
		
		return info;
	}
	
	@Override
	public ENPItemType getItemType() 
	{
		return ENPItemType.TITLE_BG;
	}
}
