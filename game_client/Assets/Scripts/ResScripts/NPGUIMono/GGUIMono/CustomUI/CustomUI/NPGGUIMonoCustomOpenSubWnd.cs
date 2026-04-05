using System;
using ALPackage;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// 自定义点击展开和关闭的UImono
    /// </summary>
    public class NPGGUIMonoCustomOpenSubWnd : MonoBehaviour
    {
        [ALHeader("点击展开的按钮")]
        public GameObject btnOpen;
        [ALHeader("点击收回的按钮")]
        public GameObject btnClose;
        [ALHeader("展开或者收回对应的UI动画")]
        public Animation subWndAni;
        [ALHeader("展开的动画名")]
        public string showAniName;
        [ALHeader("收回的动画名")]
        public string hideAniName;
        [ALHeader("是否默认展开")]
        public bool isDefaultShow;
#if NP_GAME
        //当前选中状态
        private bool _m_cusIsShow;
        private bool _m_bHasStart = false; // 是否已经执行Start

        //初始化时候设置一次当前默认选中状态
        private void Awake()
        {
            _m_cusIsShow = isDefaultShow;
            _m_bHasStart = false;
        }
        /// <summary>
        /// Awake -> OnEnable-> Start
        /// 执行顺序如上，动画的采样要在Start的时候，资源才加载完
        /// </summary>
        private void Start()
        {
            _m_bHasStart = true;
            _sampleSubWndAni();
        }
        
        private void OnEnable()
        {
            // 如果还没有Start，说明没有加载好，先不处理动画
            if (_m_bHasStart)
                _sampleSubWndAni();

            ALUGUICommon.combineBtnClick(btnOpen,_onClickOpen);
            ALUGUICommon.combineBtnClick(btnClose, _onClickClose);
            ALUGUICommon.combineBeginDrag(btnClose, _onDragClose);

            WinMsg.RegisterMsgAct(WinMsgType.CLOSE_CUSTOM_OPEN_SUB_WND, _onRecCloseMgs);
        }
        
        private void _sampleSubWndAni()
        {
            //当前是否选中
            if (_m_cusIsShow)
            {
                subWndAni.Sample(showAniName, 1);
            }
            else
            {
                subWndAni.Sample(hideAniName, 1);
            }
        }

        /// <summary>
        /// 点击展开
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickOpen(GameObject _obj)
        {
            _m_cusIsShow = true;
            
            if (null != subWndAni)
            {
                subWndAni.Play(showAniName);
            }
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickClose(GameObject _go)
        {
            _m_cusIsShow = false;
            
            if (null != subWndAni)
            {
                subWndAni.Play(hideAniName);
            }
        }

        /// <summary>
        /// 在关闭对象上开始拖拽的时候触发的处理
        /// </summary>
        /// <param name="_obj"></param>
        private void _onDragClose(PointerEventData _eventData)
        {
            _m_cusIsShow = false;
            
            if (null != subWndAni)
            {
                subWndAni.Play(hideAniName);
            }
        }
        
        private void OnDisable()
        {
            _m_cusIsShow = false;

            ALUGUICommon.uncombineBtnClick(btnOpen,_onClickOpen);
            ALUGUICommon.uncombineBtnClick(btnClose, _onClickClose);
            ALUGUICommon.uncombineBeginDrag(btnClose, _onDragClose);

            WinMsg.UnregisterMsgAct(WinMsgType.CLOSE_CUSTOM_OPEN_SUB_WND, _onRecCloseMgs);
        }

        //收到关闭的消息
        private void _onRecCloseMgs()
        {
            _onClickClose(null);
        }

#endif
    }
}