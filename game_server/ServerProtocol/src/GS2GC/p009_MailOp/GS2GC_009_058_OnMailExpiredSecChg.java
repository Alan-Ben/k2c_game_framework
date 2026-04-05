package GS2GC.p009_MailOp;

import java.nio.ByteBuffer;
public class GS2GC_009_058_OnMailExpiredSecChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件唯一id */
private long mailUid;
/** 新的邮件截至时间戳（秒） */
private int expiredTimeSec;


public GS2GC_009_058_OnMailExpiredSecChg() {
	mailUid = (long)0;
	expiredTimeSec = 0;
}

public GS2GC_009_058_OnMailExpiredSecChg(
	 long _mailUid
	, int _expiredTimeSec
) {	mailUid = _mailUid;
	expiredTimeSec = _expiredTimeSec;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)58; }

/** 邮件唯一id */
public long getMailUid() { return mailUid; }
/** 邮件唯一id */
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/** 新的邮件截至时间戳（秒） */
public int getExpiredTimeSec() { return expiredTimeSec; }
/** 新的邮件截至时间戳（秒） */
public void setExpiredTimeSec(int _expiredTimeSec) { expiredTimeSec = _expiredTimeSec; }


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
	if(_buf.remaining() > 0) mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expiredTimeSec = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mailUid);
	_buf.putInt(expiredTimeSec);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)58);
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

