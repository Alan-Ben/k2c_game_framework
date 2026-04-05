package NPUSServer.NPUSUserMgr.UserComp.TitleComp;

import Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerComboTitleType;
import NPGameRes.Refs.Title.RefPlayerTitlePrefix;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerComboTitleBO;

import java.util.List;

public class ComboTitleUnitPreMgr extends _AComboTitleUnitMgr <ComboTitleUnitPreInfo, PlayerInfo_ComboTitlePre>
{
	public ComboTitleUnitPreMgr(NPUSUserData _userData) 
	{
		super(_userData);
	}

	@Override
	public ENPPlayerComboTitleType getType() 
	{
		return ENPPlayerComboTitleType.PRE;
	}

	@Override
	protected void _initCheck() 
	{
		getUserData().lockUser();
		
		try
		{
			List<RefPlayerTitlePrefix> refList = RefPlayerTitlePrefix.getMgr().getList();

			for(int i = 0; i < refList.size(); i++)
			{
				RefPlayerTitlePrefix ref = refList.get(i);
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
				
					_AComboTitleUnit<PlayerInfo_ComboTitlePre> unit = _createUnit(bo);
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
	protected ComboTitleUnitPreInfo _createUnit(PlayerComboTitleBO _bo) 
	{
		ComboTitleUnitPreInfo info = new ComboTitleUnitPreInfo(getUserData(), _bo);
		
		return info;
	}
	
	@Override
	public ENPItemType getItemType() 
	{
		return ENPItemType.TITLE_PRE;
	}
}
