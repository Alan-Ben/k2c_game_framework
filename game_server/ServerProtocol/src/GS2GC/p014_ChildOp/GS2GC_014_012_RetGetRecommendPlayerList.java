package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 联姻请求-获取推荐玩家列表
 **/
public class GS2GC_014_012_RetGetRecommendPlayerList implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private java.util.ArrayList<Common.ChildObj.Adult_PoolBaseInfo> matchList;


public GS2GC_014_012_RetGetRecommendPlayerList() {
	matchList = new java.util.ArrayList<Common.ChildObj.Adult_PoolBaseInfo>();
}

public GS2GC_014_012_RetGetRecommendPlayerList(
	 java.util.ArrayList<Common.ChildObj.Adult_PoolBaseInfo> _matchList
) {	matchList = _matchList;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)12; }

/** 空 */
public java.util.ArrayList<Common.ChildObj.Adult_PoolBaseInfo> getMatchList() { return matchList; }
/** 空 */
public void addMatchList(Common.ChildObj.Adult_PoolBaseInfo _matchList) { matchList.add(_matchList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (matchList.size() * 36);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (matchList.size() * 36);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _matchListCount = _buf.getShort();
	for(int _i = 0; _i < _matchListCount; _i++) { 
		Common.ChildObj.Adult_PoolBaseInfo _matchList = new Common.ChildObj.Adult_PoolBaseInfo();
		if(_buf.remaining() <= 0) return;
	int __matchListCustLen = _buf.getInt();
	int __matchListCurPos = _buf.position();
	_matchList.ReadUnzipBuf(_buf, __matchListCurPos + __matchListCustLen);
	_buf.position(__matchListCurPos + __matchListCustLen);

		matchList.add(_matchList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)matchList.size());
	for(int _i = 0; _i < matchList.size(); _i++) { 
		_buf.putInt(matchList.get(_i).GetBufSize());
	matchList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)12);
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

