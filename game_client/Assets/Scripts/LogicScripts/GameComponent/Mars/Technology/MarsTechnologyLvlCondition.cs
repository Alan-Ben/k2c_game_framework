namespace GOE
{
    /// <summary>
    /// 火星科技等级条件
    /// </summary>
    public class MarsTechnologyLvlCondition : _IConditionDescShow
    {
        private MarsTechnologyRefObj _m_rTechnologyRefObj;
        private MarsTechnologyInfo _m_iTechnologyInfo;
        private int _m_iNeedLvl;
        
        public MarsTechnologyLvlCondition(MarsTechnologyRefObj _technologyRefObj, int _needLvl)
        {
            _m_rTechnologyRefObj = _technologyRefObj;
            _m_iTechnologyInfo = NPPlayer.instance.marsComp.technologySubComponent.getTechnologyInfoById(_m_rTechnologyRefObj?.id ?? 0);
            _m_iNeedLvl = _needLvl;
        }
        
        public MarsTechnologyLvlCondition(long _technologyId, int _needLvl)
        {
            _m_rTechnologyRefObj = GRefdataCoreMgr.instance.marsTechnologyRefCore.getRef(_technologyId);
            _m_iTechnologyInfo = NPPlayer.instance.marsComp.technologySubComponent.getTechnologyInfoById(_technologyId);
            _m_iNeedLvl = _needLvl;
        }
        
        public NPGTextureIndex conditionIcon { get { return _m_rTechnologyRefObj?.icon; } }
        public string conditionDesc 
        {
            get
            {
                if (_m_rTechnologyRefObj == null)
                    return string.Empty;

                return TextTranslate.instance.getLanguage(TransKeyConst.mars_technology_levelConditionDesc_str_num, _m_rTechnologyRefObj.transName, _m_iNeedLvl);
            }
        }
        public bool conditionIsEnable { get { return (_m_iTechnologyInfo?.lvl ?? 0) >= _m_iNeedLvl; } }
        public void jumpFunc()
        {
            BaseQueueNode lastNode = QueueMgr.instance._lastNode;
            // 若最后一个Node是科技详情界面，不需要重新添加Node, 直接在该界面刷新数据即可
            if (lastNode != null && lastNode.isEnable && lastNode.nodeTag == UINodeTagConst.C_MARS_TECHNOLOGY_DETAIL)
            {
                GGUIWndMasrTechnologyDetail.instance.setData(_m_rTechnologyRefObj);
            }
            else //最后一个Node不是科技详情界面，则添加新的Node
            {
                GGUIWndMasrTechnologyDetail.instance.setData(_m_rTechnologyRefObj);
                QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndMasrTechnologyDetail.instance, () =>
                {
                    GGUIWndMasrTechnologyDetail.instance.showWnd();   
                }, UINodeTagConst.C_MARS_TECHNOLOGY_DETAIL);
            }
        }
    }
}