package NPServerProtocolWriter.NP2US.Request;

import NP2US_R.p005_WebPayOp.NP2US_R_005_001_ReqGetWebPayGoodsList;
import NP2US_R.p005_WebPayOp.NP2US_R_005_002_ReqCreateWebPayOrder;
import NP2US_R.p005_WebPayOp.NP2US_R_005_003_ReqGetWebPayRoleInfo;

import java.util.List;

/**
 * WebPay操作请求协议Writer工具类
 *
 * 提供便捷方法构造WebPay相关的请求协议对象
 */
public class NP2US_R_Writer_005_WebPayOp
{
	/**
	 * 构造获取商品列表请求
	 *
	 * @param _cid 角色ID
	 * @param _channelCode 渠道标识
	 * @return 商品列表请求协议对象
	 */
	public static NP2US_R_005_001_ReqGetWebPayGoodsList make_001_ReqGetWebPayGoodsList(
		long _cid, String _channelCode)
	{
		NP2US_R_005_001_ReqGetWebPayGoodsList proto = new NP2US_R_005_001_ReqGetWebPayGoodsList();
		proto.setCid(_cid);
		proto.setChannelCode(_channelCode);
		return proto;
	}

	/**
	 * 构造创建订单请求
	 *
	 * @param _cid 角色ID
	 * @param _goodsIdList 商品ID列表
	 * @param _channelCode 渠道标识
	 * @return 创建订单请求协议对象
	 */
	public static NP2US_R_005_002_ReqCreateWebPayOrder make_002_ReqCreateWebPayOrder(
			long _cid, List<Long> _goodsIdList, String _channelCode)
	{
		NP2US_R_005_002_ReqCreateWebPayOrder proto = new NP2US_R_005_002_ReqCreateWebPayOrder();
		proto.setCid(_cid);
		proto.getGoodsIds().addAll(_goodsIdList);
		proto.setChannelCode(_channelCode);
		return proto;
	}

	/**
	 * 构造获取角色信息请求
	 *
	 * @param _cid 角色ID
	 * @return 角色信息请求协议对象
	 */
	public static NP2US_R_005_003_ReqGetWebPayRoleInfo make_003_ReqGetWebPayRoleInfo(long _cid)
	{
		NP2US_R_005_003_ReqGetWebPayRoleInfo proto = new NP2US_R_005_003_ReqGetWebPayRoleInfo();
		proto.setCid(_cid);
		return proto;
	}
}
