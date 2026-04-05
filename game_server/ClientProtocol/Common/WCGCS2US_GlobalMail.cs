using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGCS2US_GlobalMail : ALBasicProtocolPack._IALProtocolStructure {
private long gMailSID;
private string senderName;
private string title;
private string content;
private string reward;
private int createTime;
private int existTime;
private int sendType;
private List<string> channelList;
private bool isSend;


public WCGCS2US_GlobalMail() {
	gMailSID = (long)0;
	senderName = "";
	title = "";
	content = "";
	reward = "";
	createTime = 0;
	existTime = 0;
	sendType = 0;
	channelList = new List<string>();
	isSend = false;
}

public WCGCS2US_GlobalMail(
	long _gMailSID
	, string _senderName
	, string _title
	, string _content
	, string _reward
	, int _createTime
	, int _existTime
	, int _sendType
	, List<string> _channelList
	, bool _isSend
) {	gMailSID = _gMailSID;
	senderName = _senderName;
	title = _title;
	content = _content;
	reward = _reward;
	createTime = _createTime;
	existTime = _existTime;
	sendType = _sendType;
	channelList = _channelList;
	isSend = _isSend;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getGMailSID() { return gMailSID; }
public void setGMailSID(long _gMailSID) { gMailSID = _gMailSID; }
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
public int getSendType() { return sendType; }
public void setSendType(int _sendType) { sendType = _sendType; }
public List<string> getChannelList() { return channelList; }
public void addChannelList(string _channelList) { channelList.Add(_channelList); }
public bool getIsSend() { return isSend; }
public void setIsSend(bool _isSend) { isSend = _isSend; }


public int GetBufSize() {
	int _size = 21;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward);
	_size += 2;
for(int _i = 0; _i < channelList.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelList[_i]);
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 23;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward);
	_size += 2;
for(int _i = 0; _i < channelList.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelList[_i]);
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gMailSID = _buf.getLong();
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
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sendType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _channelListCount = _buf.getShort();
	for(int _i = 0; _i < _channelListCount; _i++) { 
		string _channelList = "";
		_channelList = _buf.getString();
		channelList.Add(_channelList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isSend = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(gMailSID);
	_buf.putString(senderName);
	_buf.putString(title);
	_buf.putString(content);
	_buf.putString(reward);
	_buf.putInt(createTime);
	_buf.putInt(existTime);
	_buf.putInt(sendType);
	_buf.putShort((short)channelList.Count);
	for(int _i = 0; _i < channelList.Count; _i++) { 
		_buf.putString(channelList[_i]);
	}
	_buf.put(isSend?(byte)1:(byte)0);
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
	builder.Append("gMailSID").Append(":").Append(gMailSID.ToString()).Append(", ");
	builder.Append("senderName").Append(":").Append(senderName.ToString()).Append(", ");
	builder.Append("title").Append(":").Append(title.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("reward").Append(":").Append(reward.ToString()).Append(", ");
	builder.Append("createTime").Append(":").Append(createTime.ToString()).Append(", ");
	builder.Append("existTime").Append(":").Append(existTime.ToString()).Append(", ");
	builder.Append("sendType").Append(":").Append(sendType.ToString()).Append(", ");
	builder.Append("channelList").Append(":").Append(channelList.ToString()).Append(", ");
	builder.Append("isSend").Append(":").Append(isSend.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

