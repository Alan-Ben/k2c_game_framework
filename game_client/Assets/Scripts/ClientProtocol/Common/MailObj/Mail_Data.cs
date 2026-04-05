using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MailObj
{

/// <summary>
/// 邮件数据
/// </summary>
public class Mail_Data : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件配表id,可以为0
/// </summary>
private long mailRefId;
/// <summary>
/// 发送者id,为0读取配表
/// </summary>
private int senderId;
/// <summary>
/// 标题,为空读取配表
/// </summary>
private string title;
/// <summary>
/// 内容,为空读取配表
/// </summary>
private string content;
/// <summary>
/// 替换的参数内容
/// </summary>
private List<string> contentReplace;
/// <summary>
/// 附件物品列表
/// </summary>
private NPCommon.NPCommon_ItemList itemList;
/// <summary>
/// 额外信息类型id
/// </summary>
private int exType;
/// <summary>
/// 额外信息内容
/// </summary>
private byte[] exData;
/// <summary>
/// 创建时间
/// </summary>
private int createTimeSec;
/// <summary>
/// 创建时间
/// </summary>
private int expiredTimeSec;
/// <summary>
/// 是否必读
/// </summary>
private bool isMustRead;
/// <summary>
/// 后台邮件id
/// </summary>
private long phpMailId;
/// <summary>
/// 额外标题信息内容
/// </summary>
private byte[] exTitleData;


public Mail_Data() {
	mailRefId = (long)0;
	senderId = 0;
	title = "";
	content = "";
	contentReplace = new List<string>();
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
	, string _title
	, string _content
	, List<string> _contentReplace
	, NPCommon.NPCommon_ItemList _itemList
	, int _exType
	, byte[] _exData
	, int _createTimeSec
	, int _expiredTimeSec
	, bool _isMustRead
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 邮件配表id,可以为0
/// </summary>
public long getMailRefId() { return mailRefId; }
/// <summary>
/// 邮件配表id,可以为0
/// </summary>
public void setMailRefId(long _mailRefId) { mailRefId = _mailRefId; }
/// <summary>
/// 发送者id,为0读取配表
/// </summary>
public int getSenderId() { return senderId; }
/// <summary>
/// 发送者id,为0读取配表
/// </summary>
public void setSenderId(int _senderId) { senderId = _senderId; }
/// <summary>
/// 标题,为空读取配表
/// </summary>
public string getTitle() { return title; }
/// <summary>
/// 标题,为空读取配表
/// </summary>
public void setTitle(string _title) { title = _title; }
/// <summary>
/// 内容,为空读取配表
/// </summary>
public string getContent() { return content; }
/// <summary>
/// 内容,为空读取配表
/// </summary>
public void setContent(string _content) { content = _content; }
/// <summary>
/// 替换的参数内容
/// </summary>
public List<string> getContentReplace() { return contentReplace; }
/// <summary>
/// 替换的参数内容
/// </summary>
public void addContentReplace(string _contentReplace) { contentReplace.Add(_contentReplace); }
/// <summary>
/// 附件物品列表
/// </summary>
public NPCommon.NPCommon_ItemList getItemList() { return itemList; }
/// <summary>
/// 附件物品列表
/// </summary>
public void setItemList(NPCommon.NPCommon_ItemList _itemList) { itemList = _itemList; }
/// <summary>
/// 额外信息类型id
/// </summary>
public int getExType() { return exType; }
/// <summary>
/// 额外信息类型id
/// </summary>
public void setExType(int _exType) { exType = _exType; }
/// <summary>
/// 额外信息内容
/// </summary>
public byte[] getExData() { return exData; }

/// <summary>
/// 额外信息内容
/// </summary>
public void setExData(byte[] _exData) { exData = _exData; }

/// <summary>
/// 创建时间
/// </summary>
public int getCreateTimeSec() { return createTimeSec; }
/// <summary>
/// 创建时间
/// </summary>
public void setCreateTimeSec(int _createTimeSec) { createTimeSec = _createTimeSec; }
/// <summary>
/// 创建时间
/// </summary>
public int getExpiredTimeSec() { return expiredTimeSec; }
/// <summary>
/// 创建时间
/// </summary>
public void setExpiredTimeSec(int _expiredTimeSec) { expiredTimeSec = _expiredTimeSec; }
/// <summary>
/// 是否必读
/// </summary>
public bool getIsMustRead() { return isMustRead; }
/// <summary>
/// 是否必读
/// </summary>
public void setIsMustRead(bool _isMustRead) { isMustRead = _isMustRead; }
/// <summary>
/// 后台邮件id
/// </summary>
public long getPhpMailId() { return phpMailId; }
/// <summary>
/// 后台邮件id
/// </summary>
public void setPhpMailId(long _phpMailId) { phpMailId = _phpMailId; }
/// <summary>
/// 额外标题信息内容
/// </summary>
public byte[] getExTitleData() { return exTitleData; }

/// <summary>
/// 额外标题信息内容
/// </summary>
public void setExTitleData(byte[] _exTitleData) { exTitleData = _exTitleData; }



public int GetBufSize() {
	int _size = 33;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
for(int _i = 0; _i < contentReplace.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace[_i]);
	}

	_size += 4 + itemList.GetBufSize();
	_size += 4 + (exData == null ? 0 : exData.Length);
	_size += 4 + (exTitleData == null ? 0 : exTitleData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 35;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
for(int _i = 0; _i < contentReplace.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace[_i]);
	}

	_size += 4 + itemList.GetBufSize();
	_size += 4 + (exData == null ? 0 : exData.Length);
	_size += 4 + (exTitleData == null ? 0 : exTitleData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mailRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	title = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _contentReplaceCount = _buf.getShort();
	for(int _i = 0; _i < _contentReplaceCount; _i++) { 
		string _contentReplace = "";
		_contentReplace = _buf.getString();
		contentReplace.Add(_contentReplace);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _itemListCustLen = _buf.getInt();
	int _itemListCurPos = _buf.getCurPos();
	itemList.ReadUnzipBuf(_buf, _itemListCurPos + _itemListCustLen);
	_buf.setPosition(_itemListCurPos + _itemListCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exData = _buf.getByteBuffer();

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expiredTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isMustRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	phpMailId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exTitleData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(mailRefId);
	_buf.putInt(senderId);
	_buf.putString(title);
	_buf.putString(content);
	_buf.putShort((short)contentReplace.Count);
	for(int _i = 0; _i < contentReplace.Count; _i++) { 
		_buf.putString(contentReplace[_i]);
	}
	_buf.putInt(itemList.GetBufSize());
	itemList.PutUnzipBuf(_buf);
	_buf.putInt(exType);
	_buf.putByteBuffer(exData);

	_buf.putInt(createTimeSec);
	_buf.putInt(expiredTimeSec);
	_buf.put(isMustRead?(byte)1:(byte)0);
	_buf.putLong(phpMailId);
	_buf.putByteBuffer(exTitleData);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("mailRefId").Append(":").Append(mailRefId.ToString()).Append(", ");
	builder.Append("senderId").Append(":").Append(senderId.ToString()).Append(", ");
	builder.Append("title").Append(":").Append(title.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("contentReplace").Append(":").Append(contentReplace.ToString()).Append(", ");
	builder.Append("itemList").Append(":").Append(itemList == null ? "null" : itemList.ToString()).Append(", ");
	builder.Append("exType").Append(":").Append(exType.ToString()).Append(", ");
	builder.Append("exData").Append(":").Append(exData == null ? "null" : exData.ToString()).Append(", ");
	builder.Append("createTimeSec").Append(":").Append(createTimeSec.ToString()).Append(", ");
	builder.Append("expiredTimeSec").Append(":").Append(expiredTimeSec.ToString()).Append(", ");
	builder.Append("isMustRead").Append(":").Append(isMustRead.ToString()).Append(", ");
	builder.Append("phpMailId").Append(":").Append(phpMailId.ToString()).Append(", ");
	builder.Append("exTitleData").Append(":").Append(exTitleData == null ? "null" : exTitleData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

