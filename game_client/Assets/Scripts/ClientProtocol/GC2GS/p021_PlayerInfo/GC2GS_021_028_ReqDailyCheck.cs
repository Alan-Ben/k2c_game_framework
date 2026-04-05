using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p021_PlayerInfo
{

/// <summary>
/// 每日签到
/// </summary>
public class GC2GS_021_028_ReqDailyCheck : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 点心id
/// </summary>
private long dessertId;


public GC2GS_021_028_ReqDailyCheck() {
	dessertId = (long)0;
}

public GC2GS_021_028_ReqDailyCheck(
	long _dessertId
) {	dessertId = _dessertId;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)28; }

/// <summary>
/// 点心id
/// </summary>
public long getDessertId() { return dessertId; }
/// <summary>
/// 点心id
/// </summary>
public void setDessertId(long _dessertId) { dessertId = _dessertId; }


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
	dessertId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dessertId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)28);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)28);
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
	builder.Append("dessertId").Append(":").Append(dessertId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

