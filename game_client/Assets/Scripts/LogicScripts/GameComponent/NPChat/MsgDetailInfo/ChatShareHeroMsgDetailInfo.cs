using ALPackage;
using NPEnum;
using ChatPackage;
using Common.HeroObj;
using Common.NpChatObj;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 聊天骑士分享信息
    /// </summary>
    public class ChatShareHeroMsgDetailInfo : _AMsgDetailInfo<NPCommon_ChatContent_HeroShare, NPCommon_ChatPlayerContent>, _INPChatMiniShowInfo
    {
        public ChatShareHeroMsgDetailInfo() : base((int) ENPChatMsgType.SHARE_HERO)
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
        public void setContent(HeroInfo _heroInfo)
        {
            content.setHeroId(_heroInfo.id);
            content.setSkinId(_heroInfo.curSkinId);
            content.setLevel((int)_heroInfo.level);
            content.setStep((int)_heroInfo.curStep);
            content.setStar((int)_heroInfo.star);
            content.setPower(_heroInfo.power);
            content.setTalent(_heroInfo.getTotalTalent());
            
            foreach (HeroTalentSkillInfo talentInfo in _heroInfo.heroTalentSkillInfoMgr.talentSkillInfoList)
            {
                Hero_TalentSkillInfo talentSkillInfo = new Hero_TalentSkillInfo();
                talentSkillInfo.setLevel(talentInfo.level);
                talentSkillInfo.setTealentSkillId(talentInfo.talentSkillId);
                content.addTalentSkillList(talentSkillInfo);                
            }
        }

        public string getMiniSender()
        {
            return sender.getCName();
        }

        public string getMiniContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.chat_share_hero_str,GCommon.getItemName(ENPItemType.HERO,content.getHeroId()));
        }
    }
}