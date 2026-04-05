package NPUSServer.Common;

import NPCommon.DDAlert.DDAlert;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/**
 * 道具获取途径监控工具类
 *
 * 主要功能：
 * 1. 检测特定道具是否通过非预期途径获得
 * 2. 对非预期途径进行钉钉报警
 *
 * 设计特点：
 * - 提供统一的监控接口，便于代码复用
 * - 集中管理监控逻辑，易于维护
 *
 * 线程安全：调用方需要自行保证线程安全
 */
public class ItemAcquisitionMonitor
{
    /**
     * 检查并报警：代金券道具非预期途径获取
     *
     * 执行流程：
     * 1. 检查是否为代金券道具（通过RefGeneral配置判断）
     * 2. 检查获取途径是否为合法途径（ORDER_DELIVERY）
     * 3. 如果是代金券且途径非法，触发钉钉报警
     *
     * @param _itemId 道具ID
     * @param _itemCount 本次获得数量
     * @param _newCount 新的总数量
     * @param _context 操作上下文，包含获取途径信息
     * @param _playerData 玩家数据，用于获取玩家CID
     * @param _ddAlert DDAlert实例，用于发送报警
     */
    public static void checkVoucherItemGain(long _itemId, long _itemCount,
                                           long _newCount, NPPlayerContext _context,
                                           NPUSUserData _playerData, DDAlert _ddAlert)
    {
        // 参数校验
        if (_context == null || _playerData == null || _ddAlert == null)
            return;

        // 检查是否为代金券道具，且获取途径不是订单发货
        if (RefGeneral.Ref().voucher_item_bag_item_id == _itemId && _context.getContextId() != ENPGameEvent.ORDER_DELIVERY.ordinal())
        {
            // 触发钉钉报警：玩家通过非预期途径获得代金券
            _ddAlert.warn("VoucherBagItemGain",
                    "player gain voucher bag item by unexpect way, cid:{} itemId:{} count:{} newCount:{} context:{}",
                    _playerData.getCid(), _itemId, _itemCount, _newCount, _context.getContextId());
        }
    }
}
