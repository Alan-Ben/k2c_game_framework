using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class NPGGUICommonTipDealerMgr
    {
        private Transform _m_tTipParent;//父节点
        private List<_INPTipDealerInterface> _m_lTipDealer;//tip队列，按照时间间隔逐个显示
        private bool _m_bIsShowing;//是否正在显示
        private float _m_fTipAccMaxRate;//tip最大播放速度
        private int _m_iTipAccMinNum;//tip数量达到这个数则开始加速
        private int _m_iTipAccMaxNum;//tip数量达到这个数则达到最高速
        private long _m_lShowSerialize;//显示序列号

        //已经展示了的tipdealer队列
        private List<_INPTipDealerInterface> _m_hasShowTipDealer;

        public NPGGUICommonTipDealerMgr(Transform _tipParent, float _tipAccMaxRate = 3f, int _tipAccMinNum = 2, int _tipAccMaxNum = 12)
        {
            _m_tTipParent = _tipParent;
            _m_lTipDealer = new List<_INPTipDealerInterface>();
            _m_hasShowTipDealer = new List<_INPTipDealerInterface>();
            _m_fTipAccMaxRate = _tipAccMaxRate;
            _m_iTipAccMinNum = _tipAccMinNum;
            _m_iTipAccMaxNum = _tipAccMaxNum;
            _m_bIsShowing = false;
        }
        public NPGGUICommonTipDealerMgr(float _tipAccMaxRate = 3f, int _tipAccMinNum = 2, int _tipAccMaxNum = 12)
        {
            _m_lTipDealer = new List<_INPTipDealerInterface>();
            _m_hasShowTipDealer = new List<_INPTipDealerInterface>();
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
                _INPTipDealerInterface tipDealerInterface = null;
                for(int i = _m_hasShowTipDealer.Count - 1; i > -1; i-- )
                {
                    tipDealerInterface = _m_hasShowTipDealer[i];
                    if (null == tipDealerInterface)
                        continue;
                    tipDealerInterface.discard();
                }

                _m_hasShowTipDealer.Clear();
            }

            _m_bIsShowing = false;
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }
        
        /// <summary>
        /// 根据tag移除所有的tip
        /// </summary>
        public void clearAllTipByTag(ETipDealerTagType _tag)
        {
            if (_m_hasShowTipDealer != null)
            {
                _INPTipDealerInterface tipDealerInterface = null;
                for(int i = _m_hasShowTipDealer.Count - 1; i > -1; i-- )
                {
                    tipDealerInterface = _m_hasShowTipDealer[i];
                    if (null == tipDealerInterface || tipDealerInterface.tag != _tag)
                        continue;

                    //这里先移除，不然调用discard会先回调到_onTipDealerDiscard里，也会移除一次列表导致异常
                    _m_hasShowTipDealer.RemoveAt(i);
                    tipDealerInterface.discard();
                }
            }
            clearQueueTipByTag(_tag);
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

            _INPTipDealerInterface dealerInterface = null;
            for (int i = _m_lTipDealer.Count - 1; i >= 0; i--)
            {
                dealerInterface = _m_lTipDealer[i];
                if (null != dealerInterface && dealerInterface.tag == _tag)
                {
                    _m_lTipDealer.RemoveAt(i);
                }
            }
        }

        public void removeTip(_INPTipDealerInterface _tip)
        {
            _m_lTipDealer?.Remove(_tip);
        }
        
        /// <summary>
        /// 添加一个待显示的tip
        /// </summary>
        /// <param name="_tip"></param>
        public void addTip(_INPTipDealerInterface _tip)
        {
            if (_m_lTipDealer == null)
                return;

            _m_lTipDealer.Add(_tip);

            _checkAndShowTip();
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

            _m_tTipParent = _tipParent;
            _m_lTipDealer.Add(_tip);

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
            _INPTipDealerInterface dealer = _m_lTipDealer[0];
            _m_lTipDealer.RemoveAt(0);
            
            //数据有误则跳过
            if (dealer == null)
            {
                _dealShowTip(_showSerialze);
                return;
            }

            //加入到已经展示的队列
            if (_m_hasShowTipDealer != null) 
                _m_hasShowTipDealer.Add(dealer);
            
            //显示tip
            dealer.showTip(_m_lShowSerialize, _m_tTipParent, _getCurTipSpeed(_m_lTipDealer.Count), _dealShowTip, _onTipDealerDiscard);
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

        private void _onTipDealerDiscard(_INPTipDealerInterface _dealer)
        {
            if (_m_hasShowTipDealer != null) 
                _m_hasShowTipDealer.Remove(_dealer);
        }
    }
}
