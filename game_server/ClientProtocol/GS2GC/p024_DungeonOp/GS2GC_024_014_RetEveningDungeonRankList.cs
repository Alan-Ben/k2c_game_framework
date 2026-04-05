using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p024_DungeonOp
{

public class GS2GC_024_014_RetEveningDungeonRankList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基础数据列表
/// </summary>
private List<Common.RankObj.Rank_BaseItem> rankList;
/// <summary>
/// 玩家排行数据
/// </summary>
private Common.RankObj.Rank_BaseItem selfRankItem;


public GS2GC_024_014_RetEveningDungeonRankList() {
	rankList = new List<Common.RankObj.Rank_BaseItem>();
	selfRankItem = new Common.RankObj.Rank_BaseItem();
}

public GS2GC_024_014_RetEveningDungeonRankList(
	List<Common.RankObj.Rank_BaseItem> _rankList
	, Common.RankObj.Rank_BaseItem _selfRankItem
) {	rankList = _rankList;
	selfRankItem = _selfRankItem;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)14; }

/// <summary>
/// 基础数据列表
/// </summary>
public List<Common.RankObj.Rank_BaseItem> getRankList() { return rankList; }
/// <summary>
/// 基础数据列表
/// </summary>
public void addRankList(Common.RankObj.Rank_BaseItem _rankList) { rankList.Add(_rankList); }
/// <summary>
/// 玩家排行数据
/// </summary>
public Common.RankObj.Rank_BaseItem getSelfRankItem() { return selfRankItem; }
/// <summary>
/// 玩家排行数据
/// </summary>
public void setSelfRankItem(Common.RankObj.Rank_BaseItem _selfRankItem) { selfRankItem = _selfRankItem; }


public int GetBufSize() {
	int _size = 32;
	_size += 2 + (rankList.Count * 32);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;
	_size += 2 + (rankList.Count * 32);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rankListCount = _buf.getShort();
	for(int _i = 0; _i < _rankListCount; _i++) { 
		Common.RankObj.Rank_BaseItem _rankList = new Common.RankObj.Rank_BaseItem();
		int __rankListCustLen = _buf.getInt();
	int __rankListCurPos = _buf.getCurPos();
	_rankList.ReadUnzipBuf(_buf, __rankListCurPos + __rankListCustLen);
	_buf.setPosition(__rankListCurPos + __rankListCustLen);

		rankList.Add(_rankList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _selfRankItemCustLen = _buf.getInt();
	int _selfRankItemCurPos = _buf.getCurPos();
	selfRankItem.ReadUnzipBuf(_buf, _selfRankItemCurPos + _selfRankItemCustLen);
	_buf.setPosition(_selfRankItemCurPos + _selfRankItemCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)rankList.Count);
	for(int _i = 0; _i < rankList.Count; _i++) { 
		_buf.putInt(rankList[_i].GetBufSize());
	rankList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(selfRankItem.GetBufSize());
	selfRankItem.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)14);
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
	builder.Append("rankList").Append(":").Append(rankList.ToString()).Append(", ");
	builder.Append("selfRankItem").Append(":").Append(selfRankItem == null ? "null" : selfRankItem.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

