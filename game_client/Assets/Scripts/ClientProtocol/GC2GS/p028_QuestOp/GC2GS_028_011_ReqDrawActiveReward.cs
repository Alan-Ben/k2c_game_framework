using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p028_QuestOp
{

/// <summary>
/// 领取活跃度奖励
/// </summary>
public class GC2GS_028_011_ReqDrawActiveReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务刷新序列号
/// </summary>
private long refreshSerial;
/// <summary>
/// 活跃奖励id
/// </summary>
private long activeRewardId;


public GC2GS_028_011_ReqDrawActiveReward() {
	refreshSerial = (long)0;
	activeRewardId = (long)0;
}

public GC2GS_028_011_ReqDrawActiveReward(
	long _refreshSerial
	, long _activeRewardId
) {	refreshSerial = _refreshSerial;
	activeRewardId = _activeRewardId;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)11; }

/// <summary>
/// 任务刷新序列号
/// </summary>
public long getRefreshSerial() { return refreshSerial; }
/// <summary>
/// 任务刷新序列号
/// </summary>
public void setRefreshSerial(long _refreshSerial) { refreshSerial = _refreshSerial; }
/// <summary>
/// 活跃奖励id
/// </summary>
public long getActiveRewardId() { return activeRewardId; }
/// <summary>
/// 活跃奖励id
/// </summary>
public void setActiveRewardId(long _activeRewardId) { activeRewardId = _activeRewardId; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refreshSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activeRewardId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(refreshSerial);
	_buf.putLong(activeRewardId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)11);
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
	builder.Append("refreshSerial").Append(":").Append(refreshSerial.ToString()).Append(", ");
	builder.Append("activeRewardId").Append(":").Append(activeRewardId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

