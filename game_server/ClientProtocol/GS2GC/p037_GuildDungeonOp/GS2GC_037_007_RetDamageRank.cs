using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p037_GuildDungeonOp
{

/// <summary>
/// 公会副本伤害排行榜
/// </summary>
public class GS2GC_037_007_RetDamageRank : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 排行数据
/// </summary>
private List<Common.GuildDungeonObj.GuildDungeon_DamageRankItem> itemList;
private int myRank;
private long myValue;


public GS2GC_037_007_RetDamageRank() {
	itemList = new List<Common.GuildDungeonObj.GuildDungeon_DamageRankItem>();
	myRank = 0;
	myValue = (long)0;
}

public GS2GC_037_007_RetDamageRank(
	List<Common.GuildDungeonObj.GuildDungeon_DamageRankItem> _itemList
	, int _myRank
	, long _myValue
) {	itemList = _itemList;
	myRank = _myRank;
	myValue = _myValue;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)7; }

/// <summary>
/// 排行数据
/// </summary>
public List<Common.GuildDungeonObj.GuildDungeon_DamageRankItem> getItemList() { return itemList; }
/// <summary>
/// 排行数据
/// </summary>
public void addItemList(Common.GuildDungeonObj.GuildDungeon_DamageRankItem _itemList) { itemList.Add(_itemList); }
public int getMyRank() { return myRank; }
public void setMyRank(int _myRank) { myRank = _myRank; }
public long getMyValue() { return myValue; }
public void setMyValue(long _myValue) { myValue = _myValue; }


public int GetBufSize() {
	int _size = 12;
	_size += 2 + (itemList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (itemList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_DamageRankItem _itemList = new Common.GuildDungeonObj.GuildDungeon_DamageRankItem();
		int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.getCurPos();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.setPosition(__itemListCurPos + __itemListCustLen);

		itemList.Add(_itemList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	myRank = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	myValue = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(myRank);
	_buf.putLong(myValue);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
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
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("myRank").Append(":").Append(myRank.ToString()).Append(", ");
	builder.Append("myValue").Append(":").Append(myValue.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

