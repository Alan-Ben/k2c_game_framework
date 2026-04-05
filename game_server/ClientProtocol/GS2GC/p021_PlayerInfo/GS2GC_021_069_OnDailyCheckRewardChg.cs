using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 每日签到奖励信息变更
/// </summary>
public class GS2GC_021_069_OnDailyCheckRewardChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 每日签到奖励信息
/// </summary>
private Common.DailyCheckObj.DailyCheck_RewardInfo rewardInfo;


public GS2GC_021_069_OnDailyCheckRewardChg() {
	rewardInfo = new Common.DailyCheckObj.DailyCheck_RewardInfo();
}

public GS2GC_021_069_OnDailyCheckRewardChg(
	Common.DailyCheckObj.DailyCheck_RewardInfo _rewardInfo
) {	rewardInfo = _rewardInfo;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)69; }

/// <summary>
/// 每日签到奖励信息
/// </summary>
public Common.DailyCheckObj.DailyCheck_RewardInfo getRewardInfo() { return rewardInfo; }
/// <summary>
/// 每日签到奖励信息
/// </summary>
public void setRewardInfo(Common.DailyCheckObj.DailyCheck_RewardInfo _rewardInfo) { rewardInfo = _rewardInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + rewardInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + rewardInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _rewardInfoCustLen = _buf.getInt();
	int _rewardInfoCurPos = _buf.getCurPos();
	rewardInfo.ReadUnzipBuf(_buf, _rewardInfoCurPos + _rewardInfoCustLen);
	_buf.setPosition(_rewardInfoCurPos + _rewardInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(rewardInfo.GetBufSize());
	rewardInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)69);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)69);
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
	builder.Append("rewardInfo").Append(":").Append(rewardInfo == null ? "null" : rewardInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

