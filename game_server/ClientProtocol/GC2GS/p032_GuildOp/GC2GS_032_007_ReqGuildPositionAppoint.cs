using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 请求联盟职位任命
/// </summary>
public class GC2GS_032_007_ReqGuildPositionAppoint : ALBasicProtocolPack._IALProtocolStructure {
private long memberId;
/// <summary>
/// 职位id
/// </summary>
private long positionId;


public GC2GS_032_007_ReqGuildPositionAppoint() {
	memberId = (long)0;
	positionId = (long)0;
}

public GC2GS_032_007_ReqGuildPositionAppoint(
	long _memberId
	, long _positionId
) {	memberId = _memberId;
	positionId = _positionId;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)7; }

public long getMemberId() { return memberId; }
public void setMemberId(long _memberId) { memberId = _memberId; }
/// <summary>
/// 职位id
/// </summary>
public long getPositionId() { return positionId; }
/// <summary>
/// 职位id
/// </summary>
public void setPositionId(long _positionId) { positionId = _positionId; }


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
	memberId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	positionId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(memberId);
	_buf.putLong(positionId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)7);
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
	builder.Append("memberId").Append(":").Append(memberId.ToString()).Append(", ");
	builder.Append("positionId").Append(":").Append(positionId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

