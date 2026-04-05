package Common.MailObj;

import java.nio.ByteBuffer;
/*********
 * 邮件额外信息-HERO_UP_STEP骑士升阶
 **/
public class Mail_DetailExt_HeroUpgradeStep implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
private int step;


public Mail_DetailExt_HeroUpgradeStep() {
	heroId = (long)0;
	step = 0;
}

public Mail_DetailExt_HeroUpgradeStep(
	 long _heroId
	, int _step
) {	heroId = _heroId;
	step = _step;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public int getStep() { return step; }
public void setStep(int _step) { step = _step; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) step = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putInt(step);
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

