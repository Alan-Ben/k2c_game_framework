using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CrossTeamObj
{

/// <summary>
/// 组队申请数据
/// </summary>
public class CrossTeam_ApplyInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 申请玩家CID
/// </summary>
private long applyCid;
/// <summary>
/// 请求时间（毫秒）
/// </summary>
private long applyMs;


public CrossTeam_ApplyInfo() {
	applyCid = (long)0;
	applyMs = (long)0;
}

public CrossTeam_ApplyInfo(
	long _applyCid
	, long _applyMs
) {	applyCid = _applyCid;
	applyMs = _applyMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 申请玩家CID
/// </summary>
public long getApplyCid() { return applyCid; }
/// <summary>
/// 申请玩家CID
/// </summary>
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/// <summary>
/// 请求时间（毫秒）
/// </summary>
public long getApplyMs() { return applyMs; }
/// <summary>
/// 请求时间（毫秒）
/// </summary>
public void setApplyMs(long _applyMs) { applyMs = _applyMs; }


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
	applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	applyMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(applyCid);
	_buf.putLong(applyMs);
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
	builder.Append("applyCid").Append(":").Append(applyCid.ToString()).Append(", ");
	builder.Append("applyMs").Append(":").Append(applyMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

