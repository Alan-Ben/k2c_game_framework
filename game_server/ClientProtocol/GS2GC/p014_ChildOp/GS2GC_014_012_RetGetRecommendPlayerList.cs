using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 联姻请求-获取推荐玩家列表
/// </summary>
public class GS2GC_014_012_RetGetRecommendPlayerList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private List<Common.ChildObj.Adult_PoolBaseInfo> matchList;


public GS2GC_014_012_RetGetRecommendPlayerList() {
	matchList = new List<Common.ChildObj.Adult_PoolBaseInfo>();
}

public GS2GC_014_012_RetGetRecommendPlayerList(
	List<Common.ChildObj.Adult_PoolBaseInfo> _matchList
) {	matchList = _matchList;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)12; }

/// <summary>
/// 空
/// </summary>
public List<Common.ChildObj.Adult_PoolBaseInfo> getMatchList() { return matchList; }
/// <summary>
/// 空
/// </summary>
public void addMatchList(Common.ChildObj.Adult_PoolBaseInfo _matchList) { matchList.Add(_matchList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (matchList.Count * 36);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (matchList.Count * 36);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _matchListCount = _buf.getShort();
	for(int _i = 0; _i < _matchListCount; _i++) { 
		Common.ChildObj.Adult_PoolBaseInfo _matchList = new Common.ChildObj.Adult_PoolBaseInfo();
		int __matchListCustLen = _buf.getInt();
	int __matchListCurPos = _buf.getCurPos();
	_matchList.ReadUnzipBuf(_buf, __matchListCurPos + __matchListCustLen);
	_buf.setPosition(__matchListCurPos + __matchListCustLen);

		matchList.Add(_matchList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)matchList.Count);
	for(int _i = 0; _i < matchList.Count; _i++) { 
		_buf.putInt(matchList[_i].GetBufSize());
	matchList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)12);
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
	builder.Append("matchList").Append(":").Append(matchList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

