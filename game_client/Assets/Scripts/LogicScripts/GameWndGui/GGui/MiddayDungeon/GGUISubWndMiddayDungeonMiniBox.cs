using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 
	/// </summary>
	public class GGUISubWndMiddayDungeonMiniBox : _ANPGGUIBasicSubWnd<GGUIMonoMiddayDungeonMiniBox>
	{
		private List<MiddayDungeonBoxInfo> _m_boxInfos;
		private int _m_timeDownSer;
		private int _m_curShowBoxIndex;
		private Action _m_onShowBoxWndClick;


		public GGUISubWndMiddayDungeonMiniBox(GGUIMonoMiddayDungeonMiniBox _wnd, Action _onShowBoxWndClick) : base(_wnd)
		{
			_m_onShowBoxWndClick = _onShowBoxWndClick;
			initWnd();
		}

		protected override void _onShowWnd()
		{
			_refreshWnd();
		}

		protected override void _onHideWnd()
		{
			_m_timeDownSer = ALSerializeOpMgr.next();
		}

		protected override void _onReset()
		{
			
		}

		protected override void _onDiscard()
		{
			if (wnd == null)
				return;
			ALUGUICommon.uncombineBtnClick(wnd.btnShowBox, _onBtnShowBoxClick);
		}

		protected override void _onWndInitDone()
		{
			if(null == wnd)
				return;
			ALUGUICommon.combineBtnClick(wnd.btnShowBox, _onBtnShowBoxClick);
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
	            _m_timeDownSer = ALSerializeOpMgr.next();
	            _m_boxInfos = _infos;
	            _refreshBoxInfo(_m_timeDownSer);
            });
		}

		/// <summary>
		/// 每隔几秒刷新一条宝箱信息
		/// </summary>
		private void _refreshBoxInfo(int _timeDownSer)
		{
			if (null == wnd)
				return;
			if (_timeDownSer != _m_timeDownSer)
				return;
			if(_m_boxInfos == null || _m_boxInfos.Count == 0)
			{
				ALUGUICommon.setLabelTxt(wnd.txtBoxInfo, TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_mini_box_no_box_desc));
				return;
			}
			_m_curShowBoxIndex = (_m_curShowBoxIndex + 1) % _m_boxInfos.Count;
			
			MiddayDungeonBoxInfo box = _m_boxInfos[_m_curShowBoxIndex];

			ALUGUICommon.setLabelTxt(wnd.txtBoxInfo, TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_mini_box_info_desc, box?.playerName));

			ALCommonTaskController.CommonActionAddMonoTask(() =>
			{
				_refreshBoxInfo(_timeDownSer);
			}, wnd.showNextBoxInfoTime);
		}
		
		private void _onBtnShowBoxClick(GameObject _)
		{
			if (null == wnd)
				return;
			if (_m_onShowBoxWndClick != null)
			{
				_m_onShowBoxWndClick();
			}
		}
		
	}
}