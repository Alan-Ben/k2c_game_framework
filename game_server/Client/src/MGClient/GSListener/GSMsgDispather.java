package MGClient.GSListener;

import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgDispather extends NPCustomMsgDispatcher
{
    private static GSMsgDispather _g_instance = new GSMsgDispather();

    public static GSMsgDispather getInstance()
    {
        return _g_instance;
    }

    public GSMsgDispather()
    {
        GSMsgRegister_Init.regist(this);
        GSMsgRegister_Mail.regist(this);
        GSMsgRegister_Misc.regist(this);
        GSMsgRegister_Player.regist(this);
        GSMsgRegister_Dinner.regist(this);
        GSMsgRegister_Friend.regist(this);
        GSMsgRegister_Consort.regist(this);
        GSMsgRegister_Activity.regist(this);
    }

}