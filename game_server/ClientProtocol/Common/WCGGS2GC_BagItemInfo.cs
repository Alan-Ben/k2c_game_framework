using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_BagItemInfo : ALBasicProtocolPack._IALProtocolStructure {
private long itemId;
private long itemCount;
private int lastGetTimeS;


public WCGGS2GC_BagItemInfo() {
	itemId = (long)0;
	itemCount = (long)0;
	lastGetTimeS = 0;
}

public WCGGS2GC_BagItemInfo(
	long _itemId
	, long _itemCount
	, int _lastGetTimeS
) {	itemId = _itemId;
	itemCount = _itemCount;
	lastGetTimeS = _lastGetTimeS;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getItemId() { return itemId; }
public void setItemId(long _itemId) { itemId = _itemId; }
public long getItemCount() { return itemCount; }
public void setItemCount(long _itemCount) { itemCount = _itemCount; }
public int getLastGetTimeS() { return lastGetTimeS; }
public void setLastGetTimeS(int _lastGetTimeS) { lastGetTimeS = _lastGetTimeS; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastGetTimeS = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(itemId);
	_buf.putLong(itemCount);
	_buf.putInt(lastGetTimeS);
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
	builder.Append("itemId").Append(":").Append(itemId.ToString()).Append(", ");
	builder.Append("itemCount").Append(":").Append(itemCount.ToString()).Append(", ");
	builder.Append("lastGetTimeS").Append(":").Append(lastGetTimeS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

