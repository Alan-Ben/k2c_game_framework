using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p023_ArenaOp
{

public class GS2GC_023_007_RetCelebrityRank : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 名人榜列表
/// </summary>
private List<Common.ArenaObj.Arena_CelebrityRankInfo> rankList;


public GS2GC_023_007_RetCelebrityRank() {
	rankList = new List<Common.ArenaObj.Arena_CelebrityRankInfo>();
}

public GS2GC_023_007_RetCelebrityRank(
	List<Common.ArenaObj.Arena_CelebrityRankInfo> _rankList
) {	rankList = _rankList;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)7; }

/// <summary>
/// 名人榜列表
/// </summary>
public List<Common.ArenaObj.Arena_CelebrityRankInfo> getRankList() { return rankList; }
/// <summary>
/// 名人榜列表
/// </summary>
public void addRankList(Common.ArenaObj.Arena_CelebrityRankInfo _rankList) { rankList.Add(_rankList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < rankList.Count; _i++) {
	_size += 4 + rankList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < rankList.Count; _i++) {
	_size += 4 + rankList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rankListCount = _buf.getShort();
	for(int _i = 0; _i < _rankListCount; _i++) { 
		Common.ArenaObj.Arena_CelebrityRankInfo _rankList = new Common.ArenaObj.Arena_CelebrityRankInfo();
		int __rankListCustLen = _buf.getInt();
	int __rankListCurPos = _buf.getCurPos();
	_rankList.ReadUnzipBuf(_buf, __rankListCurPos + __rankListCustLen);
	_buf.setPosition(__rankListCurPos + __rankListCustLen);

		rankList.Add(_rankList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)rankList.Count);
	for(int _i = 0; _i < rankList.Count; _i++) { 
		_buf.putInt(rankList[_i].GetBufSize());
	rankList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
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
	builder.Append("rankList").Append(":").Append(rankList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

