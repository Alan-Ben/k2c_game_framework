using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MailObj
{

/// <summary>
/// 邮件系统标题展示信息
/// </summary>
public class Mail_TitleInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件唯一id
/// </summary>
private long mailUid;
/// <summary>
/// 发送者
/// </summary>
private int senderId;
/// <summary>
/// 邮件配表id,由配表id决定类型
/// </summary>
private long mailRefId;
/// <summary>
/// 邮件标题
/// </summary>
private string title;
/// <summary>
/// 结束时间戳，秒
/// </summary>
private int endTimeSec;
/// <summary>
/// 获得邮件时间，秒
/// </summary>
private int gainTimeSec;
/// <summary>
/// 是否收藏
/// </summary>
private bool isLocked;
/// <summary>
/// 是否已读
/// </summary>
private bool isRead;
/// <summary>
/// 是否有物品
/// </summary>
private bool hasItem;
/// <summary>
/// 是否已领取物品
/// </summary>
private bool hasTaken;
/// <summary>
/// 是否已经读完
/// </summary>
private bool isReadOver;
/// <summary>
/// 是否必读
/// </summary>
private bool isMustRead;
/// <summary>
/// 替换文本列表
/// </summary>
private List<string> contentReplace;
/// <summary>
/// 额外标题信息内容
/// </summary>
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
	contentReplace = new List<string>();
	exTitleData = null;
}

