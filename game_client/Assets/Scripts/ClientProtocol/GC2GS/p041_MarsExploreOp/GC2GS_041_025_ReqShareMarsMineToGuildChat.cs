using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p041_MarsExploreOp
{

/// <summary>
/// 火星探险-分享矿到联盟聊天
/// </summary>
public class GC2GS_041_025_ReqShareMarsMineToGuildChat : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿实例ID
/// </summary>
private long mineInstanceId;


public GC2GS_041_025_ReqShareMarsMineToGuildChat() {
	mineInstanceId = (long)0;
}

public GC2GS_041_025_ReqShareMarsMineToGuildChat(
	long _mineInstanceId
) {	mineInstanceId = _mineInstanceId;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)25; }

/// <summary>
/// 矿实例ID
/// </summary>
public long getMineInstanceId() { return mineInstanceId; }
/// <summary>
/// 矿实例ID
/// </summary>
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }


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
	mineInstanceId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(mineInstanceId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)25);
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
	builder.Append("mineInstanceId").Append(":").Append(mineInstanceId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

