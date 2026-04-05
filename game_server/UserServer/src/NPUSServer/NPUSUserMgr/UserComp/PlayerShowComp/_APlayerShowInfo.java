package NPUSServer.NPUSUserMgr.UserComp.PlayerShowComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPEnum.EPlayerShowEnum;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerShowDataBO;

import java.nio.ByteBuffer;

public abstract class _APlayerShowInfo 
{
	//玩家数据
	private NPUSUserData _m_udUserData;
	//数据实例ID
	private long _m_lId;
	
	protected _APlayerShowInfo(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
	public long getId() {return _m_lId;}

	/**********
	 * 初始化数据
	 * @param _bo
	 */
	protected  void initFromBo(PlayerShowDataBO _bo) 
	{
		_m_lId = _bo.getId();
		
		_initData(_bo);
	}
	
	/**************
	 * 创建更新数据
	 * @param _buff
	 */
	protected void insertOrupdateData(ByteBuffer _buff)
	{
		getUserData().lockUser();
		
		try
		{
			BM bmObj = getUserData().getUSServer().getBM();

			if(_m_lId <= 0)
			{
				PlayerShowDataBO bo = new PlayerShowDataBO();
				bo.setCid(bmObj, getUserData().getCid());
				bo.setType(bmObj, getShowType().ordinal());
				bo.setData(bmObj, CommonFunc.ByteBfferToBytes(_buff));
				bo.insert(bmObj);
				
				_m_lId = bo.getId();
			}
			else
			{
				ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
				updateValue.addValueObj("data", _buff);
				
				bmObj.getBM(PlayerShowDataBO.class).update("id", _m_lId, updateValue);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**********
	 * 移除数据
	 */
	public void delData()
	{
		getUserData().getUSServer().getBM().getBM(PlayerShowDataBO.class).delAll("id", _m_lId);
	}

	/***********
	 * 获取数据类型
	 * @return
	 */
	abstract public EPlayerShowEnum getShowType();
	
	/*************
	 * 初始化数据内容
	 * @param _bo
	 */
	abstract protected void _initData(PlayerShowDataBO _bo);
}
