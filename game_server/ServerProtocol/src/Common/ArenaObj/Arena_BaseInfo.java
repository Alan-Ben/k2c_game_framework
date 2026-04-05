package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场基础数据
 **/
public class Arena_BaseInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 上次重置时间戳 毫秒 */
private long lastResetTimeMs;
/** 已指定攻击次数 */
private int hadSelectAttackNum;
/** 已随机攻击次数 */
private int hadRandomAttackNum;
/** 已购买随机攻击次数 */
private int hadBuyRandomAttackNum;
/** 已指定攻击大臣id列表 */
private java.util.ArrayList<Long> hadSelectAttackHeroList;
/** 已随机攻击大臣id列表 */
private java.util.ArrayList<Long> hadRandomAttackHeroList;


public Arena_BaseInfo() {
	lastResetTimeMs = (long)0;
	hadSelectAttackNum = 0;
	hadRandomAttackNum = 0;
	hadBuyRandomAttackNum = 0;
	hadSelectAttackHeroList = new java.util.ArrayList<Long>();
	hadRandomAttackHeroList = new java.util.ArrayList<Long>();
}

public Arena_BaseInfo(
	 long _lastResetTimeMs
	, int _hadSelectAttackNum
	, int _hadRandomAttackNum
	, int _hadBuyRandomAttackNum
	, java.util.ArrayList<Long> _hadSelectAttackHeroList
	, java.util.ArrayList<Long> _hadRandomAttackHeroList
) {	lastResetTimeMs = _lastResetTimeMs;
	hadSelectAttackNum = _hadSelectAttackNum;
	hadRandomAttackNum = _hadRandomAttackNum;
	hadBuyRandomAttackNum = _hadBuyRandomAttackNum;
	hadSelectAttackHeroList = _hadSelectAttackHeroList;
	hadRandomAttackHeroList = _hadRandomAttackHeroList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 上次重置时间戳 毫秒 */
public long getLastResetTimeMs() { return lastResetTimeMs; }
/** 上次重置时间戳 毫秒 */
public void setLastResetTimeMs(long _lastResetTimeMs) { lastResetTimeMs = _lastResetTimeMs; }
/** 已指定攻击次数 */
public int getHadSelectAttackNum() { return hadSelectAttackNum; }
/** 已指定攻击次数 */
public void setHadSelectAttackNum(int _hadSelectAttackNum) { hadSelectAttackNum = _hadSelectAttackNum; }
/** 已随机攻击次数 */
public int getHadRandomAttackNum() { return hadRandomAttackNum; }
/** 已随机攻击次数 */
public void setHadRandomAttackNum(int _hadRandomAttackNum) { hadRandomAttackNum = _hadRandomAttackNum; }
/** 已购买随机攻击次数 */
public int getHadBuyRandomAttackNum() { return hadBuyRandomAttackNum; }
/** 已购买随机攻击次数 */
public void setHadBuyRandomAttackNum(int _hadBuyRandomAttackNum) { hadBuyRandomAttackNum = _hadBuyRandomAttackNum; }
/** 已指定攻击大臣id列表 */
public java.util.ArrayList<Long> getHadSelectAttackHeroList() { return hadSelectAttackHeroList; }
/** 已指定攻击大臣id列表 */
public void addHadSelectAttackHeroList(long _hadSelectAttackHeroList) { hadSelectAttackHeroList.add(_hadSelectAttackHeroList); }
/** 已随机攻击大臣id列表 */
public java.util.ArrayList<Long> getHadRandomAttackHeroList() { return hadRandomAttackHeroList; }
/** 已随机攻击大臣id列表 */
public void addHadRandomAttackHeroList(long _hadRandomAttackHeroList) { hadRandomAttackHeroList.add(_hadRandomAttackHeroList); }


public final int GetBufSize() {
	int _size = 20;
	_size += 2 + (hadSelectAttackHeroList.size() * 8);
	_size += 2 + (hadRandomAttackHeroList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (hadSelectAttackHeroList.size() * 8);
	_size += 2 + (hadRandomAttackHeroList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastResetTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadSelectAttackNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadRandomAttackNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadBuyRandomAttackNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadSelectAttackHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _hadSelectAttackHeroListCount; _i++) { 
		long _hadSelectAttackHeroList = (long)0;
		if(_buf.remaining() > 0) _hadSelectAttackHeroList = _buf.getLong();
		hadSelectAttackHeroList.add(_hadSelectAttackHeroList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadRandomAttackHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _hadRandomAttackHeroListCount; _i++) { 
		long _hadRandomAttackHeroList = (long)0;
		if(_buf.remaining() > 0) _hadRandomAttackHeroList = _buf.getLong();
		hadRandomAttackHeroList.add(_hadRandomAttackHeroList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(lastResetTimeMs);
	_buf.putInt(hadSelectAttackNum);
	_buf.putInt(hadRandomAttackNum);
	_buf.putInt(hadBuyRandomAttackNum);
	_buf.putShort((short)hadSelectAttackHeroList.size());
	for(int _i = 0; _i < hadSelectAttackHeroList.size(); _i++) { 
		_buf.putLong(hadSelectAttackHeroList.get(_i));
	}
	_buf.putShort((short)hadRandomAttackHeroList.size());
	for(int _i = 0; _i < hadRandomAttackHeroList.size(); _i++) { 
		_buf.putLong(hadRandomAttackHeroList.get(_i));
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

