using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.EarningsGoalObj
{

/// <summary>
/// 赚速奖励信息
/// </summary>
public class EarningsGoal_RewardInfo : ALBasicProtocolPack._IALProtocolStructure {
private long refId;
/// <summary>
/// 首达玩家cid
/// </summary>
private long firstReachCid;
/// <summary>
/// 达成时间戳
/// </summary>
private long timestamp;


public EarningsGoal_RewardInfo() {
	refId = (long)0;
	firstReachCid = (long)0;
	timestamp = (long)0;
}

public EarningsGoal_RewardInfo(
	long _refId
	, long _firstReachCid
	, long _timestamp
) {	refId = _refId;
	firstReachCid = _firstReachCid;
	timestamp = _timestamp;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 首达玩家cid
/// </summary>
public long getFirstReachCid() { return firstReachCid; }
/// <summary>
/// 首达玩家cid
/// </summary>
public void setFirstReachCid(long _firstReachCid) { firstReachCid = _firstReachCid; }
/// <summary>
/// 达成时间戳
/// </summary>
public long getTimestamp() { return timestamp; }
/// <summary>
/// 达成时间戳
/// </summary>
public void setTimestamp(long _timestamp) { timestamp = _timestamp; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	firstReachCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timestamp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(refId);
	_buf.putLong(firstReachCid);
	_buf.putLong(timestamp);
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
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("firstReachCid").Append(":").Append(firstReachCid.ToString()).Append(", ");
	builder.Append("timestamp").Append(":").Append(timestamp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

