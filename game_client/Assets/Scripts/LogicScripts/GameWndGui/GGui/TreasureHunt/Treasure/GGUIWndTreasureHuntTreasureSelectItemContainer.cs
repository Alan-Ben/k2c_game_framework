using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
	/// <summary>
	/// 奇物选择Item容器（单选）
	/// </summary>
	public class GGUIWndTreasureHuntTreasureSelectItemContainer : _AGGUISubWndCommonContainer<GGUIMonoTreasureHuntTreasureSelectItem, GGUIMonoTreasureHuntTreasureSelectItemContainer, GGUIWndTreasureHuntTreasureSelectItem>
	{
		private List<_ITreasureHuntTreasureInfo> _m_lTreasureList; // 数据源
		private int _m_iSelectedIndex; // 当前奇物的索引

		public GGUIWndTreasureHuntTreasureSelectItemContainer(GGUIMonoTreasureHuntTreasureSelectItemContainer containerMono) : base(containerMono)
		{
			initWnd();
		}

		public event Action<_ITreasureHuntTreasureInfo> onSelectTreasure; // 当选中某个奇物

		protected override void _onDiscard()
		{
			onSelectTreasure = null;
			base._onDiscard();
		}

		protected override GGUIWndTreasureHuntTreasureSelectItem _createItemWnd(GGUIMonoTreasureHuntTreasureSelectItem itemMono)
		{
			GGUIWndTreasureHuntTreasureSelectItem itemWnd = new GGUIWndTreasureHuntTreasureSelectItem(itemMono);
			itemWnd.onSelectItem += _onClickItem;
			return itemWnd;
		}

		protected override void _discardItem(GGUIWndTreasureHuntTreasureSelectItem itemWnd)
		{
			if (itemWnd != null)
				itemWnd.onSelectItem -= _onClickItem;
			base._discardItem(itemWnd);
		}

		protected override void _refreshItemWnd(GGUIWndTreasureHuntTreasureSelectItem itemWnd, int index)
		{
			if (_m_lTreasureList == null || index < 0 || index >= _m_lTreasureList.Count)
				return;

			_ITreasureHuntTreasureInfo treasureInfo = _m_lTreasureList.SafeGet(index);
			itemWnd.setData(treasureInfo);
			itemWnd.setSelect(_m_iSelectedIndex == index);
		}

		/// <summary>
		/// 设置数据源并刷新
		/// </summary>
		public void setData(List<_ITreasureHuntTreasureInfo> treasureList, _ITreasureHuntTreasureInfo selectedTreasure = null)
		{
			_m_lTreasureList = treasureList;
			_m_iSelectedIndex = _getTreasureIndex(selectedTreasure);
			
			int count = _m_lTreasureList?.Count ?? 0;
			refreshWnd(count);
			
			// 下一帧进行滚动
			ALCommonTaskController.CommonActionAddNextFrameTask(() =>
			{
				if(wnd == null || !isShow)
					return;

				scrollToIndex(_m_iSelectedIndex, 0);
			});
		}

		/// <summary>
		/// 设置数据源并刷新
		/// </summary>
		public void setData(List<_ITreasureHuntTreasureInfo> treasureList, int _selectedIndex)
		{
			_m_lTreasureList = treasureList;
			_m_iSelectedIndex = _selectedIndex;
			
			int count = _m_lTreasureList?.Count ?? 0;
			refreshWnd(count);
			
			// 下一帧进行滚动
			ALCommonTaskController.CommonActionAddNextFrameTask(() =>
			{
				if(wnd == null || !isShow)
					return;

				scrollToIndex(_m_iSelectedIndex, 0);
			});
		}

		private void _onClickItem(GGUIWndTreasureHuntTreasureSelectItem itemWnd)
		{
			if (itemWnd == null || itemWnd.treasureInfo == null)
				return;

			int preSelectIndex = _m_iSelectedIndex;
			_m_iSelectedIndex = _getTreasureIndex(itemWnd.treasureInfo);
			if(_m_iSelectedIndex == preSelectIndex)
				return;

			refreshItem(preSelectIndex);
			refreshItem(_m_iSelectedIndex);
			
			onSelectTreasure?.Invoke(itemWnd.treasureInfo);
		}

		/// <summary>
		/// 移动ScrollRect到指定下标位置
		/// </summary>
		/// <param name="index">目标索引</param>
		/// <param name="smoothTime">平滑时间，0表示立即移动</param>
		public void scrollToIndex(int index, float smoothTime = 0.25f)
		{
			if (wnd?.scrollRect == null || _m_lTreasureList == null)
				return;

			if (index < 0 || index >= _m_lTreasureList.Count)
				return;

			// 计算目标位置的归一化坐标
			float normalizedPosition = 0f;
			if (_m_lTreasureList.Count > 1)
			{
				normalizedPosition = index / (float)(_m_lTreasureList.Count - 1);
			}

			// 根据ScrollRect的方向选择移动方式
			if (wnd.scrollRect.vertical)
			{
				// 垂直滚动：顶部为1，底部为0
				normalizedPosition = 1f - normalizedPosition;

				if (smoothTime <= 0f)
				{
					// 立即移动
					wnd.scrollRect.verticalNormalizedPosition = normalizedPosition;
				}
				else
				{
					// 平滑移动
					new ScrollerSmoothMoveTaskVertical(this, normalizedPosition, smoothTime).deal();
				}
			}
			else if (wnd.scrollRect.horizontal)
			{
				// 水平滚动：左侧为0，右侧为1
				if (smoothTime <= 0f)
				{
					// 立即移动
					wnd.scrollRect.horizontalNormalizedPosition = normalizedPosition;
				}
				else
				{
					// 平滑移动
					new ScrollerSmoothMoveTaskHorizontal(this, normalizedPosition, smoothTime).deal();
				}
			}
		}

		/// <summary>
		/// 移动ScrollRect到指定奇物位置
		/// </summary>
		/// <param name="treasureInfo">目标奇物信息</param>
		/// <param name="smoothTime">平滑时间，0表示立即移动</param>
		public void scrollToTreasure(_ITreasureHuntTreasureInfo treasureInfo, float smoothTime = 0.25f)
		{
			if (treasureInfo == null)
				return;

			int index = _getTreasureIndex(treasureInfo);
			if (index >= 0)
			{
				scrollToIndex(index, smoothTime);
			}
		}

		/// <summary>
		/// 获取奇物索引
		/// </summary>
		/// <param name="treasureInfo"></param>
		/// <returns></returns>
		private int _getTreasureIndex(_ITreasureHuntTreasureInfo treasureInfo)
		{
			if (_m_lTreasureList == null || treasureInfo == null)
				return -1;

			for (int i = 0; i < _m_lTreasureList.Count; i++)
			{
				if (_m_lTreasureList[i] != null && _m_lTreasureList[i].treasureId == treasureInfo.treasureId)
					return i;
			}

			return -1; 
		}
	}
}

