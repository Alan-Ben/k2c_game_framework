using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标预览当前阶段bar控制器
    /// </summary>
    public class GGUIWndStageGoalOverviewCurBarController : _AALUGUIGridBarController
    {
        private GGUIWndStageGoalOverviewCurBar _m_wBarWnd;//显示的窗体

        public GGUIWndStageGoalOverviewCurBarController(Transform _parent) : base()
        {
            _m_wBarWnd = new GGUIWndStageGoalOverviewCurBar(_parent);
            _m_wBarWnd.load();
        }

        public override int barHeight { get { return _m_wBarWnd != null && _m_wBarWnd.rectTransform != null ? (int)_m_wBarWnd.rectTransform.rect.height : 0; } }
        public void regLoadDoneDelegate(Action _delegate) { _m_wBarWnd?.regLoadDoneDelegate(_delegate); }

        public override void show()
        {
            _m_wBarWnd?.showWnd();
        }

        public override void hide()
        {
            _m_wBarWnd?.hideWnd();
        }

        protected override void _reset()
        {
            _m_wBarWnd?.resetWnd();
        }

        protected override void _discard()
        {
            _m_wBarWnd?.discard();
            _m_wBarWnd = null;
        }

        public override void setPos(float _x, float _y)
        {
            if (_m_wBarWnd == null)
                return;

            ALUGUICommon.setUIPos(_m_wBarWnd.rectTransform, _x, _y);
        }

        /// <summary>
        /// 重置新阶段动画
        /// </summary>
        public void resetStageAni()
        {
            _m_wBarWnd?.resetStageAni();
        }
        
        /// <summary>
        ///设置动画
        /// </summary>
        /// <param name="_normalizeTime"></param>
        public void sampleStageAni(float _normalizeTime)
        {
            _m_wBarWnd?.sampleStageAni(_normalizeTime);
        }

        /// <summary>
        /// 播放到达新阶段动画
        /// </summary>
        public void playNewStageAni(Action _onPlayDone)
        {
            if(_m_wBarWnd == null)
                _onPlayDone?.Invoke();
            else
                _m_wBarWnd.playNewStageAni(_onPlayDone);
        }

        /// <summary>
        /// 移动到最前面
        /// </summary>
        public void setMoveTrasnformToLast()
        {
            ALUnityCommon.moveTransformToLast(_m_wBarWnd?.getGameObj());
        }
    }
}
