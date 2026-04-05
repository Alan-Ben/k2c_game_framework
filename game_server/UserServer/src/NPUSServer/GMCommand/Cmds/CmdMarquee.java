package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

import java.util.Arrays;
import java.util.Collections;

@ACommander(comment = "跑马灯相关命令", name = "marquee")
public class CmdMarquee extends UsCmdBase
{
    @ACommand(comment = "增加带参数跑马灯[配表id][参数列表 用;分号间隔]")
    public String addMarquee(long _marqueeId, String _paramList)
    {
    	getUserServer().getMarqueeMgr().cmdAddMarquee(_marqueeId, Arrays.asList(_paramList.split(";")));
        return "ok";
    }

    @ACommand(comment = "增加长文本带空格跑马灯[配表id][文本 需要编码为base64传入]")
    public String addMarqueeBase64(long _marqueeId, String _rawData)
    {
        //base64解码
        String data = new String(java.util.Base64.getDecoder().decode(_rawData));
    	getUserServer().getMarqueeMgr().cmdAddMarquee(_marqueeId, Collections.singletonList(data));
        return "ok";
    }

    @ACommand(comment = "增加跑马灯不传参[配表id]")
    public String addMarqueeB(long _marqueeId)
    {
    	getUserServer().getMarqueeMgr().cmdAddMarquee(_marqueeId, null);
        return "ok";
    }
}
