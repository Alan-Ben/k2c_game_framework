using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MuseumObj
{

/// <summary>
/// 博物馆_物品信息
/// </summary>
public class Museum_ItemInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 物品ID
/// </summary>
private long itemId;
/// <summary>
/// 是否激活
/// </summary>
private bool isActive;
/// <summary>
/// 等级
/// </summary>
private int level;
/// <summary>
/// 获得时间(毫秒)
/// </summary>
private long gainTimeMs;


public Museum_ItemInfo() {
	itemId = (long)0;
	isActive = false;
	level = 0;
	gainTimeMs = (long)0;
}

public Museum_ItemInfo(
	long _itemId
	, bool _isActive
	, int _level
	, long _gainTimeMs
) {	itemId = _itemId;
	isActive = _isActive;
	level = _level;
	gainTimeMs = _gainTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 物品ID
/// </summary>
public long getItemId() { return itemId; }
/// <summary>
/// 物品ID
/// </summary>
public void setItemId(long _itemId) { itemId = _itemId; }
/// <summary>
/// 是否激活
/// </summary>
public bool getIsActive() { return isActive; }
/// <summary>
/// 是否激活
/// </summary>
public void setIsActive(bool _isActive) { isActive = _isActive; }
/// <summary>
/// 等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 等级
/// </summary>
public void setLevel(int _level) { level = _level; }
/// <summary>
/// 获得时间(毫秒)
/// </summary>
public long getGainTimeMs() { return gainTimeMs; }
/// <summary>
/// 获得时间(毫秒)
/// </summary>
public void setGainTimeMs(long _gainTimeMs) { gainTimeMs = _gainTimeMs; }


public int GetBufSize() {
	int _size = 21;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isActive = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(itemId);
	_buf.put(isActive?(byte)1:(byte)0);
	_buf.putInt(level);
	_buf.putLong(gainTimeMs);
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
	builder.Append("isActive").Append(":").Append(isActive.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("gainTimeMs").Append(":").Append(gainTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

