using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 充值返利组步骤容器
    /// </summary>
    public class GGUIWndRechargeRebatePageStepContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoRechargeRebatePageStepContainerItem, GGUIMonoRechargeRebatePageStepContainer, GGUIWndRechargeRebatePageStepContainerItem>
    {
        //item列表
        protected List<GGUIWndRechargeRebatePageStepContainerItem> _m_lItemList;

        public GGUIWndRechargeRebatePageStepContainer(GGUIMonoRechargeRebatePageStepContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndRechargeRebatePageStepContainerItem _createItemWnd(GGUIMonoRechargeRebatePageStepContainerItem _itemMono)
        {
            GGUIWndRechargeRebatePageStepContainerItem item = new GGUIWndRechargeRebatePageStepContainerItem(_itemMono);
            return item;
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndRechargeRebatePageStepContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(RechargeRebateInfo _info)
        {
            if (_info == null || _m_lItemList == null)
                return;

            List<RechargeRebateStepRefObj> stepList = _info.getStepListBySort();
            if (stepList == null)
                return;

            GGUIWndRechargeRebatePageStepContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < stepList.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];
                itemWnd.showWnd();
                itemWnd.setInfo(stepList[i], _info);
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }

            _refreshContentLayout();
        }

        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;

                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)wnd.itemContainer.transform);
            });
        }
    }
}
