using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟宝箱奖励数据
/// </summary>
public class Guild_BoxReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宝箱实例ID
/// </summary>
private long id;
/// <summary>
/// 物品
/// </summary>
private NPCommon.NPCommon_ItemInfo item;


public Guild_BoxReward() {
	id = (long)0;
	item = new NPCommon.NPCommon_ItemInfo();
}

public Guild_BoxReward(
	long _id
	, NPCommon.NPCommon_ItemInfo _item
) {	id = _id;
	item = _item;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 宝箱实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 宝箱实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 物品
/// </summary>
public NPCommon.NPCommon_ItemInfo getItem() { return item; }
/// <summary>
/// 物品
/// </summary>
public void setItem(NPCommon.NPCommon_ItemInfo _item) { item = _item; }


public int GetBufSize() {
	int _size = 8;
	_size += 4 + item.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + item.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _itemCustLen = _buf.getInt();
	int _itemCurPos = _buf.getCurPos();
	item.ReadUnzipBuf(_buf, _itemCurPos + _itemCustLen);
	_buf.setPosition(_itemCurPos + _itemCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt(item.GetBufSize());
	item.PutUnzipBuf(_buf);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("item").Append(":").Append(item == null ? "null" : item.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

