using System;
using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 获取物品队列节点
    /// 用于非Notice方式展示获取物品结果, 参考NPNoticeDealer_GetReward写的
    /// </summary>
    public class GNodeGetItem : _AGetItemNode<NPGGUIMonoGetItem, NPGGUIWndGetItem>
    {
        /// <summary>
        /// 创建获取物品节点（带过滤处理）
        /// </summary>
        /// <param name="_itemList">物品列表</param>
        /// <param name="_titleKey">标题key</param>
        /// <param name="_needTransBk">是否需要背景遮罩</param>
        /// <param name="_closeAction">关闭回调</param>
        /// <returns>获取物品节点，若物品列表无效则返回null</returns>
        public static GNodeGetItem makeGetItemNode(List<NPCommon_ItemInfo> _itemList, string _titleKey = TransKeyConst.common_getreward_tip, bool _needTransBk = true, Action _closeAction = null)
        {
            // 过滤掉在弹窗展示奖励时不需要显示的道具
            List<NPCommon_ItemInfo> resList = GCommon.filterPopWndEnableShowItemList(_itemList);
            if (resList == null || resList.Count <= 0)
            {
                return null;
            }

            return new GNodeGetItem(resList, _titleKey, _needTransBk, _closeAction);
        }

        protected GNodeGetItem(List<NPCommon_ItemInfo> _itemList, string _titleKey, bool _needTransBk, Action _closeAction) 
            : base(_itemList, _titleKey, _needTransBk, _closeAction)
        {
        }

        /// <summary>
        /// 窗口实例
        /// </summary>
        protected override NPGGUIWndGetItem _wnd { get { return NPGGUIWndGetItem.instance; } }
    }
}
