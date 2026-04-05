package Common;

import java.nio.ByteBuffer;
public class Common_UserLogInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private String name;
private long gem;
private long cardCoin;
private long silver;
private long recharge;
private int level;
private int lastLoginTime;
private int lastLoginDate;
private boolean isBanded;


public Common_UserLogInfo() {
	cid = (long)0;
	name = "";
	gem = (long)0;
	cardCoin = (long)0;
	silver = (long)0;
	recharge = (long)0;
	level = 0;
	lastLoginTime = 0;
	lastLoginDate = 0;
	isBanded = false;
}

public Common_UserLogInfo(
	 long _cid
	, String _name
	, long _gem
	, long _cardCoin
	, long _silver
	, long _recharge
	, int _level
	, int _lastLoginTime
	, int _lastLoginDate
	, boolean _isBanded
) {	cid = _cid;
	name = _name;
	gem = _gem;
	cardCoin = _cardCoin;
	silver = _silver;
	recharge = _recharge;
	level = _level;
	lastLoginTime = _lastLoginTime;
	lastLoginDate = _lastLoginDate;
	isBanded = _isBanded;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public String getName() { return name; }
public void setName(String _name) { name = _name; }
public long getGem() { return gem; }
public void setGem(long _gem) { gem = _gem; }
public long getCardCoin() { return cardCoin; }
public void setCardCoin(long _cardCoin) { cardCoin = _cardCoin; }
public long getSilver() { return silver; }
public void setSilver(long _silver) { silver = _silver; }
public long getRecharge() { return recharge; }
public void setRecharge(long _recharge) { recharge = _recharge; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public int getLastLoginTime() { return lastLoginTime; }
public void setLastLoginTime(int _lastLoginTime) { lastLoginTime = _lastLoginTime; }
public int getLastLoginDate() { return lastLoginDate; }
public void setLastLoginDate(int _lastLoginDate) { lastLoginDate = _lastLoginDate; }
public boolean getIsBanded() { return isBanded; }
public void setIsBanded(boolean _isBanded) { isBanded = _isBanded; }


public final int GetBufSize() {
	int _size = 53;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 55;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gem = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cardCoin = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) silver = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) recharge = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastLoginTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastLoginDate = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isBanded = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	_buf.putLong(gem);
	_buf.putLong(cardCoin);
	_buf.putLong(silver);
	_buf.putLong(recharge);
	_buf.putInt(level);
	_buf.putInt(lastLoginTime);
	_buf.putInt(lastLoginDate);
	_buf.put(isBanded?(byte)1:(byte)0);
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

