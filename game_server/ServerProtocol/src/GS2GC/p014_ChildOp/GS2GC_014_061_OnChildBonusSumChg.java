package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 所有子嗣（成年/未成年）总收益变动推送
 **/
public class GS2GC_014_061_OnChildBonusSumChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 所有子嗣总收益 */
private long bonus;


public GS2GC_014_061_OnChildBonusSumChg() {
	bonus = (long)0;
}

public GS2GC_014_061_OnChildBonusSumChg(
	 long _bonus
) {	bonus = _bonus;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)61; }

/** 所有子嗣总收益 */
public long getBonus() { return bonus; }
/** 所有子嗣总收益 */
public void setBonus(long _bonus) { bonus = _bonus; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bonus = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(bonus);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)61);
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

