using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DailyCheckObj
{

/// <summary>
/// 每日签到奖励信息
/// </summary>
public class DailyCheck_RewardInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 累计签到天数
/// </summary>
private int totalCheckDays;
/// <summary>
/// 已经领取奖励的天数
/// </summary>
private int rewardedDays;
/// <summary>
/// 展示数据列表
/// </summary>
private List<Common.DailyCheckObj.DailyCheck_RewardShowInfo> showInfoList;
/// <summary>
/// 数据列表的前一条数据，即上一组最后一条展示数据
/// </summary>
private Common.DailyCheckObj.DailyCheck_RewardShowInfo preShowInfo;
/// <summary>
/// 数据列表的后一条数据，即下一组第一条展示数据
/// </summary>
private Common.DailyCheckObj.DailyCheck_RewardShowInfo nextShowInfo;


public DailyCheck_RewardInfo() {
	totalCheckDays = 0;
	rewardedDays = 0;
	showInfoList = new List<Common.DailyCheckObj.DailyCheck_RewardShowInfo>();
	preShowInfo = new Common.DailyCheckObj.DailyCheck_RewardShowInfo();
	nextShowInfo = new Common.DailyCheckObj.DailyCheck_RewardShowInfo();
}

public DailyCheck_RewardInfo(
	int _totalCheckDays
	, int _rewardedDays
	, List<Common.DailyCheckObj.DailyCheck_RewardShowInfo> _showInfoList
	, Common.DailyCheckObj.DailyCheck_RewardShowInfo _preShowInfo
	, Common.DailyCheckObj.DailyCheck_RewardShowInfo _nextShowInfo
) {	totalCheckDays = _totalCheckDays;
	rewardedDays = _rewardedDays;
	showInfoList = _showInfoList;
	preShowInfo = _preShowInfo;
	nextShowInfo = _nextShowInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 累计签到天数
/// </summary>
public int getTotalCheckDays() { return totalCheckDays; }
/// <summary>
/// 累计签到天数
/// </summary>
public void setTotalCheckDays(int _totalCheckDays) { totalCheckDays = _totalCheckDays; }
/// <summary>
/// 已经领取奖励的天数
/// </summary>
public int getRewardedDays() { return rewardedDays; }
/// <summary>
/// 已经领取奖励的天数
/// </summary>
public void setRewardedDays(int _rewardedDays) { rewardedDays = _rewardedDays; }
/// <summary>
/// 展示数据列表
/// </summary>
public List<Common.DailyCheckObj.DailyCheck_RewardShowInfo> getShowInfoList() { return showInfoList; }
/// <summary>
/// 展示数据列表
/// </summary>
public void addShowInfoList(Common.DailyCheckObj.DailyCheck_RewardShowInfo _showInfoList) { showInfoList.Add(_showInfoList); }
/// <summary>
/// 数据列表的前一条数据，即上一组最后一条展示数据
/// </summary>
public Common.DailyCheckObj.DailyCheck_RewardShowInfo getPreShowInfo() { return preShowInfo; }
/// <summary>
/// 数据列表的前一条数据，即上一组最后一条展示数据
/// </summary>
public void setPreShowInfo(Common.DailyCheckObj.DailyCheck_RewardShowInfo _preShowInfo) { preShowInfo = _preShowInfo; }
/// <summary>
/// 数据列表的后一条数据，即下一组第一条展示数据
/// </summary>
public Common.DailyCheckObj.DailyCheck_RewardShowInfo getNextShowInfo() { return nextShowInfo; }
/// <summary>
/// 数据列表的后一条数据，即下一组第一条展示数据
/// </summary>
public void setNextShowInfo(Common.DailyCheckObj.DailyCheck_RewardShowInfo _nextShowInfo) { nextShowInfo = _nextShowInfo; }


public int GetBufSize() {
	int _size = 40;
	_size += 2 + (showInfoList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 42;
	_size += 2 + (showInfoList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalCheckDays = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rewardedDays = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _showInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _showInfoListCount; _i++) { 
		Common.DailyCheckObj.DailyCheck_RewardShowInfo _showInfoList = new Common.DailyCheckObj.DailyCheck_RewardShowInfo();
		int __showInfoListCustLen = _buf.getInt();
	int __showInfoListCurPos = _buf.getCurPos();
	_showInfoList.ReadUnzipBuf(_buf, __showInfoListCurPos + __showInfoListCustLen);
	_buf.setPosition(__showInfoListCurPos + __showInfoListCustLen);

		showInfoList.Add(_showInfoList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _preShowInfoCustLen = _buf.getInt();
	int _preShowInfoCurPos = _buf.getCurPos();
	preShowInfo.ReadUnzipBuf(_buf, _preShowInfoCurPos + _preShowInfoCustLen);
	_buf.setPosition(_preShowInfoCurPos + _preShowInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _nextShowInfoCustLen = _buf.getInt();
	int _nextShowInfoCurPos = _buf.getCurPos();
	nextShowInfo.ReadUnzipBuf(_buf, _nextShowInfoCurPos + _nextShowInfoCustLen);
	_buf.setPosition(_nextShowInfoCurPos + _nextShowInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(totalCheckDays);
	_buf.putInt(rewardedDays);
	_buf.putShort((short)showInfoList.Count);
	for(int _i = 0; _i < showInfoList.Count; _i++) { 
		_buf.putInt(showInfoList[_i].GetBufSize());
	showInfoList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(preShowInfo.GetBufSize());
	preShowInfo.PutUnzipBuf(_buf);
	_buf.putInt(nextShowInfo.GetBufSize());
	nextShowInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("totalCheckDays").Append(":").Append(totalCheckDays.ToString()).Append(", ");
	builder.Append("rewardedDays").Append(":").Append(rewardedDays.ToString()).Append(", ");
	builder.Append("showInfoList").Append(":").Append(showInfoList.ToString()).Append(", ");
	builder.Append("preShowInfo").Append(":").Append(preShowInfo == null ? "null" : preShowInfo.ToString()).Append(", ");
	builder.Append("nextShowInfo").Append(":").Append(nextShowInfo == null ? "null" : nextShowInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

