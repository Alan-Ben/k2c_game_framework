package Common.MailObj;

import java.nio.ByteBuffer;
/*********
 * 邮件系统标题展示信息
 **/
public class Mail_TitleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件唯一id */
private long mailUid;
/** 发送者 */
private int senderId;
/** 邮件配表id,由配表id决定类型 */
private long mailRefId;
/** 邮件标题 */
private String title;
/** 结束时间戳，秒 */
private int endTimeSec;
/** 获得邮件时间，秒 */
private int gainTimeSec;
/** 是否收藏 */
private boolean isLocked;
/** 是否已读 */
private boolean isRead;
/** 是否有物品 */
private boolean hasItem;
/** 是否已领取物品 */
private boolean hasTaken;
/** 是否已经读完 */
private boolean isReadOver;
/** 是否必读 */
private boolean isMustRead;
/** 替换文本列表 */
private java.util.ArrayList<String> contentReplace;
/** 额外标题信息内容 */
private byte[] exTitleData;


public Mail_TitleInfo() {
	mailUid = (long)0;
	senderId = 0;
	mailRefId = (long)0;
	title = "";
	endTimeSec = 0;
	gainTimeSec = 0;
	isLocked = false;
	isRead = false;
	hasItem = false;
	hasTaken = false;
	isReadOver = false;
	isMustRead = false;
	contentReplace = new java.util.ArrayList<String>();
	exTitleData = null;
}

public Mail_TitleInfo(
	 long _mailUid
	, int _senderId
	, long _mailRefId
	, String _title
	, int _endTimeSec
	, int _gainTimeSec
	, boolean _isLocked
	, boolean _isRead
	, boolean _hasItem
	, boolean _hasTaken
	, boolean _isReadOver
	, boolean _isMustRead
	, java.util.ArrayList<String> _contentReplace
	, byte[] _exTitleData
) {	mailUid = _mailUid;
	senderId = _senderId;
	mailRefId = _mailRefId;
	title = _title;
	endTimeSec = _endTimeSec;
	gainTimeSec = _gainTimeSec;
	isLocked = _isLocked;
	isRead = _isRead;
	hasItem = _hasItem;
	hasTaken = _hasTaken;
	isReadOver = _isReadOver;
	isMustRead = _isMustRead;
	contentReplace = _contentReplace;
	exTitleData = _exTitleData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 邮件唯一id */
public long getMailUid() { return mailUid; }
/** 邮件唯一id */
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/** 发送者 */
public int getSenderId() { return senderId; }
/** 发送者 */
public void setSenderId(int _senderId) { senderId = _senderId; }
/** 邮件配表id,由配表id决定类型 */
public long getMailRefId() { return mailRefId; }
/** 邮件配表id,由配表id决定类型 */
public void setMailRefId(long _mailRefId) { mailRefId = _mailRefId; }
/** 邮件标题 */
public String getTitle() { return title; }
/** 邮件标题 */
public void setTitle(String _title) { title = _title; }
/** 结束时间戳，秒 */
public int getEndTimeSec() { return endTimeSec; }
/** 结束时间戳，秒 */
public void setEndTimeSec(int _endTimeSec) { endTimeSec = _endTimeSec; }
/** 获得邮件时间，秒 */
public int getGainTimeSec() { return gainTimeSec; }
/** 获得邮件时间，秒 */
public void setGainTimeSec(int _gainTimeSec) { gainTimeSec = _gainTimeSec; }
/** 是否收藏 */
public boolean getIsLocked() { return isLocked; }
/** 是否收藏 */
public void setIsLocked(boolean _isLocked) { isLocked = _isLocked; }
/** 是否已读 */
public boolean getIsRead() { return isRead; }
/** 是否已读 */
public void setIsRead(boolean _isRead) { isRead = _isRead; }
/** 是否有物品 */
public boolean getHasItem() { return hasItem; }
/** 是否有物品 */
public void setHasItem(boolean _hasItem) { hasItem = _hasItem; }
/** 是否已领取物品 */
public boolean getHasTaken() { return hasTaken; }
/** 是否已领取物品 */
public void setHasTaken(boolean _hasTaken) { hasTaken = _hasTaken; }
/** 是否已经读完 */
public boolean getIsReadOver() { return isReadOver; }
/** 是否已经读完 */
public void setIsReadOver(boolean _isReadOver) { isReadOver = _isReadOver; }
/** 是否必读 */
public boolean getIsMustRead() { return isMustRead; }
/** 是否必读 */
public void setIsMustRead(boolean _isMustRead) { isMustRead = _isMustRead; }
/** 替换文本列表 */
public java.util.ArrayList<String> getContentReplace() { return contentReplace; }
/** 替换文本列表 */
public void addContentReplace(String _contentReplace) { contentReplace.add(_contentReplace); }
/** 额外标题信息内容 */
public byte[] getExTitleData() { return exTitleData; }
public java.nio.ByteBuffer get_buffer_ExTitleData() { if(null == exTitleData)return null; else return ByteBuffer.wrap(exTitleData); }

/** 额外标题信息内容 */
public void setExTitleData(byte[] _exTitleData) { exTitleData = _exTitleData; }
public void setExTitleData(java.nio.ByteBuffer _exTitleData) 
{
	if(null == _exTitleData){return;}
	int _oldPos = _exTitleData.position();
	int _bufLength = _exTitleData.remaining();
	exTitleData = new byte[_bufLength];
	_exTitleData.get(exTitleData);
	_exTitleData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += 2;
	for(int _i = 0; _i < contentReplace.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace.get(_i));
	}

	_size += 4 + (exTitleData == null ? 0 : exTitleData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 36;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += 2;
	for(int _i = 0; _i < contentReplace.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace.get(_i));
	}

	_size += 4 + (exTitleData == null ? 0 : exTitleData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) senderId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) title = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isLocked = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasItem = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasTaken = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isReadOver = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isMustRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _contentReplaceCount = _buf.getShort();
	for(int _i = 0; _i < _contentReplaceCount; _i++) { 
		String _contentReplace = "";
		if(_buf.remaining() > 0) _contentReplace = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		contentReplace.add(_contentReplace);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _exTitleDataCount = _buf.getInt();
	if(0 < _exTitleDataCount){
		exTitleData = new byte[_exTitleDataCount];
		_buf.get(exTitleData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mailUid);
	_buf.putInt(senderId);
	_buf.putLong(mailRefId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, title);
	_buf.putInt(endTimeSec);
	_buf.putInt(gainTimeSec);
	_buf.put(isLocked?(byte)1:(byte)0);
	_buf.put(isRead?(byte)1:(byte)0);
	_buf.put(hasItem?(byte)1:(byte)0);
	_buf.put(hasTaken?(byte)1:(byte)0);
	_buf.put(isReadOver?(byte)1:(byte)0);
	_buf.put(isMustRead?(byte)1:(byte)0);
	_buf.putShort((short)contentReplace.size());
	for(int _i = 0; _i < contentReplace.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, contentReplace.get(_i));
	}
	_buf.putInt((exTitleData == null ? 0 : exTitleData.length));
	if(null != exTitleData){_buf.put(exTitleData);}

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

