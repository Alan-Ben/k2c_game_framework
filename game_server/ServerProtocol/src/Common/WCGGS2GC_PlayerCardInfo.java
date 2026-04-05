package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_PlayerCardInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long cardId;
private boolean isUnLocked;
private int chipNum;
private int canUseNum;
private int dailyBroughtNum;
private int level;
private int upRemainSec;


public WCGGS2GC_PlayerCardInfo() {
	cardId = (long)0;
	isUnLocked = false;
	chipNum = 0;
	canUseNum = 0;
	dailyBroughtNum = 0;
	level = 0;
	upRemainSec = 0;
}

public WCGGS2GC_PlayerCardInfo(
	 long _cardId
	, boolean _isUnLocked
	, int _chipNum
	, int _canUseNum
	, int _dailyBroughtNum
	, int _level
	, int _upRemainSec
) {	cardId = _cardId;
	isUnLocked = _isUnLocked;
	chipNum = _chipNum;
	canUseNum = _canUseNum;
	dailyBroughtNum = _dailyBroughtNum;
	level = _level;
	upRemainSec = _upRemainSec;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCardId() { return cardId; }
public void setCardId(long _cardId) { cardId = _cardId; }
public boolean getIsUnLocked() { return isUnLocked; }
public void setIsUnLocked(boolean _isUnLocked) { isUnLocked = _isUnLocked; }
public int getChipNum() { return chipNum; }
public void setChipNum(int _chipNum) { chipNum = _chipNum; }
public int getCanUseNum() { return canUseNum; }
public void setCanUseNum(int _canUseNum) { canUseNum = _canUseNum; }
public int getDailyBroughtNum() { return dailyBroughtNum; }
public void setDailyBroughtNum(int _dailyBroughtNum) { dailyBroughtNum = _dailyBroughtNum; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public int getUpRemainSec() { return upRemainSec; }
public void setUpRemainSec(int _upRemainSec) { upRemainSec = _upRemainSec; }


public final int GetBufSize() {
	int _size = 29;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 31;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isUnLocked = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chipNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) canUseNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dailyBroughtNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) upRemainSec = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cardId);
	_buf.put(isUnLocked?(byte)1:(byte)0);
	_buf.putInt(chipNum);
	_buf.putInt(canUseNum);
	_buf.putInt(dailyBroughtNum);
	_buf.putInt(level);
	_buf.putInt(upRemainSec);
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

