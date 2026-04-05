using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GachaObj
{

/// <summary>
/// 公屏抽卡记录信息
/// </summary>
public class Gacha_PublicRecordInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 卡池物品id
/// </summary>
private long itemId;
/// <summary>
/// 数据id
/// </summary>
private long dbId;
/// <summary>
/// 玩家名字
/// </summary>
private string playerName;


public Gacha_PublicRecordInfo() {
	itemId = (long)0;
	dbId = (long)0;
	playerName = "";
}

public Gacha_PublicRecordInfo(
	long _itemId
	, long _dbId
	, string _playerName
) {	itemId = _itemId;
	dbId = _dbId;
	playerName = _playerName;
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
/// 数据id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
/// <summary>
/// 玩家名字
/// </summary>
public string getPlayerName() { return playerName; }
/// <summary>
/// 玩家名字
/// </summary>
public void setPlayerName(string _playerName) { playerName = _playerName; }


public int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerName = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(itemId);
	_buf.putLong(dbId);
	_buf.putString(playerName);
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("playerName").Append(":").Append(playerName.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

