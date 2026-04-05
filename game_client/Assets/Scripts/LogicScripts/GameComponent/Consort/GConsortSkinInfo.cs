using System;
using System.Collections.Generic;
using Common.ConsortObj;
using NPEnum;

namespace GOE
{
    public class GConsortSkinInfo : _IConsortSkinShowInfo
    {
        /// <summary>
        /// 皮肤ID
        /// </summary>
        private long _m_skinId;
        /// <summary>
        /// 皮肤等级
        /// </summary>
        private int _m_skinLvl;
        
        private GConsortSkinRefObj _m_skinRef;
        private GConsortSkinLvlRefObj _m_skinLvlRef;
        private GConsortSkinLvlRefObj _m_nextSkinLvlRef;
        
        public GConsortSkinInfo(Consort_SkinInfo _skinInfo)
        {
            _m_skinId = _skinInfo.getSkinId();
            _m_skinLvl = _skinInfo.getLvl();
            _m_skinRef = GRefdataCoreMgr.instance.consortSkinRefCore.getRef(_m_skinId);
            _m_skinLvlRef = GRefdataCoreMgr.instance.getSkinLvlRef(_m_skinId, _m_skinLvl);
            _m_nextSkinLvlRef = GRefdataCoreMgr.instance.getSkinLvlRef(_m_skinId, _m_skinLvl + 1);

        }

        public GConsortSkinLvlRefObj skinLvlRef { get => _m_skinLvlRef; }
        public GConsortSkinLvlRefObj nextSkinLvlRef { get => _m_nextSkinLvlRef; }
        public long skinId { get => _m_skinId; }
        public GConsortSkinRefObj skinRefObj { get => _m_skinRef; }
        public NPGTextureIndex consortHeadIcon { get { return skinRefObj?.consort_head; } }
        public NPGTextureIndex consortCardImage { get { return skinRefObj?.consort_card_image; } }
        public NPGTextureIndex skinIcon { get { return GCommon.getItemTexIcon(ENPItemType.CONSORT_SKIN, _m_skinId); } }
        public NPGTextureIndex skinCardImg { get { return skinRefObj?.skin_card_img; } }
        public NPGGoIndex tdShow { get { return skinRefObj?.td_show; } }
        public NPGGoIndex tdBgIndex { get { return skinRefObj?.td_bg_index; } }
        public int skinLvl { get => _m_skinLvl; }

        /// <summary>
        /// 妃子技能更新
        /// </summary>
        /// <param name="_skinInfo"></param>
        public void updateConsortSkinInfo(Consort_SkinInfo _skinInfo)
        {
            if(_skinInfo == null)
                return;
            
            if (_m_skinId != _skinInfo.getSkinId())
            {
                return;
            }
            _m_skinLvl = _skinInfo.getLvl();
            _m_skinLvlRef = GRefdataCoreMgr.instance.getSkinLvlRef(_m_skinId, _m_skinLvl);
            _m_nextSkinLvlRef = GRefdataCoreMgr.instance.getSkinLvlRef(_m_skinId, _m_skinLvl + 1);
        }

        public void updateConsortSkinInfo(GConsortSkinInfo _skinInfo)
        {
            if(_skinInfo == null)
                return;
            
            if (_m_skinId != _skinInfo.skinId)
            {
                return;
            }
            _m_skinLvl = _skinInfo.skinLvl;
            _m_skinLvlRef = _skinInfo.skinLvlRef;
            _m_nextSkinLvlRef = _skinInfo.nextSkinLvlRef;
        }
    }
}