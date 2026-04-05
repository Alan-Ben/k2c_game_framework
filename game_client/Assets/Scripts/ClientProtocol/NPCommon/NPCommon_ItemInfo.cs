using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_ItemInfo : ALBasicProtocolPack._IALProtocolStructure {
private int itemType;
private long subId;
private long count;
private byte[] extData;


public NPCommon_ItemInfo() {
	itemType = 0;
	subId = (long)0;
	count = (long)0;
	extData = null;
}

public NPCommon_ItemInfo(
	int _itemType
	, long _subId
	, long _count
	, byte[] _extData
) {	itemType = _itemType;
	subId = _subId;
	count = _count;
	extData = _extData;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getItemType() { return itemType; }
public void setItemType(int _itemType) { itemType = _itemType; }
public long getSubId() { return subId; }
public void setSubId(long _subId) { subId = _subId; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }
public byte[] getExtData() { return extData; }

public void setExtData(byte[] _extData) { extData = _extData; }



public int GetBufSize() {
	int _size = 20;
	_size += 4 + (extData == null ? 0 : extData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += 4 + (extData == null ? 0 : extData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	subId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(itemType);
	_buf.putLong(subId);
	_buf.putLong(count);
	_buf.putByteBuffer(extData);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("itemType").Append(":").Append(itemType.ToString()).Append(", ");
	builder.Append("subId").Append(":").Append(subId.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("extData").Append(":").Append(extData == null ? "null" : extData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

