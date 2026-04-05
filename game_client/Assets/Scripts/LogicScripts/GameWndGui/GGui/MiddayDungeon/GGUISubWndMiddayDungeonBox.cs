using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 
	/// </summary>
	public class GGUISubWndMiddayDungeonBox : _ANPGGUIBasicSubWnd<GGUIMonoMiddayDungeonBox>
	{
		private Action _m_closeAction;
		private List<MiddayDungeonBoxInfo> _m_boxInfos;
		private GGUIWndMiddayDungeonBoxItemContainer _m_itemContainer;
		public GGUISubWndMiddayDungeonBox(GGUIMonoMiddayDungeonBox _wnd, Action _close) : base(_wnd)
		{
			_m_closeAction = _close;
			initWnd();
		}

		protected override void _onShowWnd()
		{
			_refreshWnd();
		}

		protected override void _onHideWnd()
		{
		}

		protected override void _onReset()
		{
			
		}

		protected override void _onDiscard()
		{
		}

		protected override void _onWndInitDone()
		{
			if(null == wnd)
				return;
			if (wnd.itemContainer != null)
			{
				_m_itemContainer = new GGUIWndMiddayDungeonBoxItemContainer(wnd.itemContainer);
			}
			ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
		}
		
		/// <summary>
		/// 刷新界面
		/// </summary>
		private void _refreshWnd()
		{
			if(null == wnd)
				return;
			NPPlayer.instance.middayDungeonComp.reqMiddayDungeonBoxList(Common.DungeonEnum.EDungeonBoxType.MIDDAY, (_isSuc, _infos) =>
			{
				 if (_m_boxInfos == null)
					 _m_boxInfos = new List<MiddayDungeonBoxInfo>();
				 _m_boxInfos.Clear();
				 if (_infos != null)
					 for (var i = _infos.Count - 1; i >= 0; i--)
						 _m_boxInfos.Add(_infos[i]);
				 _refreshBox();
			});

		}
		
		

		private void _refreshBox()
		{
			if (_m_itemContainer != null) 
				_m_itemContainer.showItemList(_m_boxInfos);
		}

		private void _onBtnCloseClick(GameObject _)
		{
			_m_closeAction?.Invoke();
		}
	}
}