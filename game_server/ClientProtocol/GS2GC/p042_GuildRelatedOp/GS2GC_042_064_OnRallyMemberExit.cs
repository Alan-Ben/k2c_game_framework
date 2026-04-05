using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

/// <summary>
/// 成员退出集结推送
/// </summary>
public class GS2GC_042_064_OnRallyMemberExit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 集结ID
/// </summary>
private long rallyId;
/// <summary>
/// 成员CID
/// </summary>
private long memberCid;


public GS2GC_042_064_OnRallyMemberExit() {
	rallyId = (long)0;
	memberCid = (long)0;
}

public GS2GC_042_064_OnRallyMemberExit(
	long _rallyId
	, long _memberCid
) {	rallyId = _rallyId;
	memberCid = _memberCid;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)64; }

/// <summary>
/// 集结ID
/// </summary>
public long getRallyId() { return rallyId; }
/// <summary>
/// 集结ID
/// </summary>
public void setRallyId(long _rallyId) { rallyId = _rallyId; }
/// <summary>
/// 成员CID
/// </summary>
public long getMemberCid() { return memberCid; }
/// <summary>
/// 成员CID
/// </summary>
public void setMemberCid(long _memberCid) { memberCid = _memberCid; }


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
	rallyId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	memberCid = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(rallyId);
	_buf.putLong(memberCid);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)64);
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
	builder.Append("rallyId").Append(":").Append(rallyId.ToString()).Append(", ");
	builder.Append("memberCid").Append(":").Append(memberCid.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

