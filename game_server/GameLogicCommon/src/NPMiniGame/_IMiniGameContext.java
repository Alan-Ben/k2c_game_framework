package NPMiniGame;

import ALBasicProtocolPack._IALProtocolStructure;

/**
 * 游戏上下文
 */
public interface _IMiniGameContext
{
    /**
     * 构造游戏结果协议数据
     * @return _IALProtocolStructure
     */
    _IALProtocolStructure makeRetProto();

    /**
     * 游戏结果状态
     * @return int
     */
    int getRetState();

    /**
     * 获取游戏序列号
     * @return long
     */
    long getGameSerial();

    /**
     * 获取游戏配置ID
     * @return long
     */
    long getGameRefId();

    /**
     * 获取游戏分数
     * @return
     */
    long getGameScore();

    /**
     * 小游戏结束后的处理
     * @param _ret 小游戏处理是否成功
     */
    void doOver(boolean _ret);
}
