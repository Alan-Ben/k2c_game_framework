package GC2GS.p016_ChapterOp;

import java.nio.ByteBuffer;
/*********
 * 处理派遣事件
 **/
public class GC2GS_016_006_ReqDealChapterDispatchEvent implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣列表 */
private java.util.ArrayList<Long> heroList;


public GC2GS_016_006_ReqDealChapterDispatchEvent() {
	heroList = new java.util.ArrayList<Long>();
}

public GC2GS_016_006_ReqDealChapterDispatchEvent(
	 java.util.ArrayList<Long> _heroList
) {	heroList = _heroList;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)6; }

/** 大臣列表 */
public java.util.ArrayList<Long> getHeroList() { return heroList; }
/** 大臣列表 */
public void addHeroList(long _heroList) { heroList.add(_heroList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (heroList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (heroList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _heroListCount = _buf.getShort();
	for(int _i = 0; _i < _heroListCount; _i++) { 
		long _heroList = (long)0;
		if(_buf.remaining() > 0) _heroList = _buf.getLong();
		heroList.add(_heroList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)heroList.size());
	for(int _i = 0; _i < heroList.size(); _i++) { 
		_buf.putLong(heroList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)6);
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

