
using System;
using JetBrains.Annotations;

namespace GOE
{
    public class GNodeChapterMap : _AGNodeMainSub
    {
        //显示章节解锁动画的章节索引
        private bool _m_showNewChapterUnlock = false;
        //上次进入的node索引
        private int _m_lastNodeIndex;
        
        [NotNull] private ChapterMapViewMgr _m_chapterMapView;

        
        public GNodeChapterMap(bool _showNewChapterUnlock = false) 
            : base(EMainFunctionTabType.CHAPTER, UINodeTagConst_Chapter.C_CHAPTER_MAP)
        {
            _m_showNewChapterUnlock = _showNewChapterUnlock;
            _m_lastNodeIndex = NPPlayer.instance.chapterComp.curNodeIndex;
            _m_chapterMapView = new ChapterMapViewMgr();
        }
        
        /// <summary>
        /// 允许弹出的提示窗口类型，默认都不弹
        /// </summary>
        public override ENoticeType enableNoticeType { get { return ENoticeType.CHAPTER_MAP; } }
        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return true; } }

        public override void onEnterQueue()
        {
        }
        
        public override void onClose()
        {
        }

        public void setNeedShowNewChapterUnlock()
        {
            _m_showNewChapterUnlock = true;
        }
        
        protected override void _doEnterNode(Action _triggerEnterDone)
        {
            GUISceneMain.instance.showMainWnd(GGUIWndChapterMap.instance, () =>
            {
                //如果要展示新解锁章节动画
                if(_m_showNewChapterUnlock)
                {
                    GGUIWndChapterMap.instance.showNodeUnlock(NPPlayer.instance.chapterComp.chapterRefObj, 0);
                }
                //如果不是第一次进入章节地图，并且当前node索引和上次进入的node索引不一样，说明是从某个node退出回到章节地图的
                else if (_m_lastNodeIndex != NPPlayer.instance.chapterComp.curNodeIndex)
                {
                    GGUIWndChapterMap.instance.showNodeUnlock(NPPlayer.instance.chapterComp.chapterRefObj, NPPlayer.instance.chapterComp.curNodeIndex);
                }
                else
                {
                    GGUIWndChapterMap.instance.showChapterMap(NPPlayer.instance.chapterComp.chapterRefObj);
                }

                //记录当前node索引
                _m_lastNodeIndex = NPPlayer.instance.chapterComp.curNodeIndex;
                //重置新章节解锁标记位
                _m_showNewChapterUnlock = false;
                
                _m_chapterMapView.init(_triggerEnterDone);
                
                if (_triggerEnterDone != null) 
                    _triggerEnterDone();
            });
        }
        
        protected override void _doQuitNode()
        {
            _m_chapterMapView.discard();

        }
    }
}