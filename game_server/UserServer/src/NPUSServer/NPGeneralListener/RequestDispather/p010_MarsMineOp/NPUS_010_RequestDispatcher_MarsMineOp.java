package NPUSServer.NPGeneralListener.RequestDispather.p010_MarsMineOp;

import NPUSServer.NPGeneralListener.RequestDispather.NPUSGeneralRequestDispather;

/**
 * 火星矿操作协议分发器初始化
 *
 * 注册火星矿相关跨服请求处理器
 */
public class NPUS_010_RequestDispatcher_MarsMineOp
{
	/**
	 * 初始化并注册所有火星矿操作处理器
	 *
	 * @param _dispather 请求分发器
	 */
	public static void init(NPUSGeneralRequestDispather _dispather)
	{
		//注册获取火星矿数据处理器
		_dispather.regHandler(new NPUSGeneralRequest_010_001_ReqGetMarsMine(_dispather.getUSServer()));

		//注册查询火星矿是否被其他公会攻击过处理器
		_dispather.regHandler(new NPUSGeneralRequest_010_002_ReqGetMarsHadAttackByOtherTag(_dispather.getUSServer()));

		//注册前往占领火星矿处理器
		_dispather.regHandler(new NPUSGeneralRequest_010_003_ReqGoToOccupyMarsMine(_dispather.getUSServer()));

		//注册离开火星矿处理器
		_dispather.regHandler(new NPUSGeneralRequest_010_004_ReqLeaveMarsMine(_dispather.getUSServer()));

		//注册设置火星矿剩余资源数量处理器（GM命令）
		_dispather.regHandler(new NPUSGeneralRequest_010_005_ReqSetMineRemainNum(_dispather.getUSServer()));
	}
}
