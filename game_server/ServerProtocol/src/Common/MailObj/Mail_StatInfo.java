package Common.MailObj;

import java.nio.ByteBuffer;
/*********
 * 邮件统计信息
 **/
public class Mail_StatInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 总邮件数量 */
private int totalMailCount;
/** 未读邮件数量 */
private int unReadCount;
/** 未领取邮件数量 */
private int unTakeCount;


public Mail_StatInfo() {
	totalMailCount = 0;
	unReadCount = 0;
	unTakeCount = 0;
}

public Mail_StatInfo(
	 int _totalMailCount
	, int _unReadCount
	, int _unTakeCount
) {	totalMailCount = _totalMailCount;
	unReadCount = _unReadCount;
	unTakeCount = _unTakeCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 总邮件数量 */
public int getTotalMailCount() { return totalMailCount; }
/** 总邮件数量 */
public void setTotalMailCount(int _totalMailCount) { totalMailCount = _totalMailCount; }
/** 未读邮件数量 */
public int getUnReadCount() { return unReadCount; }
/** 未读邮件数量 */
public void setUnReadCount(int _unReadCount) { unReadCount = _unReadCount; }
/** 未领取邮件数量 */
public int getUnTakeCount() { return unTakeCount; }
/** 未领取邮件数量 */
public void setUnTakeCount(int _unTakeCount) { unTakeCount = _unTakeCount; }


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
	if(_buf.remaining() > 0) totalMailCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) unReadCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) unTakeCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(totalMailCount);
	_buf.putInt(unReadCount);
	_buf.putInt(unTakeCount);
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

