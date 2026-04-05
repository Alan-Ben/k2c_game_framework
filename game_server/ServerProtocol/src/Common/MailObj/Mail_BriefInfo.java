package Common.MailObj;

import java.nio.ByteBuffer;
/*********
 * 邮件统简单信息
 **/
public class Mail_BriefInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件唯一id */
private long mailUid;
/** 获得邮件时间，秒 */
private int gainTimeSec;
/** 是否已读 */
private boolean isRead;
/** 是否已领取物品 */
private boolean hasTaken;
/** 首个附件物品 */
private NPCommon.NPCommon_ItemInfo item;
/** 是否必读 */
private boolean isMustRead;
/** 是否收藏 */
private boolean isLocked;
/** 邮件配表id */
private long mailRefId;


public Mail_BriefInfo() {
	mailUid = (long)0;
	gainTimeSec = 0;
	isRead = false;
	hasTaken = false;
	item = new NPCommon.NPCommon_ItemInfo();
	isMustRead = false;
	isLocked = false;
	mailRefId = (long)0;
}

public Mail_BriefInfo(
	 long _mailUid
	, int _gainTimeSec
	, boolean _isRead
	, boolean _hasTaken
	, NPCommon.NPCommon_ItemInfo _item
	, boolean _isMustRead
	, boolean _isLocked
	, long _mailRefId
) {	mailUid = _mailUid;
	gainTimeSec = _gainTimeSec;
	isRead = _isRead;
	hasTaken = _hasTaken;
	item = _item;
	isMustRead = _isMustRead;
	isLocked = _isLocked;
	mailRefId = _mailRefId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 邮件唯一id */
public long getMailUid() { return mailUid; }
/** 邮件唯一id */
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/** 获得邮件时间，秒 */
public int getGainTimeSec() { return gainTimeSec; }
/** 获得邮件时间，秒 */
public void setGainTimeSec(int _gainTimeSec) { gainTimeSec = _gainTimeSec; }
/** 是否已读 */
public boolean getIsRead() { return isRead; }
/** 是否已读 */
public void setIsRead(boolean _isRead) { isRead = _isRead; }
/** 是否已领取物品 */
public boolean getHasTaken() { return hasTaken; }
/** 是否已领取物品 */
public void setHasTaken(boolean _hasTaken) { hasTaken = _hasTaken; }
/** 首个附件物品 */
public NPCommon.NPCommon_ItemInfo getItem() { return item; }
/** 首个附件物品 */
public void setItem(NPCommon.NPCommon_ItemInfo _item) { item = _item; }
/** 是否必读 */
public boolean getIsMustRead() { return isMustRead; }
/** 是否必读 */
public void setIsMustRead(boolean _isMustRead) { isMustRead = _isMustRead; }
/** 是否收藏 */
public boolean getIsLocked() { return isLocked; }
/** 是否收藏 */
public void setIsLocked(boolean _isLocked) { isLocked = _isLocked; }
/** 邮件配表id */
public long getMailRefId() { return mailRefId; }
/** 邮件配表id */
public void setMailRefId(long _mailRefId) { mailRefId = _mailRefId; }


public final int GetBufSize() {
	int _size = 24;
	_size += 4 + item.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;
	_size += 4 + item.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasTaken = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _itemCustLen = _buf.getInt();
	int _itemCurPos = _buf.position();
	item.ReadUnzipBuf(_buf, _itemCurPos + _itemCustLen);
	_buf.position(_itemCurPos + _itemCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isMustRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isLocked = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailRefId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mailUid);
	_buf.putInt(gainTimeSec);
	_buf.put(isRead?(byte)1:(byte)0);
	_buf.put(hasTaken?(byte)1:(byte)0);
	_buf.putInt(item.GetBufSize());
	item.PutUnzipBuf(_buf);
	_buf.put(isMustRead?(byte)1:(byte)0);
	_buf.put(isLocked?(byte)1:(byte)0);
	_buf.putLong(mailRefId);
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

