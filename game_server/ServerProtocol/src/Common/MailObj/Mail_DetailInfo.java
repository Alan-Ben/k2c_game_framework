package Common.MailObj;

import java.nio.ByteBuffer;
/*********
 * 邮件详细信息
 **/
public class Mail_DetailInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件唯一id */
private long mailUid;
/** 邮件文本内容 */
private String content;
/** 替换的参数内容 */
private java.util.ArrayList<String> contentReplace;
/** 附件物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> itemList;
/** 额外信息类型id */
private int exType;
/** 额外信息内容 */
private byte[] exData;


public Mail_DetailInfo() {
	mailUid = (long)0;
	content = "";
	contentReplace = new java.util.ArrayList<String>();
	itemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	exType = 0;
	exData = null;
}

public Mail_DetailInfo(
	 long _mailUid
	, String _content
	, java.util.ArrayList<String> _contentReplace
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _itemList
	, int _exType
	, byte[] _exData
) {	mailUid = _mailUid;
	content = _content;
	contentReplace = _contentReplace;
	itemList = _itemList;
	exType = _exType;
	exData = _exData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 邮件唯一id */
public long getMailUid() { return mailUid; }
/** 邮件唯一id */
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/** 邮件文本内容 */
public String getContent() { return content; }
/** 邮件文本内容 */
public void setContent(String _content) { content = _content; }
/** 替换的参数内容 */
public java.util.ArrayList<String> getContentReplace() { return contentReplace; }
/** 替换的参数内容 */
public void addContentReplace(String _contentReplace) { contentReplace.add(_contentReplace); }
/** 附件物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/** 附件物品列表 */
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.add(_itemList); }
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



public final int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
	for(int _i = 0; _i < contentReplace.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace.get(_i));
	}

	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}

	_size += 4 + (exData == null ? 0 : exData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
	for(int _i = 0; _i < contentReplace.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace.get(_i));
	}

	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}

	_size += 4 + (exData == null ? 0 : exData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailUid = _buf.getLong();
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
	if(_buf.remaining() > 0) exType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _exDataCount = _buf.getInt();
	if(0 < _exDataCount){
		exData = new byte[_exDataCount];
		_buf.get(exData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mailUid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	_buf.putShort((short)contentReplace.size());
	for(int _i = 0; _i < contentReplace.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, contentReplace.get(_i));
	}
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(exType);
	_buf.putInt((exData == null ? 0 : exData.length));
	if(null != exData){_buf.put(exData);}

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

