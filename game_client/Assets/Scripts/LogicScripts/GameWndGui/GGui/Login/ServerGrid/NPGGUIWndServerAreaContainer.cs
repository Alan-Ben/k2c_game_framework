using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;

namespace GOE
{
    public class NPGGUIWndServerAreaContainer : _ANPGGUIBasicSubWndContainer<NPGGUIMonoServerAreaItem, NPGGUIMonoServerAreaContainer, NPGGUIWndServerAreaItem>
    {
        private Action<string> _m_onSelectedArea; //选中的回调
        private NPGGUIWndServerAreaItem _m_selectedWnd; //选中的大区Item

        public NPGGUIWndServerAreaContainer(NPGGUIMonoServerAreaContainer _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        public Action<string> OnSelectedArea { get { return _m_onSelectedArea; } set { _m_onSelectedArea = value; } }

        protected override NPGGUIWndServerAreaItem _createItemWnd(NPGGUIMonoServerAreaItem _itemMono)
        {
            return new NPGGUIWndServerAreaItem(_itemMono);
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            _m_selectedWnd = null;
        }

        protected override void _onReset()
        {
            _m_selectedWnd = null;
        }

        protected override void _onDiscard()
        {
            _m_selectedWnd = null;
        }

        protected override void _onWndInitDone()
        {
        }

        public void refreshWnd()
        {
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口，数据不多直接清了重加
        /// </summary>
        private void _refreshWnd()
        {
            //清空数据
            clearAll();

            NPGGUIWndServerAreaItem tempItemWnd = null;

            Dictionary<string, string> areaDic = CDNSetting_AreaInfo.instance.areaMap;
            if (areaDic != null)
            {
                foreach (KeyValuePair<string, string> keyValuePair in areaDic)
                {
                    if(keyValuePair.Key == "default")
                        continue;

                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;

                    NPLoginAreaRefObj areaRefObj = GRefdataCoreMgr.instance.getAreaByTag(keyValuePair.Key);

                    //刷新数据
                    tempItemWnd.showWnd();
                    tempItemWnd.setInfo(keyValuePair.Key, areaRefObj, _onClickChgArea);
                }
            }
            
            //如果没有当前选中大区，默认选中当前大区
            if (null == _m_selectedWnd)
            {
                NPGGUIWndServerAreaItem selectWnd = getFirstItemWnd((itemWnd) => itemWnd != null && itemWnd.getAreaTag() == CDNSetting_AreaInfo.instance.areaTag);
                _onSelectedArea(selectWnd);
            }
        }

        /// <summary>
        /// 点击切换大区
        /// </summary>
        private void _onClickChgArea(NPGGUIWndServerAreaItem _subWnd)
        {
            if (null == _subWnd || _m_selectedWnd == _subWnd)
                return;

            //需要切换大区直接重登，不处理了
            if (_subWnd.getAreaTag() != CDNSetting_AreaInfo.instance.areaTag)
            {
                //切换其他区服会导致游戏事件变化及网络延迟
                NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.server_switchAreaTip_none)
                    , TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                    , null
                    , TextTranslate.instance.getLanguage(TransKeyConst.confirm)
                    , () =>
                    {
                        // _onSelectedArea(_subWnd);

                        //设置选中的大区
                        CDNSetting_AreaInfo.instance.areaId = CDNSetting_AreaInfo.instance.getAreaIdByTag(_subWnd.getAreaTag());

                        //重置选择的登录服务器id
                        LoginTokenSetting.instance.removeUidServerId(Game.instance.uid);

                        //清除cdn配置
                        GameInit_CDN.instance.forceClearAreaCdn();

                        //退出服务器列表节点
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_Login_Server_List);

                        //开始重登
                        Game.instance.relogin();
                    });
            }
            else
            {
                _onSelectedArea(_subWnd);
            }
            
        }
        
        /// <summary>
        /// 选中大区的回调处理
        /// </summary>
        /// <param name="_subWnd"></param>
        private void _onSelectedArea(NPGGUIWndServerAreaItem _subWnd)
        {
            if (null == _subWnd || _m_selectedWnd == _subWnd)
                return;

            if (null != _m_selectedWnd)
                _m_selectedWnd.setSelected(false);

            _m_selectedWnd = _subWnd;
            _m_selectedWnd.setSelected(true);

            if (_m_onSelectedArea != null) 
                _m_onSelectedArea(_m_selectedWnd.getAreaTag());
        }
    }
}