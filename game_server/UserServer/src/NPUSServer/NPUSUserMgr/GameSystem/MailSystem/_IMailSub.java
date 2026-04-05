package NPUSServer.NPUSUserMgr.GameSystem.MailSystem;

import java.nio.ByteBuffer;

/******
 * 邮件内子功能模块的统一接口
 */
public interface _IMailSub
{
    int getSubId();

    ByteBuffer toBytes();
}
