using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 每个类型队列的管理对象
    /// </summary>
    public class TipQueueTypeInfo
    {
        //任务列表
        [NotNull]private List<_ABaseTipQueueDealer> _m_dealerList = new List<_ABaseTipQueueDealer>();
        //提示类型
        private ETipQueueType _m_eType;
        //当前处理序列号
        private long _m_lDealSerialize;
        //当前是否在处理中
        private bool _m_bIsTaskDealing;
        //开始当前序列号处理的时间戳，用于判断是否超时
        private float _m_fStartDealTimeTag;
        private const float _c_fDealTimeoutS = 10f;

        //所有任务完成回调
        public event Action dealAllDoneAction;

        public TipQueueTypeInfo(ETipQueueType _type)
        {
            _m_eType = _type;
            _m_lDealSerialize = ALSerializeOpMgr.next();
            _m_bIsTaskDealing = false;
            _m_fStartDealTimeTag = 0f;
        }

        /// <summary>
        /// 提示类型
        /// </summary>
        public ETipQueueType tipType { get { return _m_eType; } }
        /// <summary>
        /// 是否所有处理完毕
        /// </summary>
        public bool isAllDone { get { return _m_dealerList.Count <= 0; } }

        /// <summary>
        /// 添加tip
        /// </summary>
        /// <param name="_dealer"></param>
        public void addDealer(_ABaseTipQueueDealer _dealer)
        {
            if (null == _dealer)
                return;

            //先注册完成回调
            _dealer.regDoneDelegate(() =>
            {
                _onDealDone(_dealer, _dealer.dealSerialize);
            });

            //如果要替换，把队列里的全部销毁
            if (_dealer.isReplaceNew)
            {
                List<_ABaseTipQueueDealer> dealerList = new List<_ABaseTipQueueDealer>();
                dealerList.AddRange(_m_dealerList);

                foreach (_ABaseTipQueueDealer anpBaseTipQueueDealer in dealerList)
                {
                    if (null == anpBaseTipQueueDealer)
                        continue;

                    //直接处理完成操作
                    anpBaseTipQueueDealer.setDealDone();
                }

                _m_dealerList.Clear();
                dealerList.Clear();
            }

            //将数据添加到队列
            _m_dealerList.Add(_dealer);
        }

        /// <summary>
        /// 开始展示tip
        /// </summary>
        public void startShowTip()
        {
            if (_m_bIsTaskDealing)
                return;

            //构造新序列号
            _m_lDealSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lDealSerialize;

            //设置为开始处理
            _m_bIsTaskDealing = true;
            //设置处理时间标记
            _m_fStartDealTimeTag = Time.unscaledTime;

            //开启任务处理，并带入下一个处理序列号
            ALCommonTaskController.CommonActionAddMonoTask(() => { _dealerTask(curSerialize); });
        }

        /// <summary>
        /// 检查展示是否超时，超时会自动完成展示避免堵塞队列
        /// </summary>
        public void checkTimeout()
        {
            //当不需要开启时，进行超时判断，如果超时则强制开启
            if (_m_bIsTaskDealing && (Time.unscaledTime - _m_fStartDealTimeTag > _c_fDealTimeoutS))
            {
                //超时的时候要将第一个已经开始的处理队列节点删除
                if (_m_dealerList.Count > 0)
                {
                    _ABaseTipQueueDealer firstDealer = _m_dealerList[0];
                    if (null != firstDealer && firstDealer.isStarted)
                    {
                        //删除节点
                        _m_dealerList.RemoveAt(0);
                        //直接设置完成
                        firstDealer.setDealDone();
                    }
                }
            }
        }

        /// <summary>
        /// 处理tip信息，带入处理序列号
        /// </summary>
        /// <returns></returns>
        protected void _dealerTask(long _dealSerialize)
        {
            //判断序列号是否有效
            if (_dealSerialize != _m_lDealSerialize)
                return;

            if (_m_dealerList.Count <= 0)
                return;

            //标记在处理中
            _ABaseTipQueueDealer firstDealer = _m_dealerList[0];

            //正常不会出现
            if (null == firstDealer)
            {
                _m_bIsTaskDealing = false;
                _m_dealerList.RemoveAt(0);
                //开启任务处理下一个
                ALCommonTaskController.CommonActionAddMonoTask(() => { _dealerTask(_dealSerialize); });
                return;
            }

            _m_bIsTaskDealing = true;
            //开始处理节点
            //判断是否需要合并展示
            if (firstDealer.needMarge && _m_dealerList.Count > 1)
            {
                List<_ABaseTipQueueDealer> dealerList = new List<_ABaseTipQueueDealer>();
                dealerList.AddRange(_m_dealerList);
                _m_dealerList.Clear();
                _m_dealerList.Add(firstDealer);
                firstDealer.startMargeDealer(dealerList, _dealSerialize);
            }
            else
                firstDealer.startDealer(_dealSerialize);
        }

        /// <summary>
        /// 处理对象结束时调用的处理函数
        /// </summary>
        /// <param name="_tipQueueDealer"></param>
        /// <param name="_dealSerialize"></param>
        private void _onDealDone(_ABaseTipQueueDealer _tipQueueDealer, long _dealSerialize)
        {
            if (null == _tipQueueDealer)
                return;

            //无后续处理跳出
            if (_m_dealerList.Count <= 0)
            {
                //判断处理执行完成对调
                _checkDealAllDoneAction();
                return;
            }

            //获取当前处理对象
            _ABaseTipQueueDealer tipDealer = _m_dealerList[0];
            //只有在处理对象一致表示数据队列正常才进行删除处理
            //否则可能因为某些问题如超时，跳过了处理对象，且后续逻辑都已经处理。这里不做处理
            if (tipDealer == _tipQueueDealer)
            {
                //移除队列
                //删除第一个对象
                _m_dealerList.RemoveAt(0);
            }


            //判断序列号是否有效，序列号无效则不进行后续处理
            if (_dealSerialize != _m_lDealSerialize)
                return;

            //检测是否已经全部处理完成，如果全部处理完成则不进行后续处理
            if (_checkDealAllDoneAction())
                return;

            //开启任务处理，并带入下一个处理序列号
            ALCommonTaskController.CommonActionAddMonoTask(() => { _dealerTask(_dealSerialize); });
        }

        /// <summary>
        /// 处理本队列内全部任务执行完毕函数
        /// </summary>
        private bool _checkDealAllDoneAction()
        {
            //这个再校验一次保险，没处理完直接返回
            if (!isAllDone)
                return false;

            //设置当前执行状态
            _m_bIsTaskDealing = false;

            if (null != dealAllDoneAction)
                dealAllDoneAction();

            return true;
        }
    }
}