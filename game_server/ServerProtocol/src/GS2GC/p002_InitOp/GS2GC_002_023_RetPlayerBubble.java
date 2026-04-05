package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_023_RetPlayerBubble implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家拥有的气泡框信息队列 */
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Bubble> bubbleInfoList;


public GS2GC_002_023_RetPlayerBubble() {
	bubbleInfoList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Bubble>();
}

public GS2GC_002_023_RetPlayerBubble(
	 java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Bubble> _bubbleInfoList
) {	bubbleInfoList = _bubbleInfoList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)23; }

/** 玩家拥有的气泡框信息队列 */
public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_Bubble> getBubbleInfoList() { return bubbleInfoList; }
/** 玩家拥有的气泡框信息队列 */
public void addBubbleInfoList(Common.NpPlayerInfoObj.PlayerInfo_Bubble _bubbleInfoList) { bubbleInfoList.add(_bubbleInfoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (bubbleInfoList.size() * 17);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (bubbleInfoList.size() * 17);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _bubbleInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _bubbleInfoListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_Bubble _bubbleInfoList = new Common.NpPlayerInfoObj.PlayerInfo_Bubble();
		if(_buf.remaining() <= 0) return;
	int __bubbleInfoListCustLen = _buf.getInt();
	int __bubbleInfoListCurPos = _buf.position();
	_bubbleInfoList.ReadUnzipBuf(_buf, __bubbleInfoListCurPos + __bubbleInfoListCustLen);
	_buf.position(__bubbleInfoListCurPos + __bubbleInfoListCustLen);

		bubbleInfoList.add(_bubbleInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)bubbleInfoList.size());
	for(int _i = 0; _i < bubbleInfoList.size(); _i++) { 
		_buf.putInt(bubbleInfoList.get(_i).GetBufSize());
	bubbleInfoList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)23);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

