using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人加护点推送
/// </summary>
public class GS2GC_015_053_OnConsortCharmPointChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private long consortId;
/// <summary>
/// 空
/// </summary>
private long charmPoint;


public GS2GC_015_053_OnConsortCharmPointChg() {
	consortId = (long)0;
	charmPoint = (long)0;
}

public GS2GC_015_053_OnConsortCharmPointChg(
	long _consortId
	, long _charmPoint
) {	consortId = _consortId;
	charmPoint = _charmPoint;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 空
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 空
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 空
/// </summary>
public long getCharmPoint() { return charmPoint; }
/// <summary>
/// 空
/// </summary>
public void setCharmPoint(long _charmPoint) { charmPoint = _charmPoint; }


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
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	charmPoint = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(charmPoint);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)53);
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
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("charmPoint").Append(":").Append(charmPoint.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

