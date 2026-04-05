using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.StageGoalObj
{

/// <summary>
/// 阶段目标大阶段首达数据
/// </summary>
public class StageGoal_BigStepFirstReachInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大阶段ID
/// </summary>
private long bigStepId;
/// <summary>
/// 首达角色ID
/// </summary>
private long cid;
/// <summary>
/// 首达时间，时间戳
/// </summary>
private long reachTimeMs;


public StageGoal_BigStepFirstReachInfo() {
	bigStepId = (long)0;
	cid = (long)0;
	reachTimeMs = (long)0;
}

public StageGoal_BigStepFirstReachInfo(
	long _bigStepId
	, long _cid
	, long _reachTimeMs
) {	bigStepId = _bigStepId;
	cid = _cid;
	reachTimeMs = _reachTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 大阶段ID
/// </summary>
public long getBigStepId() { return bigStepId; }
/// <summary>
/// 大阶段ID
/// </summary>
public void setBigStepId(long _bigStepId) { bigStepId = _bigStepId; }
/// <summary>
/// 首达角色ID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 首达角色ID
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 首达时间，时间戳
/// </summary>
public long getReachTimeMs() { return reachTimeMs; }
/// <summary>
/// 首达时间，时间戳
/// </summary>
public void setReachTimeMs(long _reachTimeMs) { reachTimeMs = _reachTimeMs; }


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
	bigStepId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	reachTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(bigStepId);
	_buf.putLong(cid);
	_buf.putLong(reachTimeMs);
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
	builder.Append("bigStepId").Append(":").Append(bigStepId.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("reachTimeMs").Append(":").Append(reachTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

