using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基础属性tip随机展示子窗口
    /// </summary>
    public class GGUIWndCommonAttrTipRandomShow : _ATALBasicUISubWnd<GGUIMonoCommonAttrTipRandomShow>
    {
        //基础属性自定义上浮提示展示管理器
        private NPGGUIAttrTipDealerMgr _m_tipMgr;
        //记录选中的父节点下标
        private Dictionary<EBasicAttrType, int> _m_dSelectParentIndex;

        public GGUIWndCommonAttrTipRandomShow(GGUIMonoCommonAttrTipRandomShow _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_tipMgr?.clear();
            _m_dSelectParentIndex?.Clear();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_tipMgr?.discard();
            _m_tipMgr = null;

            _m_dSelectParentIndex?.Clear();
            _m_dSelectParentIndex = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_tipMgr = new NPGGUIAttrTipDealerMgr(wnd.coexistMaxCount, wnd.tipAccMaxRate, wnd.tipAccMinNum, wnd.tipAccMaxNum);
            _m_dSelectParentIndex = new Dictionary<EBasicAttrType, int>();
        }

        /// <summary>
        /// 添加提示
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_value"></param>
        /// <param name="_isDifferentWithLastParent"></param>
        /// <param name="_needDelay">是否需要延时展示</param>
        /// <param name="_delayMultiple">延时倍数</param>
        public void addTip(EBasicAttrType _type, long _value, bool _isDifferentWithLastParent = false, long _delayMultiple = 0)
        {
            if (wnd == null || _m_tipMgr == null)
                return;

            //是否需要延时展示
            if (_delayMultiple > 0)
            {
                float delayTime = wnd.tipDelayShow * _delayMultiple;
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (!isShow)
                        return;

                    _m_tipMgr?.addTip(_type, _isDifferentWithLastParent ? _getDifferentAttrTipParent(_type) : _getAttrTipParent(_type), _value, wnd.attrTipId);
                }, delayTime);
            }
            else
                _m_tipMgr?.addTip(_type, _isDifferentWithLastParent ? _getDifferentAttrTipParent(_type) : _getAttrTipParent(_type), _value, wnd.attrTipId);
        }

        /// <summary>
        /// 清空提示
        /// </summary>
        public void clearTip()
        {
            _m_tipMgr?.clear();
        }

        //获取属性提示父节点
        private Transform _getAttrTipParent(EBasicAttrType _type)
        {
            // if (wnd == null || wnd.attrTipParentList == null)
            //     return null;
            //
            // for (int i = 0; i < wnd.attrTipParentList.Count; i++)
            // {
            //     if (wnd.attrTipParentList[i] != null && wnd.attrTipParentList[i].type == _type && wnd.attrTipParentList[i].tipParentList != null)
            //         return wnd.attrTipParentList[i].tipParentList.GetRandomItem();
            // }
            return null;
        }

        //获取属性提示父节点，与上次获得的不同
        private Transform _getDifferentAttrTipParent(EBasicAttrType _type)
        {
            // if (wnd == null || wnd.attrTipParentList == null)
            //     return null;
            //
            // System.Random curRandom = new System.Random();
            // for (int i = 0; i < wnd.attrTipParentList.Count; i++)
            // {
            //     if (wnd.attrTipParentList[i] != null && wnd.attrTipParentList[i].type == _type && wnd.attrTipParentList[i].tipParentList != null)
            //     {
            //         int selectIndex = curRandom.Next(0, wnd.attrTipParentList[i].tipParentList.Count);
            //         if (_m_dSelectParentIndex.TryGetValue(_type, out int recordIndex))
            //         {
            //             //如果和上次相同，取下一位
            //             if (recordIndex == selectIndex)
            //                 selectIndex++;
            //
            //             //如果超出下标，取第一位
            //             if (selectIndex >= wnd.attrTipParentList[i].tipParentList.Count)
            //                 selectIndex = 0;
            //         }
            //         //记录这次选择
            //         _m_dSelectParentIndex[_type] = selectIndex;
            //         return wnd.attrTipParentList[i].tipParentList[selectIndex];
            //     }
            // }
            return null;
        }
    }
}
