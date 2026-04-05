using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndConsortChatImageGroupDetail : _ATALBasicUIWnd<GGUIMonoConsortChatImageGroupDetail>
    {
        private static GGUIWndConsortChatImageGroupDetail _g_instance = new GGUIWndConsortChatImageGroupDetail();
    
        public static GGUIWndConsortChatImageGroupDetail instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndConsortChatImageGroupDetail();
                return _g_instance;
            }
        }

        private int _m_iImageIndex;
        private long _m_imageGroupUiPathId;
        private List<ConsortMomentImageData> _m_imageDataList;
        private ConsortMomentImageData _m_curImageData;
        private ConsortChatImageGroupRefObj _m_imageGroupRef;

        private GGUIWndConsortChatImageGroup _m_wImageGroup;
        public GGUIWndConsortChatImageGroupDetail() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoConsortChatImageGroupDetail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoConsortChatImageGroupDetail.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch { get { return true; } }

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
			if (wnd != null)
			{
				ALUGUICommon.uncombineBtnClick(wnd.btnClick, _OnBtnClick);
				ALUGUICommon.uncombineBtnClick(wnd.btnLeft, _onBtnLeftClick);
				ALUGUICommon.uncombineBtnClick(wnd.btnRight, _onBtnRightClick);
			}
		}

		protected override void _onWndInitDone()
		{
			if(null == wnd)
				return;
			
			ALUGUICommon.combineBtnClick(wnd.btnClick, _OnBtnClick);
			ALUGUICommon.combineBtnClick(wnd.btnLeft, _onBtnLeftClick);
			ALUGUICommon.combineBtnClick(wnd.btnRight, _onBtnRightClick);
		}

		public void setInfo(ConsortMomentImageData _imageData, long _imageGroupUiPathId)
		{
			_m_iImageIndex = 0;
			_m_imageGroupUiPathId = _imageGroupUiPathId;
			_m_imageDataList = new List<ConsortMomentImageData>(){_imageData};
			_refreshWnd();
		}

		public void setInfos(List<ConsortMomentImageData> _imageDataList, int _index, long _imageGroupUiPathId)
		{
			_m_iImageIndex = _index;
			_m_imageGroupUiPathId = _imageGroupUiPathId;
			_m_imageDataList = _imageDataList;
			_refreshWnd();
		}

		private void _refreshWnd()
		{
			if (wnd == null || _m_imageDataList == null)
				return;
			_m_iImageIndex =  Mathf.Clamp(_m_iImageIndex, 0, _m_imageDataList.Count - 1);
		
			ConsortMomentImageData imgData = _m_imageDataList[_m_iImageIndex];
			if (imgData != _m_curImageData)
			{
				if (_m_wImageGroup != null)
				{
					_m_wImageGroup.discard();
					_m_wImageGroup = null;
				}

				_m_curImageData = imgData;
			}

			bool hasLeft = _m_iImageIndex > 0;
			bool hasRight = _m_imageDataList.Count > 1 && _m_iImageIndex < _m_imageDataList.Count - 1;
			ALUGUICommon.setGameObjEnable(wnd.hasLeftShowGos, hasLeft);
			ALUGUICommon.setGameObjEnable(wnd.hasRightShowGos, hasRight);
			_refreshImage();
		}
		/// <summary>
		/// 刷新界面
		/// </summary>
		private void _refreshImage()
		{
			if(null == wnd )
				return;
			
			if (wnd.imageGroupParent != null)
			{
				if (_m_wImageGroup != null)
				{
					_m_wImageGroup.showWnd();
				}
				else
				{
					if (_m_curImageData != null)
					{
						_m_wImageGroup = new GGUIWndConsortChatImageGroup(_m_imageGroupUiPathId, _m_curImageData, wnd.imageGroupParent);
						_m_wImageGroup.load(_m_wImageGroup.showWnd);
					}
				}
			}
		}

		private void _OnBtnClick(GameObject _)
		{
			QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_CHAT_IMAGE_DETAIL);
		}

		private void _onBtnLeftClick(GameObject _)
		{
			_m_iImageIndex -= 1;
			_refreshWnd();
		}
		
		private void _onBtnRightClick(GameObject _)
		{
			_m_iImageIndex += 1;
			_refreshWnd();
		}
    }
}