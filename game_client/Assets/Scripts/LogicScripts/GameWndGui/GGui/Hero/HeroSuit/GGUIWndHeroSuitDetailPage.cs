using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 伙伴套系详情页签
    /// </summary>
    public class GGUIWndHeroSuitDetailPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroSuitDetailPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //伙伴信息
        private HeroInfo _m_heroInfo;
        //套系伙伴列表
        private GGUIWndHeroConsortSimpleIconContainer _m_heroContainer;
        //套系技能列表
        private GGUIWndHeroSuitSkillContainer _m_suitSkillContainer;

        public GGUIWndHeroSuitDetailPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }
        
        protected override void _onHideWnd()
        {
            _m_heroContainer?.hideWnd();
            _m_suitSkillContainer?.hideWnd();
        }
        
        protected override void _onReset()
        {
            _m_heroContainer?.resetWnd();
            _m_suitSkillContainer?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            _m_heroContainer?.discard();
            _m_heroContainer = null;

            _m_suitSkillContainer?.discard();
            _m_suitSkillContainer = null;

            if (wnd == null)
                return;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroContainer != null)
                _m_heroContainer = new GGUIWndHeroConsortSimpleIconContainer(wnd.monoHeroContainer);

            if (wnd.monoSuitSkillContainer != null)
                _m_suitSkillContainer = new GGUIWndHeroSuitSkillContainer(wnd.monoSuitSkillContainer);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId"></param>
        public void setInfo(HeroInfo _heroInfo)
        {
            _m_heroInfo = _heroInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            HeroSuitInfo suitInfo = NPPlayer.instance.heroComponent.getHeroSuitInfo(_m_heroInfo.heroRefObj.suit_id);
            if (suitInfo == null || suitInfo.suitRef == null)
            {
                Debug.LogError($"未获取到套系数据，suitId:{_m_heroInfo.heroRefObj.suit_id}");
                return;
            }

            //设置标题描述
            ALUGUICommon.setLabelTxt(wnd.txtSuitTitle, TextTranslate.instance.getLanguage(suitInfo.suitRef.halo_name));
            ALUGUICommon.setLabelTxt(wnd.txtSuitDesc, TextTranslate.instance.getLanguage(suitInfo.suitRef.desc, suitInfo.suitRef.desc_args));

            //设置伙伴列表
            if (_m_heroContainer != null)
            {
                List<long> idList = new List<long>();
                for (int i = 0; i < suitInfo.suitRef.heroRefList.Count; i++)
                {
                    idList.Add(suitInfo.suitRef.heroRefList[i].id);
                }
                idList.Sort((_a,_b)=>_a.CompareTo(_b));
                _m_heroContainer.showWnd();
                _m_heroContainer.showItemList(idList, EHeroConsortSimpleIconShowType.HERO);
            }

            //设置技能列表
            if (_m_suitSkillContainer != null)
            {
                _m_suitSkillContainer.showWnd();
                _m_suitSkillContainer.showItemList(suitInfo,_m_heroInfo, suitInfo.suitRef.halo_suit_skill_id_list);
            }
        }

    }
}