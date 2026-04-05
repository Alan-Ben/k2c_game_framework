package Common.WeekCardObj;

import java.nio.ByteBuffer;
/*********
 * 周卡-额外设置-大学学习
 **/
public class WeekCard_ExtraInfo_CollegeStudy implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣列表 */
private java.util.ArrayList<Long> heroList;
/** 是否关闭 关闭则需要给玩家留下可以收获的 */
private boolean isClose;


public WeekCard_ExtraInfo_CollegeStudy() {
	heroList = new java.util.ArrayList<Long>();
	isClose = false;
}

public WeekCard_ExtraInfo_CollegeStudy(
	 java.util.ArrayList<Long> _heroList
	, boolean _isClose
) {	heroList = _heroList;
	isClose = _isClose;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 大臣列表 */
public java.util.ArrayList<Long> getHeroList() { return heroList; }
/** 大臣列表 */
public void addHeroList(long _heroList) { heroList.add(_heroList); }
/** 是否关闭 关闭则需要给玩家留下可以收获的 */
public boolean getIsClose() { return isClose; }
/** 是否关闭 关闭则需要给玩家留下可以收获的 */
public void setIsClose(boolean _isClose) { isClose = _isClose; }


public final int GetBufSize() {
	int _size = 1;
	_size += 2 + (heroList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
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
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isClose = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)heroList.size());
	for(int _i = 0; _i < heroList.size(); _i++) { 
		_buf.putLong(heroList.get(_i));
	}
	_buf.put(isClose?(byte)1:(byte)0);
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

