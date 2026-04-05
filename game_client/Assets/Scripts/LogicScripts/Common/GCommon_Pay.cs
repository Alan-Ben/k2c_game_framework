using CommonEnum;
using NPEnum;
using System;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 请求支付-现金礼包
        /// </summary>
        /// <param name="_giftPackRef"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void reqPay(GiftPackRefObj _giftPackRef, Action _sucDelegate, Action<int, string> _failDelegate)
        {
            if (_giftPackRef == null ||
                _giftPackRef.cost_list == null ||
                _giftPackRef.cost_list.Count == 0 ||
                _giftPackRef.cost_list[0] == null ||
                _giftPackRef.cost_list[0].getItemType() != ENPItemType.PAY)
            {
                _failDelegate?.Invoke(-1, "数据错误");
                return;
            }

            long giftPackId = _giftPackRef.id;

            //客户端先自己增加已购次数，避免重复点击请求
            NPPlayer.instance.giftPackComp.addPendingBuyCount(giftPackId);

            long payRefId = _giftPackRef.cost_list[0].subId;
            string goodsName = TextTranslate.instance.getLanguage(_giftPackRef.name, _giftPackRef.name_args);
            //调用SDK支付接口
            SDKMgr.instance.reqPay(payRefId, giftPackId, goodsName, _sucDelegate, (_code, _msg) =>
            {
                //支付失败或取消，移除待扣减已购次数
                NPPlayer.instance.giftPackComp.removePendingBuyCount(giftPackId);
                _failDelegate?.Invoke(_code, _msg);
            });
        }
    }
}
