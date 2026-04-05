package NPUSServer.NPUSUserMgr;

import NPCommon.DB.BM.BM;
import NPCommon.Enum.NPCommonEnum.EClientType;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.JsonUtil;
import NPUSServer.USLog;
import USDB.Bo.PlayerBO;
import com.google.gson.JsonObject;

public class NPUSUserDataSDKInfo
{
    private NPUSUserData _m_userdata;

    //登录ip
    public String clientIp = "";

    //语言
    public String language = "";
    //操作系统
    public EClientType deviceType = EClientType.PC;
    //客户端包名
    public String clientPackageName = "";
    //客户端版本
    public String version = "";
    //设备id
    public String adid = "";
    //国家(使用国际通用缩写英文比如：中国CN、加拿大CD)
    public String nation = "";
    //一级渠道：无渠道时，苹果默认ios，安卓默认android
    public String adfrom = "";
    //二级渠道名称：无渠道时，默认使用default
    public String adfrom2 = "";
    //广告afid 同SdkId
    public String afid = "";

    //是否是真正的客户端
    public boolean isRealClient = true;

    public NPUSUserDataSDKInfo(NPUSUserData _userdata, String _customData)
    {
        _m_userdata = _userdata;

        //客户端自定义数据
        setExternalInfo(true, "", _customData);
    }

    /**
     * 设置相关信息
     * @param _clientIp   连接ip
     * @param _customData 自定义数据
     */
    public void setExternalInfo(boolean _isInit, String _clientIp, String _customData)
    {
        try
        {
            clientIp = _clientIp;

            JsonObject _jsonObj = CommonFunc.string2JsonObject(_customData);
            if (null != _jsonObj)
            {
                language = JsonUtil.getString(_jsonObj, "language", "");

                //解析客户端类型
                String deviceTypeForString = JsonUtil.getString(_jsonObj, "deviceType", "PC").toUpperCase();
                try
                {
                    deviceType = EClientType.valueOf(deviceTypeForString);
                } catch (Exception e)
                {
                    USLog.error(_m_userdata.getUSServer(), "NPUSUserDataSDKInfo setExternalInfo EClientType.valueOf fail cid:{} deviceTypeForString:{}", _m_userdata.getCid(), deviceTypeForString);
                }

                clientPackageName = JsonUtil.getString(_jsonObj, "clientPackageName", "");

                version = JsonUtil.getString(_jsonObj, "version", "");
                adid = JsonUtil.getString(_jsonObj, "adid", "");
                nation = JsonUtil.getString(_jsonObj, "nation", "");
                adfrom = JsonUtil.getString(_jsonObj, "adfrom", "");
                adfrom2 = JsonUtil.getString(_jsonObj, "adfrom2", "");
                afid = JsonUtil.getString(_jsonObj, "afid", "");

                //更新玩家的语言数据
                _m_userdata.setLang(language);
            } else
            {
                isRealClient = false;
            }
        } catch (Exception e)
        {
            USLog.error(_m_userdata.getUSServer(), "NPUSUserDataSDKInfo parse json fail", e);
        }

        //仅登录时检查记录首次SDK信息
        if (!_isInit)
        {
            checkRecordFirstTimeSDKInfo();
        }
    }

    /**
     * 检查记录首次登录SDK信息
     */
    private void checkRecordFirstTimeSDKInfo()
    {
        PlayerBO bo = _m_userdata.getPlayerComponent().getBo();
        if (bo.getCreateTime() == 0)
        {
            BM bmObj = _m_userdata.getUSServer().getBM();

            bo.setCreateTime(bmObj, CommonFunc.getNowTimeSec());
            bo.setCreateDate(bmObj, CommonFunc.getNowTagYYYYMMDD());
            bo.setAdfrom(bmObj, adfrom);
            bo.setAdfrom2(bmObj, adfrom2);
            bo.setAdid(bmObj, adid);
            bo.setClientPackageName(bmObj, clientPackageName);
            bo.setClientVerion(bmObj, version);
            bo.setNation(bmObj, nation);
            bo.setArIp(bmObj, clientIp);
            bo.setSdkid(bmObj, adid);
            bo.saveAllMarked(bmObj);
        }
    }
}