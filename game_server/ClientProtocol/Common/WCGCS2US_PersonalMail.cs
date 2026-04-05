using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGCS2US_PersonalMail : ALBasicProtocolPack._IALProtocolStructure {
private string senderName;
private string title;
private string content;
private string reward;
private int createTime;
private int existTime;


public WCGCS2US_PersonalMail() {
	senderName = "";
	title = "";
	content = "";
	reward = "";
	createTime = 0;
	existTime = 0;
}

public WCGCS2US_PersonalMail(
	string _senderName
	, string _title
	, string _content
	, string _reward
	, int _createTime
	, int _existTime
) {	senderName = _senderName;
	title = _title;
	content = _content;
	reward = _reward;
	createTime = _createTime;
	existTime = _existTime;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public string getSenderName() { return senderName; }
public void setSenderName(string _senderName) { senderName = _senderName; }
public string getTitle() { return title; }
public void setTitle(string _title) { title = _title; }
public string getContent() { return content; }
public void setContent(string _content) { content = _content; }
public string getReward() { return reward; }
public void setReward(string _reward) { reward = _reward; }
public int getCreateTime() { return createTime; }
public void setCreateTime(int _createTime) { createTime = _createTime; }
public int getExistTime() { return existTime; }
public void setExistTime(int _existTime) { existTime = _existTime; }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	title = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	reward = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	existTime = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(senderName);
	_buf.putString(title);
	_buf.putString(content);
	_buf.putString(reward);
	_buf.putInt(createTime);
	_buf.putInt(existTime);
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
	builder.Append("senderName").Append(":").Append(senderName.ToString()).Append(", ");
	builder.Append("title").Append(":").Append(title.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("reward").Append(":").Append(reward.ToString()).Append(", ");
	builder.Append("createTime").Append(":").Append(createTime.ToString()).Append(", ");
	builder.Append("existTime").Append(":").Append(existTime.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

