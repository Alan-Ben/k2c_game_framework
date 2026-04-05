using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

/// <summary>
/// 可以帮助的火星求助实例数量减少
/// </summary>
public class GS2GC_042_052_OnCanDealMarsHelpDel : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 减少可帮助的火星求助实例ID
/// </summary>
private long canDealId;


public GS2GC_042_052_OnCanDealMarsHelpDel() {
	canDealId = (long)0;
}

public GS2GC_042_052_OnCanDealMarsHelpDel(
	long _canDealId
) {	canDealId = _canDealId;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 减少可帮助的火星求助实例ID
/// </summary>
public long getCanDealId() { return canDealId; }
/// <summary>
/// 减少可帮助的火星求助实例ID
/// </summary>
public void setCanDealId(long _canDealId) { canDealId = _canDealId; }


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
	canDealId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(canDealId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)52);
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
	builder.Append("canDealId").Append(":").Append(canDealId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

