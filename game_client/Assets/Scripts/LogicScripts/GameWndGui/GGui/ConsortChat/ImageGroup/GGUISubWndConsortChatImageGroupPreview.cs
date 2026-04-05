using ALPackage;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 
	/// </summary>
	public class GGUISubWndConsortChatImageGroupPreview : _ANPGGUIBasicSubWnd<GGUIMonoConsortChatImageGroupPreview>
	{

		private long _m_imageGroupId;
		private long _m_imageGroupUiPathId;
		private ConsortChatImageGroupRefObj _m_imageGroupRef;

		private GGUIWndConsortChatImageGroup _m_wImageGroup;
		
		public GGUISubWndConsortChatImageGroupPreview(GGUIMonoConsortChatImageGroupPreview _wnd) : base(_wnd)
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
			if (wnd == null) return;
			ALUGUICommon.uncombineBtnClick(wnd.btnClick, _OnBtnClick);
		}

		protected override void _onWndInitDone()
		{
			if(null == wnd)
				return;
			
			ALUGUICommon.combineBtnClick(wnd.btnClick, _OnBtnClick);
		}

		public void setInfo(long _imageGroupId, long _imageGroupPathId)
		{
			_m_imageGroupUiPathId = _imageGroupPathId;
			_m_imageGroupId = _imageGroupId;
			_m_imageGroupRef = GRefdataCoreMgr.instance.consortChatImageGroupRefCore.getRef(_imageGroupId);
			_refreshWnd();
		}
		/// <summary>
		/// 刷新界面
		/// </summary>
		private void _refreshWnd()
		{
			if(null == wnd || _m_imageGroupRef == null)
				return;
			
			if (_m_imageGroupRef != null && wnd.imageGroupParent != null)
			{
				if (_m_wImageGroup != null)
				{
					_m_wImageGroup.showWnd();
				}
				else
				{
					ConsortMomentImageData imageData = new ConsortMomentImageData(_m_imageGroupRef.shot_type, _m_imageGroupRef.bg_img_id, _m_imageGroupRef.consort_img_id);
					_m_wImageGroup = new GGUIWndConsortChatImageGroup(_m_imageGroupUiPathId, imageData, wnd.imageGroupParent);
					_m_wImageGroup.load(_m_wImageGroup.showWnd);
				}
			}
		}

		private void _OnBtnClick(GameObject _)
		{
			if (_m_imageGroupRef == null)
				return;

			ConsortMomentImageData imageData = new ConsortMomentImageData(_m_imageGroupRef.shot_type, _m_imageGroupRef.bg_img_id, _m_imageGroupRef.consort_img_id);
			GGUIWndConsortChatImageGroupDetail.instance.setInfo(imageData, _m_imageGroupUiPathId);
			

			QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndConsortChatImageGroupDetail.instance, GGUIWndConsortChatImageGroupDetail.instance.showWnd, UINodeTagConst.C_CONSORT_CHAT_IMAGE_DETAIL);
		}
	}
}