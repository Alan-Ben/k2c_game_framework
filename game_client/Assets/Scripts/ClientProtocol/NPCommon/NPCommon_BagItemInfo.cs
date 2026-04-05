using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_BagItemInfo : ALBasicProtocolPack._IALProtocolStructure {
private long itemId;
private long itemCount;
private int lastGetTimeS;
/// <summary>
/// 首次获得本物品的时间戳
/// </summary>
private int newItemTimeS;
/// <summary>
/// 最后一次点击本物品的时间戳
/// </summary>
private int clickItemTimeS;


public NPCommon_BagItemInfo() {
	itemId = (long)0;
	itemCount = (long)0;
	lastGetTimeS = 0;
	newItemTimeS = 0;
	clickItemTimeS = 0;
}

public NPCommon_BagItemInfo(
	long _itemId
	, long _itemCount
	, int _lastGetTimeS
	, int _newItemTimeS
	, int _clickItemTimeS
) {	itemId = _itemId;
	itemCount = _itemCount;
	lastGetTimeS = _lastGetTimeS;
	newItemTimeS = _newItemTimeS;
	clickItemTimeS = _clickItemTimeS;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getItemId() { return itemId; }
public void setItemId(long _itemId) { itemId = _itemId; }
public long getItemCount() { return itemCount; }
public void setItemCount(long _itemCount) { itemCount = _itemCount; }
public int getLastGetTimeS() { return lastGetTimeS; }
public void setLastGetTimeS(int _lastGetTimeS) { lastGetTimeS = _lastGetTimeS; }
/// <summary>
/// 首次获得本物品的时间戳
/// </summary>
public int getNewItemTimeS() { return newItemTimeS; }
/// <summary>
/// 首次获得本物品的时间戳
/// </summary>
public void setNewItemTimeS(int _newItemTimeS) { newItemTimeS = _newItemTimeS; }
/// <summary>
/// 最后一次点击本物品的时间戳
/// </summary>
public int getClickItemTimeS() { return clickItemTimeS; }
/// <summary>
/// 最后一次点击本物品的时间戳
/// </summary>
public void setClickItemTimeS(int _clickItemTimeS) { clickItemTimeS = _clickItemTimeS; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastGetTimeS = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	newItemTimeS = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clickItemTimeS = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(itemId);
	_buf.putLong(itemCount);
	_buf.putInt(lastGetTimeS);
	_buf.putInt(newItemTimeS);
	_buf.putInt(clickItemTimeS);
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
	builder.Append("newItemTimeS").Append(":").Append(newItemTimeS.ToString()).Append(", ");
	builder.Append("clickItemTimeS").Append(":").Append(clickItemTimeS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

