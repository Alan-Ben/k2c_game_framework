package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 平台邮件
 **/
public class NpServerObj_PlatFormMail implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件唯一id */
private long mailUid;
/** 邮件配置id */
private long mailRefId;
/** 后台邮件id */
private long phpMailId;
/** 发送时间 */
private String sendTime;
/** 过期时间 */
private String expiredTime;
/** 默认语言 */
private String defaultLang;
/** 邮件审核时间戳 */
private long passedTimeMs;
/** 平台邮件内容 */
private java.util.ArrayList<Common.NpServerObj.NpServerObj_PlatFormMailText> platformMailTextList;
/** 平台邮件附件 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> itemList;
/** 邮件内容替换数据 */
private java.util.ArrayList<String> contentReplace;


public NpServerObj_PlatFormMail() {
	mailUid = (long)0;
	mailRefId = (long)0;
	phpMailId = (long)0;
	sendTime = "";
	expiredTime = "";
	defaultLang = "";
	passedTimeMs = (long)0;
	platformMailTextList = new java.util.ArrayList<Common.NpServerObj.NpServerObj_PlatFormMailText>();
	itemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	contentReplace = new java.util.ArrayList<String>();
}

public NpServerObj_PlatFormMail(
	 long _mailUid
	, long _mailRefId
	, long _phpMailId
	, String _sendTime
	, String _expiredTime
	, String _defaultLang
	, long _passedTimeMs
	, java.util.ArrayList<Common.NpServerObj.NpServerObj_PlatFormMailText> _platformMailTextList
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _itemList
	, java.util.ArrayList<String> _contentReplace
) {	mailUid = _mailUid;
	mailRefId = _mailRefId;
	phpMailId = _phpMailId;
	sendTime = _sendTime;
	expiredTime = _expiredTime;
	defaultLang = _defaultLang;
	passedTimeMs = _passedTimeMs;
	platformMailTextList = _platformMailTextList;
	itemList = _itemList;
	contentReplace = _contentReplace;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 邮件唯一id */
public long getMailUid() { return mailUid; }
/** 邮件唯一id */
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/** 邮件配置id */
public long getMailRefId() { return mailRefId; }
/** 邮件配置id */
public void setMailRefId(long _mailRefId) { mailRefId = _mailRefId; }
/** 后台邮件id */
public long getPhpMailId() { return phpMailId; }
/** 后台邮件id */
public void setPhpMailId(long _phpMailId) { phpMailId = _phpMailId; }
/** 发送时间 */
public String getSendTime() { return sendTime; }
/** 发送时间 */
public void setSendTime(String _sendTime) { sendTime = _sendTime; }
/** 过期时间 */
public String getExpiredTime() { return expiredTime; }
/** 过期时间 */
public void setExpiredTime(String _expiredTime) { expiredTime = _expiredTime; }
/** 默认语言 */
public String getDefaultLang() { return defaultLang; }
/** 默认语言 */
public void setDefaultLang(String _defaultLang) { defaultLang = _defaultLang; }
/** 邮件审核时间戳 */
public long getPassedTimeMs() { return passedTimeMs; }
/** 邮件审核时间戳 */
public void setPassedTimeMs(long _passedTimeMs) { passedTimeMs = _passedTimeMs; }
/** 平台邮件内容 */
public java.util.ArrayList<Common.NpServerObj.NpServerObj_PlatFormMailText> getPlatformMailTextList() { return platformMailTextList; }
/** 平台邮件内容 */
public void addPlatformMailTextList(Common.NpServerObj.NpServerObj_PlatFormMailText _platformMailTextList) { platformMailTextList.add(_platformMailTextList); }
/** 平台邮件附件 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/** 平台邮件附件 */
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.add(_itemList); }
/** 邮件内容替换数据 */
public java.util.ArrayList<String> getContentReplace() { return contentReplace; }
/** 邮件内容替换数据 */
public void addContentReplace(String _contentReplace) { contentReplace.add(_contentReplace); }


public final int GetBufSize() {
	int _size = 32;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sendTime);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(expiredTime);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(defaultLang);
	_size += 2;
	for(int _i = 0; _i < platformMailTextList.size(); _i++) {
	_size += 4 + platformMailTextList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < contentReplace.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace.get(_i));
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sendTime);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(expiredTime);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(defaultLang);
	_size += 2;
	for(int _i = 0; _i < platformMailTextList.size(); _i++) {
	_size += 4 + platformMailTextList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < contentReplace.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace.get(_i));
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) phpMailId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sendTime = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expiredTime = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) defaultLang = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) passedTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _platformMailTextListCount = _buf.getShort();
	for(int _i = 0; _i < _platformMailTextListCount; _i++) { 
		Common.NpServerObj.NpServerObj_PlatFormMailText _platformMailTextList = new Common.NpServerObj.NpServerObj_PlatFormMailText();
		if(_buf.remaining() <= 0) return;
	int __platformMailTextListCustLen = _buf.getInt();
	int __platformMailTextListCurPos = _buf.position();
	_platformMailTextList.ReadUnzipBuf(_buf, __platformMailTextListCurPos + __platformMailTextListCustLen);
	_buf.position(__platformMailTextListCurPos + __platformMailTextListCustLen);

		platformMailTextList.add(_platformMailTextList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.position();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.position(__itemListCurPos + __itemListCustLen);

		itemList.add(_itemList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _contentReplaceCount = _buf.getShort();
	for(int _i = 0; _i < _contentReplaceCount; _i++) { 
		String _contentReplace = "";
		if(_buf.remaining() > 0) _contentReplace = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		contentReplace.add(_contentReplace);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mailUid);
	_buf.putLong(mailRefId);
	_buf.putLong(phpMailId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, sendTime);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, expiredTime);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, defaultLang);
	_buf.putLong(passedTimeMs);
	_buf.putShort((short)platformMailTextList.size());
	for(int _i = 0; _i < platformMailTextList.size(); _i++) { 
		_buf.putInt(platformMailTextList.get(_i).GetBufSize());
	platformMailTextList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)contentReplace.size());
	for(int _i = 0; _i < contentReplace.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, contentReplace.get(_i));
	}
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