public Mail_TitleInfo(
	long _mailUid
	, int _senderId
	, long _mailRefId
	, string _title
	, int _endTimeSec
	, int _gainTimeSec
	, bool _isLocked
	, bool _isRead
	, bool _hasItem
	, bool _hasTaken
	, bool _isReadOver
	, bool _isMustRead
	, List<string> _contentReplace
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 邮件唯一id
/// </summary>
public long getMailUid() { return mailUid; }
/// <summary>
/// 邮件唯一id
/// </summary>
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/// <summary>
/// 发送者
/// </summary>
public int getSenderId() { return senderId; }
/// <summary>
/// 发送者
/// </summary>
public void setSenderId(int _senderId) { senderId = _senderId; }
/// <summary>
/// 邮件配表id,由配表id决定类型
/// </summary>
public long getMailRefId() { return mailRefId; }
/// <summary>
/// 邮件配表id,由配表id决定类型
/// </summary>
public void setMailRefId(long _mailRefId) { mailRefId = _mailRefId; }
/// <summary>
/// 邮件标题
/// </summary>
public string getTitle() { return title; }
/// <summary>
/// 邮件标题
/// </summary>
public void setTitle(string _title) { title = _title; }
/// <summary>
/// 结束时间戳，秒
/// </summary>
public int getEndTimeSec() { return endTimeSec; }
/// <summary>
/// 结束时间戳，秒
/// </summary>
public void setEndTimeSec(int _endTimeSec) { endTimeSec = _endTimeSec; }
/// <summary>
/// 获得邮件时间，秒
/// </summary>
public int getGainTimeSec() { return gainTimeSec; }
/// <summary>
/// 获得邮件时间，秒
/// </summary>
public void setGainTimeSec(int _gainTimeSec) { gainTimeSec = _gainTimeSec; }
/// <summary>
/// 是否收藏
/// </summary>
public bool getIsLocked() { return isLocked; }
/// <summary>
/// 是否收藏
/// </summary>
public void setIsLocked(bool _isLocked) { isLocked = _isLocked; }
/// <summary>
/// 是否已读
/// </summary>
public bool getIsRead() { return isRead; }
/// <summary>
/// 是否已读
/// </summary>
public void setIsRead(bool _isRead) { isRead = _isRead; }
/// <summary>
/// 是否有物品
/// </summary>
public bool getHasItem() { return hasItem; }
/// <summary>
/// 是否有物品
/// </summary>
public void setHasItem(bool _hasItem) { hasItem = _hasItem; }
/// <summary>
/// 是否已领取物品
/// </summary>
public bool getHasTaken() { return hasTaken; }
/// <summary>
/// 是否已领取物品
/// </summary>
public void setHasTaken(bool _hasTaken) { hasTaken = _hasTaken; }
/// <summary>
/// 是否已经读完
/// </summary>
public bool getIsReadOver() { return isReadOver; }
/// <summary>
/// 是否已经读完
/// </summary>
public void setIsReadOver(bool _isReadOver) { isReadOver = _isReadOver; }
/// <summary>
/// 是否必读
/// </summary>
public bool getIsMustRead() { return isMustRead; }
/// <summary>
/// 是否必读
/// </summary>
public void setIsMustRead(bool _isMustRead) { isMustRead = _isMustRead; }
/// <summary>
/// 替换文本列表
/// </summary>
public List<string> getContentReplace() { return contentReplace; }
/// <summary>
/// 替换文本列表
/// </summary>
public void addContentReplace(string _contentReplace) { contentReplace.Add(_contentReplace); }
/// <summary>
/// 额外标题信息内容
/// </summary>
public byte[] getExTitleData() { return exTitleData; }

/// <summary>
/// 额外标题信息内容
/// </summary>
public void setExTitleData(byte[] _exTitleData) { exTitleData = _exTitleData; }



public int GetBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += 2;
for(int _i = 0; _i < contentReplace.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace[_i]);
	}

	_size += 4 + (exTitleData == null ? 0 : exTitleData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 36;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += 2;
for(int _i = 0; _i < contentReplace.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace[_i]);
	}

	_size += 4 + (exTitleData == null ? 0 : exTitleData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mailRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	title = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isLocked = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasItem = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasTaken = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isReadOver = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isMustRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _contentReplaceCount = _buf.getShort();
	for(int _i = 0; _i < _contentReplaceCount; _i++) { 
		string _contentReplace = "";
		_contentReplace = _buf.getString();
		contentReplace.Add(_contentReplace);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exTitleData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(mailUid);
	_buf.putInt(senderId);
	_buf.putLong(mailRefId);
	_buf.putString(title);
	_buf.putInt(endTimeSec);
	_buf.putInt(gainTimeSec);
	_buf.put(isLocked?(byte)1:(byte)0);
	_buf.put(isRead?(byte)1:(byte)0);
	_buf.put(hasItem?(byte)1:(byte)0);
	_buf.put(hasTaken?(byte)1:(byte)0);
	_buf.put(isReadOver?(byte)1:(byte)0);
	_buf.put(isMustRead?(byte)1:(byte)0);
	_buf.putShort((short)contentReplace.Count);
	for(int _i = 0; _i < contentReplace.Count; _i++) { 
		_buf.putString(contentReplace[_i]);
	}
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
	builder.Append("mailUid").Append(":").Append(mailUid.ToString()).Append(", ");
	builder.Append("senderId").Append(":").Append(senderId.ToString()).Append(", ");
	builder.Append("mailRefId").Append(":").Append(mailRefId.ToString()).Append(", ");
	builder.Append("title").Append(":").Append(title.ToString()).Append(", ");
	builder.Append("endTimeSec").Append(":").Append(endTimeSec.ToString()).Append(", ");
	builder.Append("gainTimeSec").Append(":").Append(gainTimeSec.ToString()).Append(", ");
	builder.Append("isLocked").Append(":").Append(isLocked.ToString()).Append(", ");
	builder.Append("isRead").Append(":").Append(isRead.ToString()).Append(", ");
	builder.Append("hasItem").Append(":").Append(hasItem.ToString()).Append(", ");
	builder.Append("hasTaken").Append(":").Append(hasTaken.ToString()).Append(", ");
	builder.Append("isReadOver").Append(":").Append(isReadOver.ToString()).Append(", ");
	builder.Append("isMustRead").Append(":").Append(isMustRead.ToString()).Append(", ");
	builder.Append("contentReplace").Append(":").Append(contentReplace.ToString()).Append(", ");
	builder.Append("exTitleData").Append(":").Append(exTitleData == null ? "null" : exTitleData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

