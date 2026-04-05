using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动基金-移除推送
/// </summary>
public class GS2GC_017_068_OnActivityFundRemove : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基金ID
/// </summary>
private long fundId;


public GS2GC_017_068_OnActivityFundRemove() {
	fundId = (long)0;
}

public GS2GC_017_068_OnActivityFundRemove(
	long _fundId
) {	fundId = _fundId;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)68; }

/// <summary>
/// 基金ID
/// </summary>
public long getFundId() { return fundId; }
/// <summary>
/// 基金ID
/// </summary>
public void setFundId(long _fundId) { fundId = _fundId; }


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
	fundId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(fundId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)68);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)68);
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
	builder.Append("fundId").Append(":").Append(fundId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

