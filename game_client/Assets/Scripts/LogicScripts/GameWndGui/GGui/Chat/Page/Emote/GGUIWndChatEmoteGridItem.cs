using System;
using ALPackage;
using GOE;
using UnityEngine;


/// <summary>
/// 表情item
/// </summary>
public class GGUIWndChatEmoteGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoChatEmoteGridItem>
{
    private GChatEmoteItemRefObj _m_emoteRef;
    private GameObject _m_emoteGo;
    private NPGGoIndex _m_emoteIdx;
    public event Action<GGUIWndChatEmoteGridItem> onClickAction;
    
    public GGUIWndChatEmoteGridItem(GGUIMonoChatEmoteGridItem _wnd) : base(_wnd)
    {
        initWnd();
    }

    public GChatEmoteItemRefObj emoteRef { get => _m_emoteRef; }

    protected override void _onShowWnd()
    {
        
    }

    protected override void _onHideWnd()
    {
        
    }

    protected override void _onReset()
    {
        
    }

    protected override void _onDiscard()
    {
        _discardGo();
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
        ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickBtn);
    }

    protected override void _resetGridItem()
    {
        
    }

    private void _onClickBtn(GameObject obj)
    {
        onClickAction?.Invoke(this);
    }

    /// <summary>
    /// 设置显示信息
    /// </summary>
    /// <param name="_emoteRef"></param>
    public void setInfo(GChatEmoteItemRefObj _emoteRef)
    {
        _m_emoteRef = _emoteRef;
        _refreshWnd();
    }

    /// <summary>
    /// 刷新界面
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd)
            return;
        if(null == _m_emoteRef)
            return;
        NPGGoIndex curGoIndex = _m_emoteRef.go_index;

        if (curGoIndex.Equals(_m_emoteIdx))
            return;
        _discardGo();
        
        GGoResCore.instance.loadObj(_m_emoteRef.go_index, (_assetHandle) =>
        {
            
            if (_assetHandle == null || _assetHandle.loadedInfo == null)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                return;
            }
            //获取资源对象
            GameObject assetGo = _assetHandle.loadedInfo.obj;
            if (null == assetGo)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                return;
            }

            if (!curGoIndex.Equals(_m_emoteRef.go_index))
                return;
            _m_emoteIdx = curGoIndex;
            _m_emoteGo = GameObject.Instantiate(assetGo);
            _m_emoteGo.transform.SetParent(wnd.itemParent);
            _m_emoteGo.transform.localPosition = Vector3.zero;
            _m_emoteGo.transform.localScale = Vector3.one;
        });
    }
    
    /// <summary>
    /// 销毁已经加载的go
    /// </summary>
    private void _discardGo()
    {
        if(null != _m_emoteGo)
            ALUnityCommon.releaseGameObj(_m_emoteGo);
        _m_emoteGo = null;
        _m_emoteIdx = null;
    }
}
