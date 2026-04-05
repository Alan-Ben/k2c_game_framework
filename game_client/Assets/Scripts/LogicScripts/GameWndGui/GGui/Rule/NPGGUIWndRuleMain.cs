using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using UnityEngine.EventSystems;

/// <summary>
/// 规则弹窗
/// </summary>
///
public class NPGGUIWndRuleMain : _ATALBasicUIWnd<NPGGUIMonoRuleMain> ,_IScrollerSmoothMovable
{
    private long _m_resPathId;
    private int _m_iCurIndex;//当前页下标
        
    private float _m_fTargetHorizontal = 0;//目标位置
    private List<float> _m_lPosList;//每张预览图片位置
    private bool _m_bIsDrag = false;//是否抓取
    private float _m_fStartTime = 0;
    private ALCommonEnableTaskController _m_aTickTask;

    public NPGGUIWndRuleMain(long _resPathId,int _defaultIndex) : base(EALUIWndLayer.ADDITION)
    {
        _m_resPathId = _resPathId;
        _m_iCurIndex = _defaultIndex;
    }


    public ScrollRect scrollRect { get => wnd?.scrollRect; }
    protected override string _monoAssetPath { get => UIResPathAssistant.getAssetPath(_m_resPathId); }
    protected override string _monoObjName { get => UIResPathAssistant.getObjName(_m_resPathId); }
    protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    

    protected override void _onShowWnd()
    {
        if (_m_lPosList != null)
        {
            _m_iCurIndex = Mathf.Clamp(_m_iCurIndex, 0, _m_lPosList.Count - 1);
            _m_fTargetHorizontal = _m_lPosList[_m_iCurIndex]; //设置当前坐标，更新函数进行插值  
            if(scrollRect != null)
                scrollRect.horizontalNormalizedPosition = _m_fTargetHorizontal;
        }
        _m_bIsDrag = false;
        _m_fStartTime = 0;
        _refreshWnd();

        _m_aTickTask = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_tickUpdate);
        
