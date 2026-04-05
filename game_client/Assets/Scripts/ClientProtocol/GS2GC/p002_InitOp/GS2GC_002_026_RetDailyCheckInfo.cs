using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_026_RetDailyCheckInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 每日签到信息
/// </summary>
private Common.DailyCheckObj.DailyCheck_Info info;
/// <summary>
/// 每日签到奖励信息
/// </summary>
private Common.DailyCheckObj.DailyCheck_RewardInfo rewardInfo;


public GS2GC_002_026_RetDailyCheckInfo() {
	info = new Common.DailyCheckObj.DailyCheck_Info();
	rewardInfo = new Common.DailyCheckObj.DailyCheck_RewardInfo();
}

public GS2GC_002_026_RetDailyCheckInfo(
	Common.DailyCheckObj.DailyCheck_Info _info
	, Common.DailyCheckObj.DailyCheck_RewardInfo _rewardInfo
) {	info = _info;
	rewardInfo = _rewardInfo;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)26; }

/// <summary>
/// 每日签到信息
/// </summary>
public Common.DailyCheckObj.DailyCheck_Info getInfo() { return info; }
/// <summary>
/// 每日签到信息
/// </summary>
public void setInfo(Common.DailyCheckObj.DailyCheck_Info _info) { info = _info; }
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
	_size += 4 + info.GetBufSize();
	_size += 4 + rewardInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + info.GetBufSize();
	_size += 4 + rewardInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.getCurPos();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.setPosition(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _rewardInfoCustLen = _buf.getInt();
	int _rewardInfoCurPos = _buf.getCurPos();
	rewardInfo.ReadUnzipBuf(_buf, _rewardInfoCurPos + _rewardInfoCustLen);
	_buf.setPosition(_rewardInfoCurPos + _rewardInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putInt(rewardInfo.GetBufSize());
	rewardInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)26);
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
	builder.Append("info").Append(":").Append(info == null ? "null" : info.ToString()).Append(", ");
	builder.Append("rewardInfo").Append(":").Append(rewardInfo == null ? "null" : rewardInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

