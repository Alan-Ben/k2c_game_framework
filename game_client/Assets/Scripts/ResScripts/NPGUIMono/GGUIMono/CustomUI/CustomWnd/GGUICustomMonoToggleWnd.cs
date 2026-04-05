using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 用于做一些需要展开收起动画的支持，策划可以自己配，展开收起记录到内存，整个游戏生命周期内有效
    /// </summary>
    public class GGUICustomMonoToggleWnd : MonoBehaviour
    {
        [ALHeader("默认是否选中")]
        public bool isOn;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("选中 状态时显示")]
        public List<GameObject> isOnShow;
        [ALHeader("非选中 状态时显示")]
        public List<GameObject> isOffShow;
        [ALHeader("选中动画")]
        public Animation selectAnimation;
        [ALHeader("选中动画名称")]
        public string selectAniName;
        [ALHeader("隐藏取消选中动画名称")]
        public string disSelectAniName;
        
        private bool _m_bIsOn;//当前选中状态

        private void Awake()
        {
            //绑定进入按钮操作
            ALUGUICommon.combineBtnClick(btnClick, _onClickSelectButton);

            //使用默认值设置选中状态
            setSelected(isOn, true, false);
        }

        private void _onClickSelectButton(GameObject _gameObject)
        {
            setSelected(!_m_bIsOn, true);
        }
        
        //设置选中状态
        public virtual void setSelected(bool _isSelect, bool _force = false, bool _showAni = true)
        {
            if(_force || _m_bIsOn != _isSelect)
            {
                _m_bIsOn = _isSelect;
                ALUGUICommon.setGameObjEnable(isOnShow, _isSelect);
                ALUGUICommon.setGameObjEnable(isOffShow, !_isSelect);

                if(null == selectAnimation)
                    return;

                //是否播放动画
                if (_showAni)
                {
                    //播放动画时候要延迟处理隐藏，类似窗口hide动画处理过程
                    if (_isSelect)
                    {
                        ALUGUICommon.setGameObjEnable(isOffShow, true);
                        _dealSelectAniAction(selectAniName, () =>
                        {
                            if(_m_bIsOn)
                                ALUGUICommon.setGameObjEnable(isOffShow, false);                    
                        });
                    }
                    else
                    {
                        ALUGUICommon.setGameObjEnable(isOnShow, true);
                        _dealSelectAniAction(disSelectAniName, () =>
                        {
                            if(!_m_bIsOn)
                                ALUGUICommon.setGameObjEnable(isOnShow, false);
                        });
                    }   
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(isOffShow, !_m_bIsOn);
                    ALUGUICommon.setGameObjEnable(isOnShow, _m_bIsOn);
                }
            }
        }

        /***********
         * 在播放对应动作后执行指定函数
         **/
        private void _dealSelectAniAction(string _aniName, Action _aniDoneAction = null)
        {
            //处理动画，然后延迟处理函数
            if (null != selectAnimation)
            {
                AnimationClip clip = selectAnimation.GetClip(_aniName);
                if (null != clip)
                {
                    selectAnimation.ForcePlay(_aniName);

                    //延迟处理事件
                    ALCommonActionMonoTask.addMonoTask(_aniDoneAction, clip.length);
                }
                else
                {
                    if (null != _aniDoneAction)
                        _aniDoneAction();
                }
            }
            else
            {
                if (null != _aniDoneAction)
                    _aniDoneAction();
            }
        }
    }
}