package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Player.RefPlayerBubble;
import NPGameRes.Refs.Player.RefPlayerCuteActor;
import NPGameRes.Refs.Player.RefPlayerIcon;
import NPGameRes.Refs.Player.RefPlayerIconBgk;
import NPGameRes.Refs.Title.RefPlayerTitle;
import NPUSServer.GMCommand.UsCmdBase;


@ACommander(comment = "展示道具相关命令", name = "expireItem")
public class CmdExpireItem extends UsCmdBase
{
    @ACommand(comment = "添加气泡框，根据带入时间增加有效时长")
    public String addBubble(long _bubbleId, int _expireTimeS)
    {
        RefPlayerBubble refObj = RefPlayerBubble.getMgr().get(_bubbleId);
        if (null == refObj)
            return "no ref";
        //添加title
        getOwner().gainItem(ENPItemType.BUBBLE, _bubbleId, _expireTimeS, getContext());
        //发送消息返回客户端
        getOwner().sendMsgToGC(getContext().getCollector().toProto());
        return "ok";
    }

    @ACommand(comment = "消除气泡框有效时间")
    public String takeBubbleTime(long _bubbleId, int _takeTimeS)
    {
        //消除时间
        getOwner().getBubbleComponent().reduceItemTime(_bubbleId, _takeTimeS, getContext());

        return "ok";
    }

    @ACommand(comment = "删除气泡框")
    public String delBubble(long _bubbleId)
    {
        //添加title
        getOwner().getBubbleComponent().delItem(_bubbleId);

        return "ok";
    }

    @ACommand(comment = "添加Q版形象，根据带入时间增加有效时长")
    public String addCuteActor(long _cuteActorId, int _expireTimeS)
    {
        RefPlayerCuteActor refObj = RefPlayerCuteActor.getMgr().get(_cuteActorId);
        if (null == refObj)
            return "no cuteActor ref";

        //添加cuteActor
        getOwner().gainItem(ENPItemType.CUTE_ACTOR, _cuteActorId, _expireTimeS, getContext());
        //发送消息返回客户端
        getOwner().sendMsgToGC(getContext().getCollector().toProto());

        return "ok";
    }

    @ACommand(comment = "消除Q版形象有效时间")
    public String takeCuteActorTime(long _cuteActorId, int _takeTimeS)
    {
        //消除时间
        getOwner().getCuteActorComponent().reduceItemTime(_cuteActorId, _takeTimeS, getContext());

        return "ok";
    }

    @ACommand(comment = "删除Q版形象")
    public String delCuteActor(long _cuteActorId)
    {
        getOwner().getCuteActorComponent().delItem(_cuteActorId);
        return "ok";
    }

    @ACommand(comment = "添加头像，根据带入时间增加有效时长")
    public String addIcon(long _iconId, int _expireTimeS)
    {
        RefPlayerIcon refObj = RefPlayerIcon.getMgr().get(_iconId);
        if (null == refObj)
            return "no icon ref";

        //添加title
        getOwner().gainItem(ENPItemType.ICON, _iconId, _expireTimeS, getContext());
        //发送消息返回客户端
        getOwner().sendMsgToGC(getContext().getCollector().toProto());

        return "ok";
    }

    @ACommand(comment = "消除头像有效时间")
    public String takeIconTime(long _iconId, int _takeTimeS)
    {
        //消除时间
        getOwner().getIconComponent().reduceItemTime(_iconId, _takeTimeS, getContext());

        return "ok";
    }

    @ACommand(comment = "删除头像")
    public String delIcon(long _iconId)
    {
        //添加title
        getOwner().getIconComponent().delItem(_iconId);

        return "ok";
    }

    @ACommand(comment = "添加头像框，根据带入时间增加有效时长")
    public String addIconBgk(long _iconBgkId, int _expireTimeS)
    {
        RefPlayerIconBgk refObj = RefPlayerIconBgk.getMgr().get(_iconBgkId);
        if (null == refObj)
            return "no icon bgk ref";

        //添加title
        getOwner().gainItem(ENPItemType.ICON_BGK, _iconBgkId, _expireTimeS, getContext());
        //发送消息返回客户端
        getOwner().sendMsgToGC(getContext().getCollector().toProto());

        return "ok";
    }

    @ACommand(comment = "消除头像框有效时间")
    public String takeIconBgkTime(long _iconBgkId, int _takeTimeS)
    {
        //消除时间
        getOwner().getIconBgkComponent().reduceItemTime(_iconBgkId, _takeTimeS, getContext());

        return "ok";
    }

    @ACommand(comment = "删除头像框")
    public String delIconBgk(long _iconBgkId)
    {
        //添加title
        getOwner().getIconBgkComponent().delItem(_iconBgkId);

        return "ok";
    }

    @ACommand(comment = "添加称号，根据带入时间增加有效时长")
    public String addTitle(long _titleId, int _expireTimeS)
    {
        RefPlayerTitle refObj = RefPlayerTitle.getMgr().get(_titleId);
        if (null == refObj)
            return "no title ref";

        //添加title
        getOwner().gainItem(ENPItemType.TITLE, _titleId, _expireTimeS, getContext());
        //发送消息返回客户端
        getOwner().sendMsgToGC(getContext().getCollector().toProto());

        return "ok";
    }

    @ACommand(comment = "消除称号有效时间")
    public String takeTitleTime(long _titleId, int _takeTimeS)
    {
        //消除时间
        getOwner().getTitleComponent().reduceItemTime(_titleId, _takeTimeS, getContext());

        return "ok";
    }

    @ACommand(comment = "删除称号")
    public String delTitle(long _titleId)
    {
        //添加title
        getOwner().getTitleComponent().delItem(_titleId);

        return "ok";
    }
}
