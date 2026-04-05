package Common;

import java.nio.ByteBuffer;
public class Common_TodayLikeCidInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 上次刷新日期 */
private int lastFreshDate;
/** 点赞过玩家CID列表 */
private java.util.ArrayList<Long> likeCidList;


public Common_TodayLikeCidInfo() {
	lastFreshDate = 0;
	likeCidList = new java.util.ArrayList<Long>();
}

public Common_TodayLikeCidInfo(
	 int _lastFreshDate
	, java.util.ArrayList<Long> _likeCidList
) {	lastFreshDate = _lastFreshDate;
	likeCidList = _likeCidList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 上次刷新日期 */
public int getLastFreshDate() { return lastFreshDate; }
/** 上次刷新日期 */
public void setLastFreshDate(int _lastFreshDate) { lastFreshDate = _lastFreshDate; }
/** 点赞过玩家CID列表 */
public java.util.ArrayList<Long> getLikeCidList() { return likeCidList; }
/** 点赞过玩家CID列表 */
public void addLikeCidList(long _likeCidList) { likeCidList.add(_likeCidList); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (likeCidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (likeCidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastFreshDate = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _likeCidListCount = _buf.getShort();
	for(int _i = 0; _i < _likeCidListCount; _i++) { 
		long _likeCidList = (long)0;
		if(_buf.remaining() > 0) _likeCidList = _buf.getLong();
		likeCidList.add(_likeCidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(lastFreshDate);
	_buf.putShort((short)likeCidList.size());
	for(int _i = 0; _i < likeCidList.size(); _i++) { 
		_buf.putLong(likeCidList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