        // 注册模拟点击消息监听
        WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_RULE_WND_NEXT_BTN, _onSimulateClickNextBtn);
        WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_RULE_WND_PRE_BTN, _onSimulateClickPreBtn);
    }

    protected override void _onHideWnd()
    {
        _m_aTickTask.setDisable();
        
        // 解除模拟点击消息监听
        WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_RULE_WND_NEXT_BTN, _onSimulateClickNextBtn);
        WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_RULE_WND_PRE_BTN, _onSimulateClickPreBtn);
    }

    protected override void _onReset()
    {
        
    }

    protected override void _onDiscard()
    {
        _m_aTickTask.setDisable();
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
            
        ALUGUICommon.combineBtnClick(wnd.closeBtn, _clickClose);
        ALUGUICommon.combineBtnClick(wnd.closeBtn2, _clickClose);
        ALUGUICommon.combineBtnClick(wnd.btnLeft, _onClickLeft);
        ALUGUICommon.combineBtnClick(wnd.btnLeft2, _onClickLeft);
        ALUGUICommon.combineBtnClick(wnd.btnRight, _onClickRight);
        ALUGUICommon.combineBtnClick(wnd.btnRight2, _onClickRight);
        ALUGUICommon.combineBeginDrag(wnd.btnDrag, _onBeginDrag);
        ALUGUICommon.combineDrag(wnd.btnDrag, _onDrag);
        ALUGUICommon.combineEndDrag(wnd.btnDrag, _onEndDrag);
        if(wnd.listPageJumper != null)
        {
            for (int i = 0; i < wnd.listPageJumper.Count; i++)
            {
                NPGGUICommonPageJumper forUnit = wnd.listPageJumper[i];
                if (forUnit == null)
                    continue;
                ALUGUICommon.combineBtnClick(forUnit.btn, (_obj) =>
                {
                    _onPageJumperClick(forUnit.pageIndex);
                });
            }
        }

        if (wnd.goPreviewList != null)
        {
            int count = wnd.goPreviewList.Length;
            _m_lPosList = new List<float>();
            for (int i = 0; i < count; i++)
            {
                _m_lPosList.Add(i / (float)Mathf.Max(1, count - 1));
            }
        }
    }

    private void _onPageJumperClick(short _pageIndex)
    {
        _switchPage(_pageIndex);
    }

    private void _clickClose(GameObject _obj)
    {
        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_RULE_MAIN);
    }

    private void _onClickLeft(GameObject _obj)
    {
        _m_iCurIndex--;
        _switchPage(_m_iCurIndex);
    }

    private void _onClickRight(GameObject _obj)
    {
        _m_iCurIndex++;
        _switchPage(_m_iCurIndex);
    }

    /// <summary>
    /// 模拟点击下一页按钮
    /// </summary>
    private void _onSimulateClickNextBtn()
    {
        _onClickRight(null);
    }

    /// <summary>
    /// 模拟点击上一页按钮
    /// </summary>
    private void _onSimulateClickPreBtn()
    {
        _onClickLeft(null);
    }
    
    
    private ScrollRect _m_mTempSubScroll;//当前页签下的scroll
    private int _m_iTempStartDragIndex;//开始拖拽的下标

    private float _m_fMoveDistanceX;

    private void _onBeginDrag(PointerEventData _data)
    {
        _m_bIsDrag = true;
        //wnd.scrollRect.OnBeginDrag(_data);
            
        if (_m_iCurIndex < wnd.goPreviewList?.Length)
        {
            _m_mTempSubScroll = wnd.goPreviewList[_m_iCurIndex]?.GetComponentInChildren<ScrollRect>();
            if(_m_mTempSubScroll != null)
            {
                _m_mTempSubScroll.OnBeginDrag(_data);
            }
        }
        _m_iTempStartDragIndex = _m_iCurIndex;

        _m_fMoveDistanceX = 0;
    }

    private void _onDrag(PointerEventData _data)
    {
        if (_m_mTempSubScroll != null)
        {
            _m_mTempSubScroll.OnDrag(_data);
        }
     
        _m_fMoveDistanceX += _data.delta.x;
    }

    private void _onEndDrag(PointerEventData _data)
    {
        if (wnd == null)
            return;

        float posX = wnd.scrollRect.horizontalNormalizedPosition;
        int index = 0;
        if(_m_lPosList != null)
        {
            float offset = Mathf.Abs(_m_lPosList[index] - posX);//计算当前位置与第一页的偏移量，初始化offect

            for (int i = 1; i < _m_lPosList.Count; i++)
            {    //遍历页签，选取当前x位置和每页偏移量最小的那个页面
                float temp = Mathf.Abs(_m_lPosList[i] - posX);
                if (temp < offset)
                {
                    index = i;
                    offset = temp;
                }
            }
        }

        if(_m_iCurIndex == index)
        {
            if(_m_lPosList != null)
            {
                float tMoveDistanceX = _m_fMoveDistanceX;
                // // 如果阿拉伯语则左右滑动反向
                // if(NPGameSetting.instance.getIsRightToLeft())
                //     tMoveDistanceX *= -1;
                    
                if(tMoveDistanceX > wnd.moveDistanceToNext)
                    _m_iCurIndex = Mathf.Clamp(_m_iCurIndex - 1, 0, _m_lPosList.Count - 1);
                else if(tMoveDistanceX < -wnd.moveDistanceToNext)
                    _m_iCurIndex = Mathf.Clamp(_m_iCurIndex + 1, 0, _m_lPosList.Count - 1);
            }
        }
        else
            _m_iCurIndex = index;

        if(_m_lPosList != null)
            _m_fTargetHorizontal = _m_lPosList[_m_iCurIndex]; //设置当前坐标，更新函数进行插值  
        _m_bIsDrag = false;
        _m_fStartTime = 0;

        //wnd.scrollRect.OnEndDrag(_data);
            
        if (_m_mTempSubScroll != null)
        {
            _m_mTempSubScroll.OnEndDrag(_data);
        }

        _switchPage(_m_iCurIndex);
    }
    //切换页面
    private void _switchPage(int _index)
    {
        _m_iCurIndex = _index;
    
        if (_m_lPosList != null)
        {
            _m_iCurIndex = Mathf.Clamp(_m_iCurIndex, 0, _m_lPosList.Count - 1);
            _m_fTargetHorizontal = _m_lPosList[_m_iCurIndex]; //设置当前坐标，更新函数进行插值  
        }
    
        _m_bIsDrag = false;
        _m_fStartTime = 0;
    
        _refreshWnd();
    
        //滑动页面
        new ScrollerSmoothMoveTaskHorizontal(this, _m_fTargetHorizontal, 0.1f).deal();
    }
    
    private void _refreshWnd()
    {
        _refreshLeftRightBtn();
        _refreshCircle();
        _refreshPageNum();
    }
    
    /// <summary>
    /// 刷新底部小圆点
    /// </summary>
    private void _refreshCircle()
    {
        if (wnd!= null && wnd.goCircleList != null)
        {
            for (int i = 0; i < wnd.goCircleList.Length; i++)
            {
                ALUGUICommon.setGameObjEnable(wnd.goCircleList[i], i == _m_iCurIndex);
            }
        }
    }

    /// <summary>
    /// 刷新页数文本
    /// </summary>
    private void _refreshPageNum()
    {
        if (wnd == null)
            return;

        long totalPageCount = (wnd.goPreviewList != null ? wnd.goPreviewList.Length : 0) - wnd.needSubtractContentsNum;
        long curPage = _m_iCurIndex + 1 - wnd.needSubtractContentsNum;
        ALUGUICommon.setLabelTxt(wnd.txtPageNum, TextTranslate.instance.getLanguage(TransKeyConst.common_pageNum_num_num, curPage, totalPageCount));
    }
    
    /// <summary>
    /// 刷新左右按钮状态
    /// </summary>
    private void _refreshLeftRightBtn()
    {
        if(wnd == null)
            return;
        ALUGUICommon.setGameObjEnable(wnd.btnLeft, _m_iCurIndex != 0);
        ALUGUICommon.setGameObjEnable(wnd.btnLeft2, _m_iCurIndex != 0);
        ALUGUICommon.setGameObjEnable(wnd.btnRight, wnd.goPreviewList != null && _m_iCurIndex < wnd.goPreviewList.Length - 1);
        ALUGUICommon.setGameObjEnable(wnd.btnRight2, wnd.goPreviewList != null && _m_iCurIndex < wnd.goPreviewList.Length - 1);
                
        ALUGUICommon.setGameObjEnable(wnd.goZeroPageHide, _m_iCurIndex != 0);
        ALUGUICommon.setGameObjEnable(wnd.goLastPageShow, wnd.goPreviewList != null && _m_iCurIndex == wnd.goPreviewList.Length - 1);
        ALUGUICommon.setGameObjEnable(wnd.goLastPageHide, wnd.goPreviewList != null && _m_iCurIndex != wnd.goPreviewList.Length - 1);
    }
    
    private void _tickUpdate()
    {
        if (!_m_bIsDrag)
        {
            if (wnd == null)
            {
                return;
            }
            _m_fStartTime += Time.deltaTime;
            float t = _m_fStartTime * wnd.fSpeed;
            if (wnd.scrollRect != null)
            {
                //wnd.scrollRect.horizontalNormalizedPosition = Mathf.Lerp(wnd.scrollRect.horizontalNormalizedPosition, _m_fTargetHorizontal, t); //加速滑动效果
    
                if (Mathf.Abs(wnd.scrollRect.horizontalNormalizedPosition - _m_fTargetHorizontal) < 0.0001f)
                {
                    //_refreshWnd();
                    _m_bIsDrag = true;
    
                    if (_m_mTempSubScroll != null)
                    {
                        if (_m_iTempStartDragIndex != _m_iCurIndex)
                            _m_mTempSubScroll.verticalNormalizedPosition = 1f;
                        _m_mTempSubScroll = null;
                    }
                }
            }
        }
    }
}
