using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p021_PlayerInfo
{

public class GC2GS_021_026_ReqGainAchievePointReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 成就点奖励配置ID
/// </summary>
private long achievePointRewardId;


public GC2GS_021_026_ReqGainAchievePointReward() {
	achievePointRewardId = (long)0;
}

public GC2GS_021_026_ReqGainAchievePointReward(
	long _achievePointRewardId
) {	achievePointRewardId = _achievePointRewardId;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)26; }

/// <summary>
/// 成就点奖励配置ID
/// </summary>
public long getAchievePointRewardId() { return achievePointRewardId; }
/// <summary>
/// 成就点奖励配置ID
/// </summary>
public void setAchievePointRewardId(long _achievePointRewardId) { achievePointRewardId = _achievePointRewardId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	achievePointRewardId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(achievePointRewardId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
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
	builder.Append("achievePointRewardId").Append(":").Append(achievePointRewardId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

