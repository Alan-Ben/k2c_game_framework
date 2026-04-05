package GC2GS.p009_MailOp;

import java.nio.ByteBuffer;
/*********
 * 设置已读完
 **/
public class GC2GS_009_009_ReqSetReadOver implements ALBasicProtocolPack._IALProtocolStructure {
/** 设置已读完 */
private long mailUid;


public GC2GS_009_009_ReqSetReadOver() {
	mailUid = (long)0;
}

public GC2GS_009_009_ReqSetReadOver(
	 long _mailUid
) {	mailUid = _mailUid;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)9; }

/** 设置已读完 */
public long getMailUid() { return mailUid; }
/** 设置已读完 */
public void setMailUid(long _mailUid) { mailUid = _mailUid; }


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
	if(_buf.remaining() > 0) mailUid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mailUid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)9);
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

