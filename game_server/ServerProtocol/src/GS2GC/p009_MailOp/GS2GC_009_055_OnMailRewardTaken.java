package GS2GC.p009_MailOp;

import java.nio.ByteBuffer;
public class GS2GC_009_055_OnMailRewardTaken implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件唯一id列表 */
private java.util.ArrayList<Long> mailUidList;


public GS2GC_009_055_OnMailRewardTaken() {
	mailUidList = new java.util.ArrayList<Long>();
}

public GS2GC_009_055_OnMailRewardTaken(
	 java.util.ArrayList<Long> _mailUidList
) {	mailUidList = _mailUidList;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)55; }

/** 邮件唯一id列表 */
public java.util.ArrayList<Long> getMailUidList() { return mailUidList; }
/** 邮件唯一id列表 */
public void addMailUidList(long _mailUidList) { mailUidList.add(_mailUidList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (mailUidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (mailUidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _mailUidListCount = _buf.getShort();
	for(int _i = 0; _i < _mailUidListCount; _i++) { 
		long _mailUidList = (long)0;
		if(_buf.remaining() > 0) _mailUidList = _buf.getLong();
		mailUidList.add(_mailUidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)mailUidList.size());
	for(int _i = 0; _i < mailUidList.size(); _i++) { 
		_buf.putLong(mailUidList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)55);
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

