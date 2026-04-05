using ALPackage;
using UnityEngine;

namespace GOE
{
    public partial class _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM>
    {
        /// <summary>
        /// 重置数据状态
        /// </summary>
        private class MoveShowcaseState_RESET : _AMoveShowcaseState
        {
            //是否强制刷新
            private bool _m_forceRefresh;
            //是否加载完成
            private bool _m_isLoadDone;

            
            public MoveShowcaseState_RESET(_NGGUIWndCommonShowCase_LeftRightMove<T_ITEM> _wnd) : base(_wnd)
            {
            }
            
            public override EMoveShowCaseStateType state { get { return EMoveShowCaseStateType.RESET; } }

            public void setInfo(bool _forceRefresh)
            {
                _m_forceRefresh = _forceRefresh;
            }
            
            protected override void _onEnter()
            {
                if(null == _m_showcaseInfo || null == _m_data)
                    return;

                _m_isLoadDone = false;
                ALStepCounter alStepCounter = new ALStepCounter();
                alStepCounter.chgTotalStepCount(4);
                alStepCounter.regAllDoneDelegate(() =>
                {
                    _m_isLoadDone = true;
                    //刷新位置
                    _refreshUnitPos();
                    //切换回idle状态
                    _changeState(EMoveShowCaseStateType.IDLE);
                });
                
                //刷新showcase里面的单位
                //刷新加载左边的单位
                _m_showcaseInfo.removeLoadUnit(_m_data.left.index);
                _m_showcaseInfo.additionLoadUnit(_m_data.left.infoObj, _m_data.left.index, alStepCounter.addDoneStepCount);
                
                //刷新加载右边的单位
                _m_showcaseInfo.removeLoadUnit(_m_data.right.index);
                _m_showcaseInfo.additionLoadUnit(_m_data.right.infoObj, _m_data.right.index, alStepCounter.addDoneStepCount);
                
                //刷新加载背景
                _m_showcaseInfo.additionLoadUnit(_m_data.bg, 3, alStepCounter.addDoneStepCount);

                //强制刷新的话中间单位也重新复制
                if(_m_forceRefresh)
                {
                    _m_showcaseInfo.additionLoadUnit(_m_data.center.infoObj, _m_data.center.index, alStepCounter.addDoneStepCount);
                }
                else
                {
                    alStepCounter.addDoneStepCount();
                }
            }

            protected override void _onExit()
            {
            }

            protected override void _onTick(float _deltaTime)
            {
                
            }

            public override bool canEnterState(_ATALStateBase<EMoveShowCaseStateType> _newState)
            {
                if (null == _newState)
                    return false;

                //没加载完成不能切换
                if (!_m_isLoadDone)
                    return false;
                
                if(_newState.state == EMoveShowCaseStateType.IDLE
                   || _newState.state == EMoveShowCaseStateType.NONE)
                    return true;

                return false;
            }

            public override void resetData()
            {
                _m_isLoadDone = false;
                _m_forceRefresh = false;
            }

            //刷新每个单位的位置
            private void _refreshUnitPos()
            {
                if(null == _m_showcaseInfo || null == _m_data || null ==_m_data.center.infoObj)
                    return;

                //左边对象位置
                if (_m_data.left.infoObj != null)
                    _m_data.left.setLocalPosition(_m_data.leftDefaultPos);
                //中间对象位置
                if (_m_data.center.infoObj != null)
                    _m_data.center.setLocalPosition(_m_data.centerDefaultPos);
                //右边对象位置
                if (_m_data.right.infoObj != null)
                    _m_data.right.setLocalPosition(_m_data.rightDefaultPos);
            }
        } 
    }
}