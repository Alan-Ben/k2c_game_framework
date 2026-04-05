using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-矿石排行榜项
/// </summary>
public class TreasureHunt_OreRankItem : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 排名
/// </summary>
private int rank;
/// <summary>
/// cid
/// </summary>
private long cid;
/// <summary>
/// 重量
/// </summary>
private int weight;


public TreasureHunt_OreRankItem() {
	rank = 0;
	cid = (long)0;
	weight = 0;
}

public TreasureHunt_OreRankItem(
	int _rank
	, long _cid
	, int _weight
) {	rank = _rank;
	cid = _cid;
	weight = _weight;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 排名
/// </summary>
public int getRank() { return rank; }
/// <summary>
/// 排名
/// </summary>
public void setRank(int _rank) { rank = _rank; }
/// <summary>
/// cid
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// cid
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 重量
/// </summary>
public int getWeight() { return weight; }
/// <summary>
/// 重量
/// </summary>
public void setWeight(int _weight) { weight = _weight; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rank = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	weight = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(rank);
	_buf.putLong(cid);
	_buf.putInt(weight);
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
	builder.Append("rank").Append(":").Append(rank.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("weight").Append(":").Append(weight.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

