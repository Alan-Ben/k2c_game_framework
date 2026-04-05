using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_MailInfo : ALBasicProtocolPack._IALProtocolStructure {
private long id;
private string sender;
private string title;
private string content;
private string keyParams;
private long createTimeMs;
private long readTimeMs;
private long gainTimeMs;
private List<NPCommon.NPCommon_ItemInfo> reward;


public NPCommon_MailInfo() {
	id = (long)0;
	sender = "";
	title = "";
	content = "";
	keyParams = "";
	createTimeMs = (long)0;
	readTimeMs = (long)0;
	gainTimeMs = (long)0;
	reward = new List<NPCommon.NPCommon_ItemInfo>();
}

public NPCommon_MailInfo(
	long _id
	, string _sender
	, string _title
	, string _content
	, string _keyParams
	, long _createTimeMs
	, long _readTimeMs
	, long _gainTimeMs
	, List<NPCommon.NPCommon_ItemInfo> _reward
) {	id = _id;
	sender = _sender;
	title = _title;
	content = _content;
	keyParams = _keyParams;
	createTimeMs = _createTimeMs;
	readTimeMs = _readTimeMs;
	gainTimeMs = _gainTimeMs;
	reward = _reward;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public string getSender() { return sender; }
public void setSender(string _sender) { sender = _sender; }
public string getTitle() { return title; }
public void setTitle(string _title) { title = _title; }
public string getContent() { return content; }
public void setContent(string _content) { content = _content; }
public string getKeyParams() { return keyParams; }
public void setKeyParams(string _keyParams) { keyParams = _keyParams; }
public long getCreateTimeMs() { return createTimeMs; }
public void setCreateTimeMs(long _createTimeMs) { createTimeMs = _createTimeMs; }
public long getReadTimeMs() { return readTimeMs; }
public void setReadTimeMs(long _readTimeMs) { readTimeMs = _readTimeMs; }
public long getGainTimeMs() { return gainTimeMs; }
public void setGainTimeMs(long _gainTimeMs) { gainTimeMs = _gainTimeMs; }
public List<NPCommon.NPCommon_ItemInfo> getReward() { return reward; }
public void addReward(NPCommon.NPCommon_ItemInfo _reward) { reward.Add(_reward); }


public int GetBufSize() {
	int _size = 32;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sender);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(keyParams);
	_size += 2;
for(int _i = 0; _i < reward.Count; _i++) {
	_size += 4 + reward[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sender);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(keyParams);
	_size += 2;
for(int _i = 0; _i < reward.Count; _i++) {
	_size += 4 + reward[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sender = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	title = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	keyParams = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	readTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rewardCount = _buf.getShort();
	for(int _i = 0; _i < _rewardCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _reward = new NPCommon.NPCommon_ItemInfo();
		int __rewardCustLen = _buf.getInt();
	int __rewardCurPos = _buf.getCurPos();
	_reward.ReadUnzipBuf(_buf, __rewardCurPos + __rewardCustLen);
	_buf.setPosition(__rewardCurPos + __rewardCustLen);

		reward.Add(_reward);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putString(sender);
	_buf.putString(title);
	_buf.putString(content);
	_buf.putString(keyParams);
	_buf.putLong(createTimeMs);
	_buf.putLong(readTimeMs);
	_buf.putLong(gainTimeMs);
	_buf.putShort((short)reward.Count);
	for(int _i = 0; _i < reward.Count; _i++) { 
		_buf.putInt(reward[_i].GetBufSize());
	reward[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("sender").Append(":").Append(sender.ToString()).Append(", ");
	builder.Append("title").Append(":").Append(title.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("keyParams").Append(":").Append(keyParams.ToString()).Append(", ");
	builder.Append("createTimeMs").Append(":").Append(createTimeMs.ToString()).Append(", ");
	builder.Append("readTimeMs").Append(":").Append(readTimeMs.ToString()).Append(", ");
	builder.Append("gainTimeMs").Append(":").Append(gainTimeMs.ToString()).Append(", ");
	builder.Append("reward").Append(":").Append(reward.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

