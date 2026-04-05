using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_023_RetPlayerBubble : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家拥有的气泡框信息队列
/// </summary>
private List<Common.NpPlayerInfoObj.PlayerInfo_Bubble> bubbleInfoList;


public GS2GC_002_023_RetPlayerBubble() {
	bubbleInfoList = new List<Common.NpPlayerInfoObj.PlayerInfo_Bubble>();
}

public GS2GC_002_023_RetPlayerBubble(
	List<Common.NpPlayerInfoObj.PlayerInfo_Bubble> _bubbleInfoList
) {	bubbleInfoList = _bubbleInfoList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)23; }

/// <summary>
/// 玩家拥有的气泡框信息队列
/// </summary>
public List<Common.NpPlayerInfoObj.PlayerInfo_Bubble> getBubbleInfoList() { return bubbleInfoList; }
/// <summary>
/// 玩家拥有的气泡框信息队列
/// </summary>
public void addBubbleInfoList(Common.NpPlayerInfoObj.PlayerInfo_Bubble _bubbleInfoList) { bubbleInfoList.Add(_bubbleInfoList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (bubbleInfoList.Count * 17);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (bubbleInfoList.Count * 17);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _bubbleInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _bubbleInfoListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_Bubble _bubbleInfoList = new Common.NpPlayerInfoObj.PlayerInfo_Bubble();
		int __bubbleInfoListCustLen = _buf.getInt();
	int __bubbleInfoListCurPos = _buf.getCurPos();
	_bubbleInfoList.ReadUnzipBuf(_buf, __bubbleInfoListCurPos + __bubbleInfoListCustLen);
	_buf.setPosition(__bubbleInfoListCurPos + __bubbleInfoListCustLen);

		bubbleInfoList.Add(_bubbleInfoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)bubbleInfoList.Count);
	for(int _i = 0; _i < bubbleInfoList.Count; _i++) { 
		_buf.putInt(bubbleInfoList[_i].GetBufSize());
	bubbleInfoList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)23);
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
	builder.Append("bubbleInfoList").Append(":").Append(bubbleInfoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

