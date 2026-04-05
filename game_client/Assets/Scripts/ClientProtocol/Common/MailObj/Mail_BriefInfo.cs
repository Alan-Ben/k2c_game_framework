using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MailObj
{

/// <summary>
/// 邮件统简单信息
/// </summary>
public class Mail_BriefInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件唯一id
/// </summary>
private long mailUid;
/// <summary>
/// 获得邮件时间，秒
/// </summary>
private int gainTimeSec;
/// <summary>
/// 是否已读
/// </summary>
private bool isRead;
/// <summary>
/// 是否已领取物品
/// </summary>
private bool hasTaken;
/// <summary>
/// 首个附件物品
/// </summary>
private NPCommon.NPCommon_ItemInfo item;
/// <summary>
/// 是否必读
/// </summary>
private bool isMustRead;
/// <summary>
/// 是否收藏
/// </summary>
private bool isLocked;
/// <summary>
/// 邮件配表id
/// </summary>
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
	, bool _isRead
	, bool _hasTaken
	, NPCommon.NPCommon_ItemInfo _item
	, bool _isMustRead
	, bool _isLocked
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
/// 获得邮件时间，秒
/// </summary>
public int getGainTimeSec() { return gainTimeSec; }
/// <summary>
/// 获得邮件时间，秒
/// </summary>
public void setGainTimeSec(int _gainTimeSec) { gainTimeSec = _gainTimeSec; }
/// <summary>
/// 是否已读
/// </summary>
public bool getIsRead() { return isRead; }
/// <summary>
/// 是否已读
/// </summary>
public void setIsRead(bool _isRead) { isRead = _isRead; }
/// <summary>
/// 是否已领取物品
/// </summary>
public bool getHasTaken() { return hasTaken; }
/// <summary>
/// 是否已领取物品
/// </summary>
public void setHasTaken(bool _hasTaken) { hasTaken = _hasTaken; }
/// <summary>
/// 首个附件物品
/// </summary>
public NPCommon.NPCommon_ItemInfo getItem() { return item; }
/// <summary>
/// 首个附件物品
/// </summary>
public void setItem(NPCommon.NPCommon_ItemInfo _item) { item = _item; }
/// <summary>
/// 是否必读
/// </summary>
public bool getIsMustRead() { return isMustRead; }
/// <summary>
/// 是否必读
/// </summary>
public void setIsMustRead(bool _isMustRead) { isMustRead = _isMustRead; }
/// <summary>
/// 是否收藏
/// </summary>
public bool getIsLocked() { return isLocked; }
/// <summary>
/// 是否收藏
/// </summary>
public void setIsLocked(bool _isLocked) { isLocked = _isLocked; }
/// <summary>
/// 邮件配表id
/// </summary>
public long getMailRefId() { return mailRefId; }
/// <summary>
/// 邮件配表id
/// </summary>
public void setMailRefId(long _mailRefId) { mailRefId = _mailRefId; }


public int GetBufSize() {
	int _size = 24;
	_size += 4 + item.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;
	_size += 4 + item.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasTaken = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _itemCustLen = _buf.getInt();
	int _itemCurPos = _buf.getCurPos();
	item.ReadUnzipBuf(_buf, _itemCurPos + _itemCustLen);
	_buf.setPosition(_itemCurPos + _itemCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isMustRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isLocked = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mailRefId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
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
	builder.Append("gainTimeSec").Append(":").Append(gainTimeSec.ToString()).Append(", ");
	builder.Append("isRead").Append(":").Append(isRead.ToString()).Append(", ");
	builder.Append("hasTaken").Append(":").Append(hasTaken.ToString()).Append(", ");
	builder.Append("item").Append(":").Append(item == null ? "null" : item.ToString()).Append(", ");
	builder.Append("isMustRead").Append(":").Append(isMustRead.ToString()).Append(", ");
	builder.Append("isLocked").Append(":").Append(isLocked.ToString()).Append(", ");
	builder.Append("mailRefId").Append(":").Append(mailRefId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

