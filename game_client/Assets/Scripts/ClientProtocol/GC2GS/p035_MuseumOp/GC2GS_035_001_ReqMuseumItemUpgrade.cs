using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p035_MuseumOp
{

/// <summary>
/// 博物馆物品升级
/// </summary>
public class GC2GS_035_001_ReqMuseumItemUpgrade : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 物品ID
/// </summary>
private long itemId;


public GC2GS_035_001_ReqMuseumItemUpgrade() {
	itemId = (long)0;
}

public GC2GS_035_001_ReqMuseumItemUpgrade(
	long _itemId
) {	itemId = _itemId;
}

public byte getMainOrder() { return (byte)35; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 物品ID
/// </summary>
public long getItemId() { return itemId; }
/// <summary>
/// 物品ID
/// </summary>
public void setItemId(long _itemId) { itemId = _itemId; }


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
	itemId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(itemId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)35);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)35);
	_recBuf.put((byte)1);
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
	builder.Append("itemId").Append(":").Append(itemId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

