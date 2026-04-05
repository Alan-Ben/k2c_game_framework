using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p006_BagItemOp
{

public class GC2GS_006_007_ReqConvert : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 原道具id
/// </summary>
private long bagItemId;
/// <summary>
/// 想要转换获得的目标道具数量
/// </summary>
private long bagItemCount;


public GC2GS_006_007_ReqConvert() {
	bagItemId = (long)0;
	bagItemCount = (long)0;
}

public GC2GS_006_007_ReqConvert(
	long _bagItemId
	, long _bagItemCount
) {	bagItemId = _bagItemId;
	bagItemCount = _bagItemCount;
}

public byte getMainOrder() { return (byte)6; }

public byte getSubOrder() { return (byte)7; }

/// <summary>
/// 原道具id
/// </summary>
public long getBagItemId() { return bagItemId; }
/// <summary>
/// 原道具id
/// </summary>
public void setBagItemId(long _bagItemId) { bagItemId = _bagItemId; }
/// <summary>
/// 想要转换获得的目标道具数量
/// </summary>
public long getBagItemCount() { return bagItemCount; }
/// <summary>
/// 想要转换获得的目标道具数量
/// </summary>
public void setBagItemCount(long _bagItemCount) { bagItemCount = _bagItemCount; }


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
	bagItemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	bagItemCount = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(bagItemId);
	_buf.putLong(bagItemCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
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
	builder.Append("bagItemId").Append(":").Append(bagItemId.ToString()).Append(", ");
	builder.Append("bagItemCount").Append(":").Append(bagItemCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

