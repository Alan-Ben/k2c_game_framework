
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 货币对应的一个资源收集目标对象
    /// </summary>
    public abstract class _ANPGGUISubNumHarvestWnd : _ANPGGUISubHarvestWnd
    {
        private readonly Text _m_tText;
        //翻译Key
        protected readonly string _m_sTransKey;

        //当前货币值真实数据
        private long _m_lRealCount;

        //收获数字的管理器，只有在目标类型有效的情况下才创建
        [NotNull] private readonly NPGGUIHarvestNumController _m_hcHarvestController;
        
        protected _ANPGGUISubNumHarvestWnd(EHarvestType _harvestResType, Text _txtNum, RectTransform _target, string _transKey = null)
            : base(_harvestResType, _target)
        {
            _m_tText = _txtNum;
            _m_sTransKey = _transKey;

            _m_lRealCount = 0;
            _m_hcHarvestController = new NPGGUIHarvestNumController();
        }
        protected _ANPGGUISubNumHarvestWnd(EHarvestType _harvestResType, Text _txtNum, string _transKey = null)
            : base(_harvestResType, _txtNum == null ? null : _txtNum.rectTransform)
        {
            _m_tText = _txtNum;
            _m_sTransKey = _transKey;

            _m_lRealCount = 0;
            _m_hcHarvestController = new NPGGUIHarvestNumController();
        }

        /// <summary>
        /// 初始化相关的监控和管理
        /// </summary>
        protected override void _onInit()
        {
            _m_lRealCount = _getRealCount();

            //刷新显示
            _refresh();
            _m_hcHarvestController.regAllTaskDoneDelegate(_refresh);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void _onDiscard()
        {
            //清空数据控制类
            _m_hcHarvestController.discard();
        }

        /// <summary>
        /// 更新最新的数量
        /// </summary>
        public void updateNum(long _newCount)
        {
            //刷新真实数据
            _m_lRealCount = _newCount;

            //刷新显示
            _refresh();
        }

        /// <summary>
        /// 刷新处理
        /// </summary>
        private void _refresh()
        {
            //判断是否有任务在处理
            if (_m_hcHarvestController.taskEnable)
            {
                //使用任务数据刷新
                long taskCount = _m_hcHarvestController.calTotalCount();
                ALUGUICommon.setLabelTxt(_m_tText, string.IsNullOrEmpty(_m_sTransKey) ? taskCount.ToLargeString(harvestType.toLargeStringType()) : TextTranslate.instance.getLanguage(_m_sTransKey, taskCount.ToLargeString(harvestType.toLargeStringType())));
            }
            else
            {
                //刷新显示
                ALUGUICommon.setLabelTxt(_m_tText, string.IsNullOrEmpty(_m_sTransKey) ? _m_lRealCount.ToLargeString(harvestType.toLargeStringType()) : TextTranslate.instance.getLanguage(_m_sTransKey, _m_lRealCount.ToLargeString(harvestType.toLargeStringType())));
            }
        }

        protected abstract long _getRealCount();

        #region 收获接口部分

        /// <summary>
        /// 粒子开始表现时的处理
        /// 此时需要设置好相关的状态以及可能触发消息的处理，避免错误的刷新操作
        /// </summary>
        public override void particleStart(long _serialize)
        {
            _m_hcHarvestController.startNumFunc(_m_lRealCount, _serialize);
        }
        /// <summary>
        /// 粒子表现结束的处理
        /// </summary>
        /// <param name="_serialize"></param>
        public override void particleComplete(long _serialize)
        {
            //设置任务完成
            _m_hcHarvestController.setTaskDone(_serialize);

            //刷新显示
            _refresh();
        }
        /// <summary>
        /// 单个粒子飞行到位的处理
        /// </summary>
        public override void particleItemDone(long _serialize, long _itemCount)
        {
            //设置任务数量
            _m_hcHarvestController.addTaskNum(_serialize, _itemCount);

            //刷新显示
            _refresh();
        }

        #endregion
    }
}