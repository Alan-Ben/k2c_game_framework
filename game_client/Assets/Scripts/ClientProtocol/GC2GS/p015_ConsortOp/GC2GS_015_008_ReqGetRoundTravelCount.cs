using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p015_ConsortOp
{

/// <summary>
/// 家人-获取周期出游次数
/// </summary>
public class GC2GS_015_008_ReqGetRoundTravelCount : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人邀约类型ID
/// </summary>
private long consortTravelId;


public GC2GS_015_008_ReqGetRoundTravelCount() {
	consortTravelId = (long)0;
}

public GC2GS_015_008_ReqGetRoundTravelCount(
	long _consortTravelId
) {	consortTravelId = _consortTravelId;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)8; }

/// <summary>
/// 家人邀约类型ID
/// </summary>
public long getConsortTravelId() { return consortTravelId; }
/// <summary>
/// 家人邀约类型ID
/// </summary>
public void setConsortTravelId(long _consortTravelId) { consortTravelId = _consortTravelId; }


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
	consortTravelId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortTravelId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)8);
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
	builder.Append("consortTravelId").Append(":").Append(consortTravelId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

