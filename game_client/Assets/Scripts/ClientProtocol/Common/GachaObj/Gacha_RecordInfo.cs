using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GachaObj
{

/// <summary>
/// 抽卡记录信息
/// </summary>
public class Gacha_RecordInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 卡池物品id
/// </summary>
private long itemId;
/// <summary>
/// 抽卡时间戳 秒
/// </summary>
private int rollTimeSec;
/// <summary>
/// 数据id
/// </summary>
private long dbId;


public Gacha_RecordInfo() {
	itemId = (long)0;
	rollTimeSec = 0;
	dbId = (long)0;
}

public Gacha_RecordInfo(
	long _itemId
	, int _rollTimeSec
	, long _dbId
) {	itemId = _itemId;
	rollTimeSec = _rollTimeSec;
	dbId = _dbId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 卡池物品id
/// </summary>
public long getItemId() { return itemId; }
/// <summary>
/// 卡池物品id
/// </summary>
public void setItemId(long _itemId) { itemId = _itemId; }
/// <summary>
/// 抽卡时间戳 秒
/// </summary>
public int getRollTimeSec() { return rollTimeSec; }
/// <summary>
/// 抽卡时间戳 秒
/// </summary>
public void setRollTimeSec(int _rollTimeSec) { rollTimeSec = _rollTimeSec; }
/// <summary>
/// 数据id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }


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
	rollTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(itemId);
	_buf.putInt(rollTimeSec);
	_buf.putLong(dbId);
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
	builder.Append("rollTimeSec").Append(":").Append(rollTimeSec.ToString()).Append(", ");
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

