package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_PlayerJoinGuild;
import Common.OfflineRewardObj.Offline_PlayerQuitGuild;
import Common.ServerObj.ServerObj_GuildBoxSettleList;
import Common.ServerObj.ServerObj_PayCallbackInfo;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerOfflineRewardBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;

public class OfflineRewardFunc
{
    /**
     * 添加玩家离线奖励，兼容在线与离线状态
     * @param _userServer
     * @param _cid
     * @param _offlineRewardType
     * @param _offlineData
     * @param _itemList
     * @param _rewardShow
     * @param _context
     */
    public static void addPlayerOfflineReward(NPUserServer _userServer, long _cid, EOfflineRewardEnum _offlineRewardType,
                                              _IALProtocolStructure _offlineData, _IALProtocolStructure _itemList, _IALProtocolStructure _rewardShow, NPPlayerContext _context)
    {
        _addPlayerOfflineReward(
                _userServer,
                _cid,
                _offlineRewardType,
                _toPackage(_offlineData),
                _toPackage(_itemList),
                _toPackage(_rewardShow),
                _context);
    }
    public static void addPlayerOfflineReward(NPUserServer _userServer, long _cid, EOfflineRewardEnum _offlineRewardType,
                                              _IALProtocolStructure _offlineData, NPPlayerContext _context)
    {
        addPlayerOfflineReward(_userServer, _cid, _offlineRewardType, _offlineData, null, null, _context);
    }

    /**
     * 添加玩家离线奖励，入参为ByteBuffer格式离线数据
     * @param _userServer
     * @param _cid
     * @param _offlineRewardType
     * @param _offlineData
     * @param _itemList
     * @param _rewardShow
     * @param _context
     */
    public static void addPlayerOfflineReward(NPUserServer _userServer, long _cid, EOfflineRewardEnum _offlineRewardType,
                                              ByteBuffer _offlineData, ByteBuffer _itemList, ByteBuffer _rewardShow, NPPlayerContext _context)
    {
        _addPlayerOfflineReward(_userServer, _cid, _offlineRewardType, _offlineData, _itemList, _rewardShow, _context);
    }

    private static void _addPlayerOfflineReward(NPUserServer _userServer, long _cid, EOfflineRewardEnum _offlineRewardType,
                                                ByteBuffer _offlineData, ByteBuffer _itemList, ByteBuffer _rewardShow, NPPlayerContext _context)
    {
        _userServer.getLoaderMgr().safeCall(() ->
        {
            NPUSUserData userData = _userServer.getUsUserMgr().lookupCacheUserData(_cid);
            if (null == userData)
            {
                _dealOfflineData(_userServer, _cid, _offlineRewardType, _offlineData, _itemList, _rewardShow);
                return;
            }

            userData.safeCall(() ->
            {
                userData.getOfflineRewardComponent().addReward(
                        _offlineRewardType,
                        _offlineData,
                        _itemList,
                        _rewardShow,
                        _context);
            });
        });
    }

    public static void addPlayerOfflineReward(NPUserServer _userServer, long _cid, EOfflineRewardEnum _offlineRewardType,
                                              ByteBuffer _offlineData, NPPlayerContext _context)
    {
        addPlayerOfflineReward(_userServer, _cid, _offlineRewardType, _offlineData, null, null, _context);
    }

    /**
     * 玩家加入公会处理
     * @param _userServer
     * @param _cid
     */
    public static void onPlayerJoinGuild(NPUserServer _userServer, long _cid, long _guildId, String _guildName, String _newSimpleName, ArrayList<Integer> _addPerArr)
    {
        Offline_PlayerJoinGuild offlineData = new Offline_PlayerJoinGuild(_guildId, _guildName, _newSimpleName, _addPerArr);
        addPlayerOfflineReward(_userServer, _cid, EOfflineRewardEnum.JOIN_GUILD, offlineData, null);
    }

    /**
     * 玩家退出公会处理
     * @param _userServer
     * @param _cid
     * @param _timestamp
     */
    public static void onPlayerQuitGuild(NPUserServer _userServer, long _cid, long _oriGuildId, String _guildName, long _timestamp, boolean _isKick)
    {
        Offline_PlayerQuitGuild offlineData = new Offline_PlayerQuitGuild(_oriGuildId, _guildName, _timestamp, _isKick);
        addPlayerOfflineReward(_userServer, _cid, EOfflineRewardEnum.QUIT_GUILD, offlineData, null);
    }

    /**
     * 玩家订单支付处理
     * @param _userServer
     * @param _payCallbackInfo
     */
    public static void onPlayerOrderPay(NPUserServer _userServer, ServerObj_PayCallbackInfo _payCallbackInfo, NPPlayerContext _context)
    {
        addPlayerOfflineReward(_userServer, _payCallbackInfo.getCid(), EOfflineRewardEnum.ORDER_PAY, _payCallbackInfo, _context);
    }

    /**
     * 玩家结算联盟宝箱
     * @param _userServer
     * @param _cid
     * @param _offlineData
     * @param _context
     */
    public static void onPlayerSettleGuildBox(NPUserServer _userServer, long _cid, ServerObj_GuildBoxSettleList _offlineData, NPPlayerContext _context)
    {
        addPlayerOfflineReward(_userServer, _cid, EOfflineRewardEnum.GUILD_BOX_DISPATCH, _offlineData, _context);
    }

    /**
     * 处理离线数据
     * @param _userServer
     * @param _cid
     * @param _offlineRewardType
     * @param _offlineData
     */
    private static void _dealOfflineData(NPUserServer _userServer, long _cid, EOfflineRewardEnum _offlineRewardType,
                                         ByteBuffer _offlineData, ByteBuffer _itemList, ByteBuffer _rewardShow)
    {
        PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
        bo.setCid(_userServer.getBM(), _cid);
        bo.setRewardType(_userServer.getBM(), _offlineRewardType.ordinal());
        if (null != _offlineData)
            bo.setOfflineData(_userServer.getBM(), CommonFunc.ByteBfferToBytes(_offlineData));
        if (null != _itemList)
            bo.setItemList(_userServer.getBM(), CommonFunc.ByteBfferToBytes(_itemList));
        if (null != _rewardShow)
            bo.setRewardShow(_userServer.getBM(), CommonFunc.ByteBfferToBytes(_rewardShow));
        bo.insert(_userServer.getBM());
    }

    private static ByteBuffer _toPackage(_IALProtocolStructure _data)
    {
        if (null == _data)
            return null;

        return _data.makePackage();
    }

}


