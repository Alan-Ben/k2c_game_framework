using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndStageGoalOverviewSmallStepPointContainer : _AGGUISubWndCommonContainer<GGUIMonoStageGoalOverviewSmallStepPointContainerItem, GGUIMonoStageGoalOverviewSmallStepPointContainer, GGUISubWndStageGoalOverviewSmallStepPointContainerItem>
    {
        private StageGoalBigStepRefObj _m_refObj;
        private long _m_curStageStepId;
        
        private float _m_width;
        private float _m_itemSpace;
        
        
        public GGUISubWndStageGoalOverviewSmallStepPointContainer(GGUIMonoStageGoalOverviewSmallStepPointContainer _containerMono) 
            : base(_containerMono)
        {
            initWnd();
        }


        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd == null)
                return;

            if (wnd.itemContainer != null && wnd.itemContainer.transform is RectTransform transform)
                _m_width = transform.rect.width;
        }


        protected override GGUISubWndStageGoalOverviewSmallStepPointContainerItem _createItemWnd(GGUIMonoStageGoalOverviewSmallStepPointContainerItem _itemMono)
        {
            return new GGUISubWndStageGoalOverviewSmallStepPointContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndStageGoalOverviewSmallStepPointContainerItem _itemWnd, int _index)
        {
            if (_m_refObj == null)
                return;
            
            ALUGUICommon.setUIPos(_itemWnd.rectTransform, _m_itemSpace * (_index + 1), 0);
            _itemWnd.refreshWnd(_m_curStageStepId - _m_refObj.begins_from_small_step > _index);
        }
        

        public void refreshWnd(StageGoalBigStepRefObj _refObj, long _curStageStepId)
        {
            _m_refObj = _refObj;
            _m_curStageStepId = _curStageStepId;
            int smallStepNum = 0;
            if (_m_refObj != null)
            {
                smallStepNum = _m_refObj.end_small_step - _m_refObj.begins_from_small_step + 1;
                _m_itemSpace = _m_width / smallStepNum;
            }
            
            refreshWnd(smallStepNum);
        }
    }
}