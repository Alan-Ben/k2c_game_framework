using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p031_RankOp
{

/// <summary>
/// 请求常驻排行榜一键点赞
/// </summary>
public class GC2GS_031_004_ReqRankFixedAKeyLike : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 常驻排行榜id列表
/// </summary>
private List<long> rankFixedIdList;
/// <summary>
/// 是否跨服
/// </summary>
private bool isCross;


public GC2GS_031_004_ReqRankFixedAKeyLike() {
	rankFixedIdList = new List<long>();
	isCross = false;
}

public GC2GS_031_004_ReqRankFixedAKeyLike(
	List<long> _rankFixedIdList
	, bool _isCross
) {	rankFixedIdList = _rankFixedIdList;
	isCross = _isCross;
}

public byte getMainOrder() { return (byte)31; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 常驻排行榜id列表
/// </summary>
public List<long> getRankFixedIdList() { return rankFixedIdList; }
/// <summary>
/// 常驻排行榜id列表
/// </summary>
public void addRankFixedIdList(long _rankFixedIdList) { rankFixedIdList.Add(_rankFixedIdList); }
/// <summary>
/// 是否跨服
/// </summary>
public bool getIsCross() { return isCross; }
/// <summary>
/// 是否跨服
/// </summary>
public void setIsCross(bool _isCross) { isCross = _isCross; }


public int GetBufSize() {
	int _size = 1;
	_size += 2 + (rankFixedIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;
	_size += 2 + (rankFixedIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rankFixedIdListCount = _buf.getShort();
	for(int _i = 0; _i < _rankFixedIdListCount; _i++) { 
		long _rankFixedIdList = (long)0;
		_rankFixedIdList = _buf.getLong();
		rankFixedIdList.Add(_rankFixedIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isCross = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)rankFixedIdList.Count);
	for(int _i = 0; _i < rankFixedIdList.Count; _i++) { 
		_buf.putLong(rankFixedIdList[_i]);
	}
	_buf.put(isCross?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)31);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)31);
	_recBuf.put((byte)4);
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
	builder.Append("rankFixedIdList").Append(":").Append(rankFixedIdList.ToString()).Append(", ");
	builder.Append("isCross").Append(":").Append(isCross.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

