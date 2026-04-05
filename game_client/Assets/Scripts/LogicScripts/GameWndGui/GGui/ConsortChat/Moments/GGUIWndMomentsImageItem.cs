using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndMomentsImageItem : _ATALBasicUISubWnd<GGUIMonoMomentsImageItem>
    {

        private List<ConsortMomentImageData> _m_imageDataList;
        private int _m_index;
        private ConsortMomentImageData _m_imageData;

        private GGUIWndConsortChatImageGroup _m_wImageGroup;
        
        public GGUIWndMomentsImageItem(GGUIMonoMomentsImageItem _wnd) : base(_wnd)
        {
            initWnd();
        }

      
		protected override void _onShowWnd()
		{
			_refreshWnd();
		}

		protected override void _onHideWnd()
		{
			_m_wImageGroup?.hideWnd();
		}

		protected override void _onReset()
		{
			_m_wImageGroup?.resetWnd();
		}

		protected override void _onDiscard()
		{
			_m_wImageGroup?.discard();
			_m_wImageGroup = null;
			if (wnd == null) return;
			ALUGUICommon.uncombineBtnClick(wnd.btnClick, _OnBtnClick);
		}

		protected override void _onWndInitDone()
		{
			if(null == wnd)
				return;
			
			ALUGUICommon.combineBtnClick(wnd.btnClick, _OnBtnClick);
		}

		public void setInfo(List<ConsortMomentImageData> _imageDataList, int _index)
		{
			_m_imageDataList = _imageDataList;
			_m_index = _index;
			if (_m_imageDataList != null && _m_index >= 0 && _m_index <_m_imageDataList.Count) 
				_m_imageData = _m_imageDataList[_m_index];
			_refreshWnd();
		}
		/// <summary>
		/// 刷新界面
		/// </summary>
		private void _refreshWnd()
		{
			if(null == wnd)
				return;

			if (wnd.imageGroupParent != null)
			{
				if (_m_wImageGroup != null)
				{
					_m_wImageGroup.showWnd();
				}
				else
				{
					if (_m_imageData != null)
					{
						_m_wImageGroup = new GGUIWndConsortChatImageGroup(wnd.momentImageGroupPathId, _m_imageData, wnd.imageGroupParent);
						_m_wImageGroup.load(_m_wImageGroup.showWnd);
					}
				}
			}
		}

		private void _OnBtnClick(GameObject _)
		{
			if (wnd == null)
				return;
			GGUIWndConsortChatImageGroupDetail.instance.setInfos(_m_imageDataList, _m_index, wnd.momentImageGroupPathId);

			QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndConsortChatImageGroupDetail.instance, GGUIWndConsortChatImageGroupDetail.instance.showWnd, UINodeTagConst.C_CONSORT_CHAT_IMAGE_DETAIL);
		}
    }
}
