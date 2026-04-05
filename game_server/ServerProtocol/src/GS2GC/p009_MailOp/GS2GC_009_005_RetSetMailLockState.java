package GS2GC.p009_MailOp;

import java.nio.ByteBuffer;
public class GS2GC_009_005_RetSetMailLockState implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件唯一id */
private long mailUid;
/** 是否收藏 */
private boolean isLocked;


public GS2GC_009_005_RetSetMailLockState() {
	mailUid = (long)0;
	isLocked = false;
}

public GS2GC_009_005_RetSetMailLockState(
	 long _mailUid
	, boolean _isLocked
) {	mailUid = _mailUid;
	isLocked = _isLocked;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)5; }

/** 邮件唯一id */
public long getMailUid() { return mailUid; }
/** 邮件唯一id */
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/** 是否收藏 */
public boolean getIsLocked() { return isLocked; }
/** 是否收藏 */
public void setIsLocked(boolean _isLocked) { isLocked = _isLocked; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isLocked = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mailUid);
	_buf.put(isLocked?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)5);
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

