
using ALPackage;
using NPEnum;
using ChatPackage;
using Common;
using Common.NpChatObj;
using CommonEnum;
using GOE.BonusSpace;

namespace GOE
{
    /// <summary>
    /// 聊天子嗣分享信息
    /// </summary>
    public class ChatShareChildMsgDetailInfo : _AMsgDetailInfo<NPCommon_ChatContent_ChildShare, NPCommon_ChatPlayerContent>, _INPChatMiniShowInfo
    {
        public ChatShareChildMsgDetailInfo() : base((int) ENPChatMsgType.SHARE_CHILD)
        {
        }
        
        /// <summary>
        /// 是否是我自己的消息
        /// </summary>
        public override bool isMyMsg { get { return sender.getCid() == NPPlayer.instance.playerInfo.CID; } }
        
        /// <summary>
        /// 设置消息发送者为我自己
        /// </summary>
        public void setSenderIsMyself()
        {
            sender.setCName(NPPlayer.instance.playerInfo.PlayerName);
            sender.setCid(NPPlayer.instance.playerInfo.CID);
            sender.setIconId(NPPlayer.instance.playerInfo.curIcon.baseData.sub_id);
            sender.setIconBgkId(NPPlayer.instance.playerInfo.curIconBgk.baseData.sub_id);
            sender.setBubbleId(NPPlayer.instance.playerInfo.getCurrentBubbleId());
            sender.setVipLvl((int)NPPlayer.instance.playerInfo.getCurrentVIPLvl());
            sender.setPlayerSkinId(NPPlayer.instance.playerInfo.getCurrentSkinId());
            sender.setCurTitle(NPPlayer.instance.titleComp.curTitle);
            sender.setIsShow(NPPlayer.instance.titleComp.isShowOthers);
        }

        /// <summary>
        /// 设置消息内容...
        /// </summary>
        public void setContent(ChildInfo _childInfo)
        {
            if (_childInfo == null)
                return;

            content.setChildName(_childInfo.name);
            content.setConsortId(_childInfo.guardianRef != null ? _childInfo.guardianRef.id : 0);
            content.setChildLevel(_childInfo.level);
            content.setChildStep(_childInfo.step);
            content.setChildResId(_childInfo.resRef != null ? _childInfo.resRef.id : 0);
            content.setChildCareerId(_childInfo.careerRef != null ? _childInfo.careerRef.career_id : 0);
            content.setChildQualityId(_childInfo.qualityRef != null ? _childInfo.qualityRef.id : 0);
            content.setAttrType(_childInfo.attrRef != null ? _childInfo.attrRef.type : ESpecAttrType.NONE);
            content.setEarnings(_childInfo.earnings);
            content.setEducationExpValue(_childInfo.getEducationExpValue());
            content.setEducatingBaseAwards(NPPlayer.instance?.playerInfo?.curLevelRef?.child_educate_get_hero_exp ?? 0);
            content.setChildQualityBonus(_childInfo.initStudyBonus);
            content.setIsSuper(_childInfo.isSuper);
            _AUnionBonusMgr bonusMgr = NPPlayer.instance.playerBonusMgr.findMgrByTag(EUnionBonusMgrTag.HERO);
            long heroBonusValue = bonusMgr?.getTotalPropertyBonus(EBonusPropertyType.CHILD_TRAIN_GAIN_PER, _childInfo.bonusJudgeParts) ?? 0;
            content.setHeroBonus(heroBonusValue);
            bonusMgr = NPPlayer.instance.playerBonusMgr.findMgrByTag(EUnionBonusMgrTag.CONSORT);
            long consortBonusValue = bonusMgr?.getTotalPropertyBonus(EBonusPropertyType.CHILD_TRAIN_GAIN_PER, _childInfo.bonusJudgeParts) ?? 0;
            content.setConsortBonus(consortBonusValue);
            content.setCid(NPPlayer.instance.playerInfo?.CID ?? 0);
            content.setIsGraduated(false);
            content.setAdultId(0);
        }

        /// <summary>
        /// 设置消息内容...
        /// </summary>
        public void setContent(AdultInfo _adultInfo)
        {
            if (_adultInfo == null)
                return;

            content.setChildName(_adultInfo.name);
            content.setConsortId(_adultInfo.guardianRef != null ? _adultInfo.guardianRef.id : 0);
            content.setChildLevel(_adultInfo.maxLevel);
            content.setChildStep(_adultInfo.qualityRef != null ? _adultInfo.qualityRef.getMaxStep() : 0);
            content.setChildResId(_adultInfo.resRef != null ? _adultInfo.resRef.id : 0);
            content.setChildCareerId(_adultInfo.careerRef != null ? _adultInfo.careerRef.career_id : 0);
            content.setChildQualityId(_adultInfo.qualityRef != null ? _adultInfo.qualityRef.id : 0);
            content.setAttrType(_adultInfo.attrRef != null ? _adultInfo.attrRef.type : ESpecAttrType.NONE);
            content.setEarnings(_adultInfo.earnings);
            content.setEducationExpValue(_adultInfo.getEducationExpValue());
            content.setEducatingBaseAwards(NPPlayer.instance?.playerInfo?.curLevelRef?.child_educate_get_hero_exp ?? 0);
            content.setChildQualityBonus(_adultInfo.initStudyBonus);
            content.setIsSuper(_adultInfo.isSuper);
            _AUnionBonusMgr bonusMgr = NPPlayer.instance.playerBonusMgr.findMgrByTag(EUnionBonusMgrTag.HERO);
            long heroBonusValue = bonusMgr?.getTotalPropertyBonus(EBonusPropertyType.CHILD_TRAIN_GAIN_PER, _adultInfo.bonusJudgeParts) ?? 0;
            content.setHeroBonus(heroBonusValue);
            bonusMgr = NPPlayer.instance.playerBonusMgr.findMgrByTag(EUnionBonusMgrTag.CONSORT);
            long consortBonusValue = bonusMgr?.getTotalPropertyBonus(EBonusPropertyType.CHILD_TRAIN_GAIN_PER, _adultInfo.bonusJudgeParts) ?? 0;
            content.setConsortBonus(consortBonusValue);
            content.setCid(NPPlayer.instance.playerInfo?.CID ?? 0);
            content.setIsGraduated(true);
            content.setAdultId(_adultInfo.id);
        }

        public string getMiniSender()
        {
            return sender.getCName();
        }

        public string getMiniContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.chat_share_child_str,content.getChildName());
        }
    }
}