package GC2GS.p016_ChapterOp;

import java.nio.ByteBuffer;
/*********
 * 领取关卡剧情奖励
 **/
public class GC2GS_016_007_ReqDrawChapterPlotReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 剧情ID列表 */
private java.util.ArrayList<Long> plotIdList;


public GC2GS_016_007_ReqDrawChapterPlotReward() {
	plotIdList = new java.util.ArrayList<Long>();
}

public GC2GS_016_007_ReqDrawChapterPlotReward(
	 java.util.ArrayList<Long> _plotIdList
) {	plotIdList = _plotIdList;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)7; }

/** 剧情ID列表 */
public java.util.ArrayList<Long> getPlotIdList() { return plotIdList; }
/** 剧情ID列表 */
public void addPlotIdList(long _plotIdList) { plotIdList.add(_plotIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (plotIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (plotIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _plotIdListCount = _buf.getShort();
	for(int _i = 0; _i < _plotIdListCount; _i++) { 
		long _plotIdList = (long)0;
		if(_buf.remaining() > 0) _plotIdList = _buf.getLong();
		plotIdList.add(_plotIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)plotIdList.size());
	for(int _i = 0; _i < plotIdList.size(); _i++) { 
		_buf.putLong(plotIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)7);
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

