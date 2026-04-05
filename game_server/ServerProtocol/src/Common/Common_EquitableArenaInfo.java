package Common;

import java.nio.ByteBuffer;
public class Common_EquitableArenaInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int arenatype;
private int freeTimes;
private boolean isChallenge;
private int victory;
private int failed;
private boolean hasGotBalanceBox;
private int maxVictory;
private int remainSec;


public Common_EquitableArenaInfo() {
	arenatype = 0;
	freeTimes = 0;
	isChallenge = false;
	victory = 0;
	failed = 0;
	hasGotBalanceBox = false;
	maxVictory = 0;
	remainSec = 0;
}

public Common_EquitableArenaInfo(
	 int _arenatype
	, int _freeTimes
	, boolean _isChallenge
	, int _victory
	, int _failed
	, boolean _hasGotBalanceBox
	, int _maxVictory
	, int _remainSec
) {	arenatype = _arenatype;
	freeTimes = _freeTimes;
	isChallenge = _isChallenge;
	victory = _victory;
	failed = _failed;
	hasGotBalanceBox = _hasGotBalanceBox;
	maxVictory = _maxVictory;
	remainSec = _remainSec;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getArenatype() { return arenatype; }
public void setArenatype(int _arenatype) { arenatype = _arenatype; }
public int getFreeTimes() { return freeTimes; }
public void setFreeTimes(int _freeTimes) { freeTimes = _freeTimes; }
public boolean getIsChallenge() { return isChallenge; }
public void setIsChallenge(boolean _isChallenge) { isChallenge = _isChallenge; }
public int getVictory() { return victory; }
public void setVictory(int _victory) { victory = _victory; }
public int getFailed() { return failed; }
public void setFailed(int _failed) { failed = _failed; }
public boolean getHasGotBalanceBox() { return hasGotBalanceBox; }
public void setHasGotBalanceBox(boolean _hasGotBalanceBox) { hasGotBalanceBox = _hasGotBalanceBox; }
public int getMaxVictory() { return maxVictory; }
public void setMaxVictory(int _maxVictory) { maxVictory = _maxVictory; }
public int getRemainSec() { return remainSec; }
public void setRemainSec(int _remainSec) { remainSec = _remainSec; }


public final int GetBufSize() {
	int _size = 26;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 28;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) arenatype = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) freeTimes = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isChallenge = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) victory = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) failed = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasGotBalanceBox = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxVictory = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) remainSec = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(arenatype);
	_buf.putInt(freeTimes);
	_buf.put(isChallenge?(byte)1:(byte)0);
	_buf.putInt(victory);
	_buf.putInt(failed);
	_buf.put(hasGotBalanceBox?(byte)1:(byte)0);
	_buf.putInt(maxVictory);
	_buf.putInt(remainSec);
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

