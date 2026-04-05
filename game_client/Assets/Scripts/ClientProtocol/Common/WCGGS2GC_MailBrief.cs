using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_MailBrief : ALBasicProtocolPack._IALProtocolStructure {
private long sId;
private string title;
private long createTime;
private long readTime;
private long pickUpTime;
private string senderName;
private bool isReward;
private long expireTime;


public WCGGS2GC_MailBrief() {
	sId = (long)0;
	title = "";
	createTime = (long)0;
	readTime = (long)0;
	pickUpTime = (long)0;
	senderName = "";
	isReward = false;
	expireTime = (long)0;
}

public WCGGS2GC_MailBrief(
	long _sId
	, string _title
	, long _createTime
	, long _readTime
	, long _pickUpTime
	, string _senderName
	, bool _isReward
	, long _expireTime
) {	sId = _sId;
	title = _title;
	createTime = _createTime;
	readTime = _readTime;
	pickUpTime = _pickUpTime;
	senderName = _senderName;
	isReward = _isReward;
	expireTime = _expireTime;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getSId() { return sId; }
public void setSId(long _sId) { sId = _sId; }
public string getTitle() { return title; }
public void setTitle(string _title) { title = _title; }
public long getCreateTime() { return createTime; }
public void setCreateTime(long _createTime) { createTime = _createTime; }
public long getReadTime() { return readTime; }
public void setReadTime(long _readTime) { readTime = _readTime; }
public long getPickUpTime() { return pickUpTime; }
public void setPickUpTime(long _pickUpTime) { pickUpTime = _pickUpTime; }
public string getSenderName() { return senderName; }
public void setSenderName(string _senderName) { senderName = _senderName; }
public bool getIsReward() { return isReward; }
public void setIsReward(bool _isReward) { isReward = _isReward; }
public long getExpireTime() { return expireTime; }
public void setExpireTime(long _expireTime) { expireTime = _expireTime; }


public int GetBufSize() {
	int _size = 41;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 43;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	title = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createTime = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	readTime = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pickUpTime = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expireTime = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(sId);
	_buf.putString(title);
	_buf.putLong(createTime);
	_buf.putLong(readTime);
	_buf.putLong(pickUpTime);
	_buf.putString(senderName);
	_buf.put(isReward?(byte)1:(byte)0);
	_buf.putLong(expireTime);
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
	builder.Append("sId").Append(":").Append(sId.ToString()).Append(", ");
	builder.Append("title").Append(":").Append(title.ToString()).Append(", ");
	builder.Append("createTime").Append(":").Append(createTime.ToString()).Append(", ");
	builder.Append("readTime").Append(":").Append(readTime.ToString()).Append(", ");
	builder.Append("pickUpTime").Append(":").Append(pickUpTime.ToString()).Append(", ");
	builder.Append("senderName").Append(":").Append(senderName.ToString()).Append(", ");
	builder.Append("isReward").Append(":").Append(isReward.ToString()).Append(", ");
	builder.Append("expireTime").Append(":").Append(expireTime.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

