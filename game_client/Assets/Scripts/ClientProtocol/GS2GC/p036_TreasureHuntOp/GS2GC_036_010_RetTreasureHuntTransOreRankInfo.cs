using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

public class GS2GC_036_010_RetTreasureHuntTransOreRankInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿石排行榜前三
/// </summary>
private List<Common.TreasureHuntObj.TreasureHunt_OreRankItem> topThreeList;
/// <summary>
/// 当前排名
/// </summary>
private int rank;


public GS2GC_036_010_RetTreasureHuntTransOreRankInfo() {
	topThreeList = new List<Common.TreasureHuntObj.TreasureHunt_OreRankItem>();
	rank = 0;
}

public GS2GC_036_010_RetTreasureHuntTransOreRankInfo(
	List<Common.TreasureHuntObj.TreasureHunt_OreRankItem> _topThreeList
	, int _rank
) {	topThreeList = _topThreeList;
	rank = _rank;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)10; }

/// <summary>
/// 矿石排行榜前三
/// </summary>
public List<Common.TreasureHuntObj.TreasureHunt_OreRankItem> getTopThreeList() { return topThreeList; }
/// <summary>
/// 矿石排行榜前三
/// </summary>
public void addTopThreeList(Common.TreasureHuntObj.TreasureHunt_OreRankItem _topThreeList) { topThreeList.Add(_topThreeList); }
/// <summary>
/// 当前排名
/// </summary>
public int getRank() { return rank; }
/// <summary>
/// 当前排名
/// </summary>
public void setRank(int _rank) { rank = _rank; }


public int GetBufSize() {
	int _size = 4;
	_size += 2 + (topThreeList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (topThreeList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _topThreeListCount = _buf.getShort();
	for(int _i = 0; _i < _topThreeListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_OreRankItem _topThreeList = new Common.TreasureHuntObj.TreasureHunt_OreRankItem();
		int __topThreeListCustLen = _buf.getInt();
	int __topThreeListCurPos = _buf.getCurPos();
	_topThreeList.ReadUnzipBuf(_buf, __topThreeListCurPos + __topThreeListCustLen);
	_buf.setPosition(__topThreeListCurPos + __topThreeListCustLen);

		topThreeList.Add(_topThreeList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rank = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)topThreeList.Count);
	for(int _i = 0; _i < topThreeList.Count; _i++) { 
		_buf.putInt(topThreeList[_i].GetBufSize());
	topThreeList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(rank);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)10);
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
	builder.Append("topThreeList").Append(":").Append(topThreeList.ToString()).Append(", ");
	builder.Append("rank").Append(":").Append(rank.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

