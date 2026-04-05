package NPGameRes.GameObjs.Battle;

import java.util.ArrayList;

public class NPSkinPropertyRefObj extends _IALBasicRefObj
{

    public static abstract class WCGBaseReplaceInfo
    {
        public long ref_id;
    }

    public static class WCGAudioIDReplaceInfo extends WCGBaseReplaceInfo
    {
        //被替换的音效索引ID
        public long replace_audio_id;

        public void parse(String sValue)
        {
            String[] info = sValue.split(":");
            this.ref_id = Long.parseLong(info[0]);
            this.replace_audio_id = Long.parseLong(info[1]);
        }
    }

    //特效替换信息对象
    public static class WCGSfxIDReplaceInfo extends WCGBaseReplaceInfo
    {
        //被替换的特效ID
        public long replace_sfx_id;

        public void parse(String sValue)
        {
            String[] info = sValue.split(":");
            this.ref_id = Long.parseLong(info[0]);
            this.replace_sfx_id = Long.parseLong(info[1]);
        }
    }

    //皮肤替换信息对象(召唤物、前置建筑、占位、进化对象等)
    public class WCGActorSkinReplaceInfo extends WCGBaseReplaceInfo
    {
        //被替换的皮肤参数ID
        public long replace_skin_id;

        public void parse(String sValue)
        {
            String[] info = sValue.split(":");
            this.ref_id = Long.parseLong(info[0]);
            this.replace_skin_id = Long.parseLong(info[1]);
        }
    }

    public long _refId()
    {
        return skin_id;
    }

    public long skin_id;//皮肤索引ID
    public long detal_id;//关联皮肤属性ID
    public long actor_id;//对应的ActorID
    public String model;
    public String low_model;
    public Boolean has_low_res;
    public ArrayList<WCGAudioIDReplaceInfo> replace_audio_list = new ArrayList<WCGAudioIDReplaceInfo>();
    public ArrayList<WCGSfxIDReplaceInfo> replace_sfx_list = new ArrayList<WCGSfxIDReplaceInfo>();
    public ArrayList<WCGActorSkinReplaceInfo> replace_actor_skin_list = new ArrayList<WCGActorSkinReplaceInfo>();

    //根据ActorID获取对应的应该被替换的目标对象皮肤参数ID
    public long getReplaceActorSkinID(long _actorID)
    {
        if (replace_actor_skin_list == null)
            return 0;

        WCGActorSkinReplaceInfo tarInfo;
        for (int i = 0; i < replace_actor_skin_list.size(); i++)
        {
            tarInfo = replace_actor_skin_list.get(i);

            if (tarInfo == null)
                continue;

            if (tarInfo.ref_id == _actorID)
                return tarInfo.replace_skin_id;
        }

        return 0;
    }
}