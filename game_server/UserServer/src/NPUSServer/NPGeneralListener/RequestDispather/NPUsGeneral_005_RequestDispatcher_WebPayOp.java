package NPUSServer.NPGeneralListener.RequestDispather;

import NPUSServer.NPGeneralListener.RequestDispather.p005_WebPayOp.RequestDealer_NP2US_R_005_001_ReqGetWebPayGoodsList;
import NPUSServer.NPGeneralListener.RequestDispather.p005_WebPayOp.RequestDealer_NP2US_R_005_002_ReqCreateWebPayOrder;
import NPUSServer.NPGeneralListener.RequestDispather.p005_WebPayOp.RequestDealer_NP2US_R_005_003_ReqGetWebPayRoleInfo;

/**
 * WebPay操作协议注册器
 *
 * 注册p005_WebPayOp包下的所有协议处理器
 */
public class NPUsGeneral_005_RequestDispatcher_WebPayOp
{
	/**
	 * 初始化并注册协议处理器
	 *
	 * @param _dispather 请求分发器
	 */
	public static void init(NPUSGeneralRequestDispather _dispather)
	{
		// 注册获取商品列表协议处理器
		_dispather.regHandler(new RequestDealer_NP2US_R_005_001_ReqGetWebPayGoodsList(_dispather.getUSServer()));

		// 注册创建订单协议处理器
		_dispather.regHandler(new RequestDealer_NP2US_R_005_002_ReqCreateWebPayOrder(_dispather.getUSServer()));

		// 注册获取角色信息协议处理器
		_dispather.regHandler(new RequestDealer_NP2US_R_005_003_ReqGetWebPayRoleInfo(_dispather.getUSServer()));
	}
}
