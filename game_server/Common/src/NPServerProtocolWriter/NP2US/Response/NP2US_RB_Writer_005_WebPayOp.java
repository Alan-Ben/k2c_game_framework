package NPServerProtocolWriter.NP2US.Response;

import Common.ServerObj.ServerObj_WebPayGoodsList;
import Common.ServerObj.ServerObj_WebPayOrderList;
import Common.ServerObj.ServerObj_WebPayRoleInfo;
import NP2US_RB.p005_WebPayOp.NP2US_RB_005_001_RetGetWebPayGoodsList;
import NP2US_RB.p005_WebPayOp.NP2US_RB_005_002_RetCreateWebPayOrder;
import NP2US_RB.p005_WebPayOp.NP2US_RB_005_003_RetGetWebPayRoleInfo;

import java.util.List;

/**
 * WebPay操作响应协议Writer工具类
 *
 * 提供便捷方法构造WebPay相关的响应协议对象
 */
public class NP2US_RB_Writer_005_WebPayOp
{
	/**
	 * 构造商品列表响应
	 *
	 * @param _goodsList 商品列表
	 * @return 商品列表响应协议对象
	 */
	public static NP2US_RB_005_001_RetGetWebPayGoodsList make_001_RetGetWebPayGoodsList(
		ServerObj_WebPayGoodsList _goodsList)
	{
		NP2US_RB_005_001_RetGetWebPayGoodsList proto = new NP2US_RB_005_001_RetGetWebPayGoodsList();
		proto.setGoodsList(_goodsList);
		return proto;
	}

	/**
	 * 构造创建订单成功响应
	 *
	 * @param _uid 用户ID
	 * @param _orderList 订单列表
	 * @return 创建订单响应协议对象
	 */
	public static NP2US_RB_005_002_RetCreateWebPayOrder make_002_RetCreateWebPayOrder(
		String _uid, ServerObj_WebPayOrderList _orderList)
	{
		NP2US_RB_005_002_RetCreateWebPayOrder proto = new NP2US_RB_005_002_RetCreateWebPayOrder();
		proto.setErrCode(0);  // 成功时错误码为0
		proto.setUid(_uid);
		proto.setOrderList(_orderList);
		return proto;
	}

	/**
	 * 构造创建订单失败响应
	 *
	 * @param _errCode 错误码
	 * @param _failedGoodsIds 失败的商品ID列表
	 * @return 创建订单响应协议对象
	 */
	public static NP2US_RB_005_002_RetCreateWebPayOrder make_002_RetCreateWebPayOrderFailed(
		int _errCode, List<Long> _failedGoodsIds)
	{
		NP2US_RB_005_002_RetCreateWebPayOrder proto = new NP2US_RB_005_002_RetCreateWebPayOrder();
		proto.setErrCode(_errCode);
		proto.setUid("");
		if (_failedGoodsIds != null && !_failedGoodsIds.isEmpty())
			proto.getFailedGoodsIds().addAll(_failedGoodsIds);
		return proto;
	}

	/**
	 * 构造角色信息响应
	 *
	 * @param _roleInfo 角色信息
	 * @return 角色信息响应协议对象
	 */
	public static NP2US_RB_005_003_RetGetWebPayRoleInfo make_003_RetGetWebPayRoleInfo(ServerObj_WebPayRoleInfo _roleInfo)
	{
		NP2US_RB_005_003_RetGetWebPayRoleInfo proto = new NP2US_RB_005_003_RetGetWebPayRoleInfo();
		proto.setRoleInfo(_roleInfo);
		return proto;
	}
}
