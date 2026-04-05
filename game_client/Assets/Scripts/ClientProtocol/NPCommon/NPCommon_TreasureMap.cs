using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 藏宝图数据
/// </summary>
public class NPCommon_TreasureMap : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 唯一id
/// </summary>
private long dbId;
/// <summary>
/// 藏宝图配置id
/// </summary>
private long refId;
/// <summary>
/// 藏宝图空间位置
/// </summary>
private long spaceId;
/// <summary>
/// 藏宝图位置x
/// </summary>
private int posX;
/// <summary>
/// 藏宝图位置y
/// </summary>
private int posY;


public NPCommon_TreasureMap() {
	dbId = (long)0;
	refId = (long)0;
	spaceId = (long)0;
	posX = 0;
	posY = 0;
}

public NPCommon_TreasureMap(
	long _dbId
	, long _refId
	, long _spaceId
	, int _posX
	, int _posY
) {	dbId = _dbId;
	refId = _refId;
	spaceId = _spaceId;
	posX = _posX;
	posY = _posY;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 唯一id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 唯一id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
/// <summary>
/// 藏宝图配置id
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// 藏宝图配置id
/// </summary>
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 藏宝图空间位置
/// </summary>
public long getSpaceId() { return spaceId; }
/// <summary>
/// 藏宝图空间位置
/// </summary>
public void setSpaceId(long _spaceId) { spaceId = _spaceId; }
/// <summary>
/// 藏宝图位置x
/// </summary>
public int getPosX() { return posX; }
/// <summary>
/// 藏宝图位置x
/// </summary>
public void setPosX(int _posX) { posX = _posX; }
/// <summary>
/// 藏宝图位置y
/// </summary>
public int getPosY() { return posY; }
/// <summary>
/// 藏宝图位置y
/// </summary>
public void setPosY(int _posY) { posY = _posY; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	spaceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	posX = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	posY = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(refId);
	_buf.putLong(spaceId);
	_buf.putInt(posX);
	_buf.putInt(posY);
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("spaceId").Append(":").Append(spaceId.ToString()).Append(", ");
	builder.Append("posX").Append(":").Append(posX.ToString()).Append(", ");
	builder.Append("posY").Append(":").Append(posY.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

