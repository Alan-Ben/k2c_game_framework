using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 藏品图鉴预览Scene
    /// </summary>
    public class GMainGUIMainSceneEquipPreview : _ANPGMainGUIAddSceneResBar<GGUIWndEquipDetail>
    {
        private static GMainGUIMainSceneEquipPreview _g_instance = new GMainGUIMainSceneEquipPreview();
        [NotNull] 
        public static GMainGUIMainSceneEquipPreview instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GMainGUIMainSceneEquipPreview();
                return _g_instance;
            }
        }

        //当前展示的藏品列表
        private List<EquipRefObj> _m_lEquipRefList = new List<EquipRefObj>();
        //当前展示的下标
        private int _m_iCurShowIndex;

        protected override GGUIWndEquipDetail _m_wnd { get { return GGUIWndEquipDetail.instance; } }

        public override void _dealShowScene(Action _delegate)
        {
            base._dealShowScene(_delegate);
            setShowIdex(_m_iCurShowIndex);
        }

        /// <summary>
        /// 初始化显示数据
        /// </summary>
        /// <param name="_equipInfoList"></param>
        /// <param name="_idx"></param>
        public void initShowData(List<EquipRefObj> _equipInfoList, int _idx)
        {
            _m_lEquipRefList = _equipInfoList;
            _m_iCurShowIndex = _idx;
        }

        //显示上一个或下一个
        public void showPre()
        {
            if (_m_lEquipRefList == null || _m_lEquipRefList.Count <= 0)
                return;

            setShowIdex(_m_iCurShowIndex - 1);
        }
        public void showNext()
        {
            if (_m_lEquipRefList == null || _m_lEquipRefList.Count <= 0)
                return;

            setShowIdex(_m_iCurShowIndex + 1);
        }

        public void setShowIdex(int _idx)
        {
            //如果无数据则不处理
            if (_m_lEquipRefList == null || _m_lEquipRefList.Count <= 0)
                return;

            //设置索引
            _m_iCurShowIndex = (_idx + _m_lEquipRefList.Count) % _m_lEquipRefList.Count;

            EquipRefObj equipRef = _m_lEquipRefList[_m_iCurShowIndex];
            if (equipRef == null)
                return;

            //设置数据
            GGUIWndEquipDetail.instance.setInfo(
                null,
                equipRef,
                _m_iCurShowIndex == 0,
                _m_iCurShowIndex == _m_lEquipRefList.Count - 1,
                showPre,
                showNext);
        }
    }
}
