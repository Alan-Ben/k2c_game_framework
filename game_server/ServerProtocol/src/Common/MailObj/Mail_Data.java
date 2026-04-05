package Common.MailObj;

import java.nio.ByteBuffer;
/*********
 * 邮件数据
 **/
public class Mail_Data implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件配表id,可以为0 */
private long mailRefId;
/** 发送者id,为0读取配表 */
private int senderId;
/** 标题,为空读取配表 */
private String title;
/** 内容,为空读取配表 */
private String content;
/** 替换的参数内容 */
private java.util.ArrayList<String> contentReplace;
/** 附件物品列表 */
private NPCommon.NPCommon_ItemList itemList;
/** 额外信息类型id */
private int exType;
/** 额外信息内容 */
private byte[] exData;
/** 创建时间 */
private int createTimeSec;
/** 创建时间 */
private int expiredTimeSec;
/** 是否必读 */
private boolean isMustRead;
/** 后台邮件id */
private long phpMailId;
/** 额外标题信息内容 */
private byte[] exTitleData;


public Mail_Data() {
	mailRefId = (long)0;
	senderId = 0;
	title = "";
	content = "";
	contentReplace = new java.util.ArrayList<String>();
	itemList = new NPCommon.NPCommon_ItemList();
	exType = 0;
	exData = null;
	createTimeSec = 0;
	expiredTimeSec = 0;
	isMustRead = false;
	phpMailId = (long)0;
	exTitleData = null;
}

public Mail_Data(
	 long _mailRefId
	, int _senderId
	, String _title
	, String _content
	, java.util.ArrayList<String> _contentReplace
	, NPCommon.NPCommon_ItemList _itemList
	, int _exType
	, byte[] _exData
	, int _createTimeSec
	, int _expiredTimeSec
	, boolean _isMustRead
	, long _phpMailId
	, byte[] _exTitleData
) {	mailRefId = _mailRefId;
	senderId = _senderId;
	title = _title;
	content = _content;
	contentReplace = _contentReplace;
	itemList = _itemList;
	exType = _exType;
	exData = _exData;
	createTimeSec = _createTimeSec;
	expiredTimeSec = _expiredTimeSec;
	isMustRead = _isMustRead;
	phpMailId = _phpMailId;
	exTitleData = _exTitleData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 邮件配表id,可以为0 */
public long getMailRefId() { return mailRefId; }
/** 邮件配表id,可以为0 */
public void setMailRefId(long _mailRefId) { mailRefId = _mailRefId; }
/** 发送者id,为0读取配表 */
public int getSenderId() { return senderId; }
/** 发送者id,为0读取配表 */
public void setSenderId(int _senderId) { senderId = _senderId; }
/** 标题,为空读取配表 */
public String getTitle() { return title; }
/** 标题,为空读取配表 */
public void setTitle(String _title) { title = _title; }
/** 内容,为空读取配表 */
public String getContent() { return content; }
/** 内容,为空读取配表 */
public void setContent(String _content) { content = _content; }
/** 替换的参数内容 */
public java.util.ArrayList<String> getContentReplace() { return contentReplace; }
/** 替换的参数内容 */
public void addContentReplace(String _contentReplace) { contentReplace.add(_contentReplace); }
/** 附件物品列表 */
public NPCommon.NPCommon_ItemList getItemList() { return itemList; }
/** 附件物品列表 */
public void setItemList(NPCommon.NPCommon_ItemList _itemList) { itemList = _itemList; }
/** 额外信息类型id */
public int getExType() { return exType; }
/** 额外信息类型id */
public void setExType(int _exType) { exType = _exType; }
/** 额外信息内容 */
public byte[] getExData() { return exData; }
public java.nio.ByteBuffer get_buffer_ExData() { if(null == exData)return null; else return ByteBuffer.wrap(exData); }

/** 额外信息内容 */
public void setExData(byte[] _exData) { exData = _exData; }
public void setExData(java.nio.ByteBuffer _exData) 
{
	if(null == _exData){return;}
	int _oldPos = _exData.position();
	int _bufLength = _exData.remaining();
	exData = new byte[_bufLength];
	_exData.get(exData);
	_exData.position(_oldPos);
}

/** 创建时间 */
public int getCreateTimeSec() { return createTimeSec; }
/** 创建时间 */
public void setCreateTimeSec(int _createTimeSec) { createTimeSec = _createTimeSec; }
/** 创建时间 */
public int getExpiredTimeSec() { return expiredTimeSec; }
/** 创建时间 */
public void setExpiredTimeSec(int _expiredTimeSec) { expiredTimeSec = _expiredTimeSec; }
/** 是否必读 */
public boolean getIsMustRead() { return isMustRead; }
/** 是否必读 */
public void setIsMustRead(boolean _isMustRead) { isMustRead = _isMustRead; }
/** 后台邮件id */
public long getPhpMailId() { return phpMailId; }
/** 后台邮件id */
public void setPhpMailId(long _phpMailId) { phpMailId = _phpMailId; }
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
	int _size = 33;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
	for(int _i = 0; _i < contentReplace.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace.get(_i));
	}

	_size += 4 + itemList.GetBufSize();
	_size += 4 + (exData == null ? 0 : exData.length);
	_size += 4 + (exTitleData == null ? 0 : exTitleData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 35;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
	for(int _i = 0; _i < contentReplace.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace.get(_i));
	}

	_size += 4 + itemList.GetBufSize();
	_size += 4 + (exData == null ? 0 : exData.length);
	_size += 4 + (exTitleData == null ? 0 : exTitleData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) senderId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) title = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
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
	int _itemListCustLen = _buf.getInt();
	int _itemListCurPos = _buf.position();
	itemList.ReadUnzipBuf(_buf, _itemListCurPos + _itemListCustLen);
	_buf.position(_itemListCurPos + _itemListCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) exType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _exDataCount = _buf.getInt();
	if(0 < _exDataCount){
		exData = new byte[_exDataCount];
		_buf.get(exData);
	}

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expiredTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isMustRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) phpMailId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _exTitleDataCount = _buf.getInt();
	if(0 < _exTitleDataCount){
		exTitleData = new byte[_exTitleDataCount];
		_buf.get(exTitleData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mailRefId);
	_buf.putInt(senderId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, title);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	_buf.putShort((short)contentReplace.size());
	for(int _i = 0; _i < contentReplace.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, contentReplace.get(_i));
	}
	_buf.putInt(itemList.GetBufSize());
	itemList.PutUnzipBuf(_buf);
	_buf.putInt(exType);
	_buf.putInt((exData == null ? 0 : exData.length));
	if(null != exData){_buf.put(exData);}

	_buf.putInt(createTimeSec);
	_buf.putInt(expiredTimeSec);
	_buf.put(isMustRead?(byte)1:(byte)0);
	_buf.putLong(phpMailId);
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

