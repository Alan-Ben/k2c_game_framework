using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p034_InnOp
{

/// <summary>
/// 旅店设施升级
/// </summary>
public class GC2GS_034_003_ReqInnStationUpgrade : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 设施ID
/// </summary>
private long stationId;


public GC2GS_034_003_ReqInnStationUpgrade() {
	stationId = (long)0;
}

public GC2GS_034_003_ReqInnStationUpgrade(
	long _stationId
) {	stationId = _stationId;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 设施ID
/// </summary>
public long getStationId() { return stationId; }
/// <summary>
/// 设施ID
/// </summary>
public void setStationId(long _stationId) { stationId = _stationId; }


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
	stationId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(stationId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)3);
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
	builder.Append("stationId").Append(":").Append(stationId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

