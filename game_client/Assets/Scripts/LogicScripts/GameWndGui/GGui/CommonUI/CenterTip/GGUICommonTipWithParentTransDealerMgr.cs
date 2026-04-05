using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用tip绑定父节点展示管理器
    /// </summary>
    public class GGUICommonTipWithParentTransDealerMgr
    {
        private List<TipWithParentData> _m_lTipDealer;//tip队列，按照时间间隔逐个显示
        private bool _m_bIsShowing;//是否正在显示
        private long _m_lCoexistMaxCount;//tip同时存在最大数量
        private float _m_fTipAccMaxRate;//tip最大播放速度
        private int _m_iTipAccMinNum;//tip数量达到这个数则开始加速
        private int _m_iTipAccMaxNum;//tip数量达到这个数则达到最高速
        private long _m_lShowSerialize;//显示序列号
        private List<TipWithParentData> _m_hasShowTipDealer;//已经展示了的tipdealer队列

        public GGUICommonTipWithParentTransDealerMgr(long _coexistMaxCount, float _tipAccMaxRate = 3f, int _tipAccMinNum = 2, int _tipAccMaxNum = 12)
        {
            _m_lCoexistMaxCount = _coexistMaxCount;
            _m_lTipDealer = new List<TipWithParentData>();
            _m_hasShowTipDealer = new List<TipWithParentData>();
            _m_fTipAccMaxRate = _tipAccMaxRate;
            _m_iTipAccMinNum = _tipAccMinNum;
            _m_iTipAccMaxNum = _tipAccMaxNum;
            _m_bIsShowing = false;
        }

        public bool isShowing { get => _m_bIsShowing; }

        #region 外部调用

        /// <summary>
        /// 清空数据
        /// </summary>
        public void clear()
        {
            _m_lTipDealer?.Clear();
            if (_m_hasShowTipDealer != null)
            {
                TipWithParentData tipDealerInterface;
                for(int i = _m_hasShowTipDealer.Count - 1; i > -1; i-- )
                {
                    tipDealerInterface = _m_hasShowTipDealer[i];
                    if (null == tipDealerInterface.tipInterface)
                        continue;
                    tipDealerInterface.tipInterface.discard();
                }

                _m_hasShowTipDealer.Clear();
            }

            _m_bIsShowing = false;
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }
        
        /// <summary>
        /// 根据tag移除为展示的tip
        /// </summary>
        public void clearQueueTipByTag(ETipDealerTagType _tag)
        {
            //NONE默认值不处理
            if(_tag == ETipDealerTagType.NONE)
                return;
            
            if(null == _m_lTipDealer)
                return;

            TipWithParentData dealerInterface;
            for (int i = _m_lTipDealer.Count - 1; i >= 0; i--)
            {
                dealerInterface = _m_lTipDealer[i];
                if (null != dealerInterface.tipInterface && dealerInterface.tipInterface.tag == _tag)
                {
                    _m_lTipDealer.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 添加一个待显示的tip
        /// </summary>
        /// <param name="_tip"></param>
        /// <param name="_tipParent"></param>
        public void addTip(_INPTipDealerInterface _tip, Transform _tipParent)
        {
            if (_m_lTipDealer == null)
                return;

            TipWithParentData data = new TipWithParentData();
            data.tipInterface = _tip;
            data.tipParent = _tipParent;
            _m_lTipDealer.Add(data);

            _checkAndShowTip();
        }

        /// <summary>
        /// 开始显示
        /// </summary>
        public void start()
        {
            _checkAndShowTip();
        }

        /// <summary>
        /// 暂停显示
        /// </summary>
        public void pause()
        {
            _m_bIsShowing = false;
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }
        
        #endregion

        /// <summary>
        /// 检查是否需要显示tip，如果需要则显示tip
        /// </summary>
        private void _checkAndShowTip()
        {
            //不需要显示或已经在显示，不处理
            if (_m_lTipDealer.Count == 0 || _m_bIsShowing)
                return;


            //标记正在显示
            _m_bIsShowing = true;

            //序列号增加
            long serialize = _m_lShowSerialize = ALSerializeOpMgr.next();

            //延迟处理
            ALCommonActionMonoTask.addLaterMonoTask(() =>
            {
                _dealShowTip(serialize);
            });
        }

        /// <summary>
        /// 显示tip
        /// </summary>
        private void _dealShowTip(long _showSerialze)
        {
            //序列号不一致，不处理
            if (_showSerialze != _m_lShowSerialize)
            {
                return;
            }

            //判断是否播放结束
            if (_m_lTipDealer == null
                || _m_lTipDealer.Count == 0)
            {
                _m_bIsShowing = false;
                return;
            }

            //从待显示的队列中取出tip数据
            TipWithParentData dealer = _m_lTipDealer[0];
            _m_lTipDealer.RemoveAt(0);
            
            //数据有误则跳过
            if (dealer.tipInterface == null || dealer.tipParent == null)
            {
                _dealShowTip(_showSerialze);
                return;
            }

            //加入到已经展示的队列
            if (_m_hasShowTipDealer != null)
            {
                _m_hasShowTipDealer.Add(dealer);

                //直接销毁超出数量的tip
                while (_m_hasShowTipDealer.Count > 0 && _m_hasShowTipDealer.Count > _m_lCoexistMaxCount)
                {
                    TipWithParentData tipData = _m_hasShowTipDealer[0];
                    _m_hasShowTipDealer.RemoveAt(0);
                    tipData.tipInterface.discard();
                }
            }
            
            //显示tip
            dealer.tipInterface.showTip(_m_lShowSerialize, dealer.tipParent, _getCurTipSpeed(_m_lTipDealer.Count), _dealShowTip, _onTipDealerDiscard);
        }

        /// <summary>
        /// 获取当前tip播放速度
        /// </summary>
        /// <returns></returns>
        private float _getCurTipSpeed(int _showCount)
        {
            if (_showCount >= _m_iTipAccMinNum)
            {
                return ((float)_showCount).RemapClamp(_m_iTipAccMinNum, _m_iTipAccMaxNum, 1, _m_fTipAccMaxRate);
            }
            return 1f;
        }

        /// <summary>
        /// 播放完成销毁tip
        /// </summary>
        /// <param name="_dealer"></param>
        private void _onTipDealerDiscard(_INPTipDealerInterface _dealer)
        {
            for (int i = 0; i < _m_hasShowTipDealer.Count; i++)
            {
                if (_m_hasShowTipDealer[i].tipInterface == _dealer)
                {
                    _m_hasShowTipDealer.RemoveAt(i);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// tip与父节点结构体
    /// </summary>
    public struct TipWithParentData
    {
        public _INPTipDealerInterface tipInterface;
        public Transform tipParent;
    }
}
