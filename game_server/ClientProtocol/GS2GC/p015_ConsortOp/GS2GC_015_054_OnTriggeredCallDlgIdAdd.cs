using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 已触发的邀约剧情ID推送
/// </summary>
public class GS2GC_015_054_OnTriggeredCallDlgIdAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private long consortId;
/// <summary>
/// 空
/// </summary>
private long triggeredCallStroryId;


public GS2GC_015_054_OnTriggeredCallDlgIdAdd() {
	consortId = (long)0;
	triggeredCallStroryId = (long)0;
}

public GS2GC_015_054_OnTriggeredCallDlgIdAdd(
	long _consortId
	, long _triggeredCallStroryId
) {	consortId = _consortId;
	triggeredCallStroryId = _triggeredCallStroryId;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)54; }

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
public long getTriggeredCallStroryId() { return triggeredCallStroryId; }
/// <summary>
/// 空
/// </summary>
public void setTriggeredCallStroryId(long _triggeredCallStroryId) { triggeredCallStroryId = _triggeredCallStroryId; }


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
	triggeredCallStroryId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(triggeredCallStroryId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)54);
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
	builder.Append("triggeredCallStroryId").Append(":").Append(triggeredCallStroryId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

