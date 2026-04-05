package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_MailInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long id;
private String sender;
private String title;
private String content;
private String keyParams;
private long createTimeMs;
private long readTimeMs;
private long gainTimeMs;
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> reward;


public NPCommon_MailInfo() {
	id = (long)0;
	sender = "";
	title = "";
	content = "";
	keyParams = "";
	createTimeMs = (long)0;
	readTimeMs = (long)0;
	gainTimeMs = (long)0;
	reward = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public NPCommon_MailInfo(
	 long _id
	, String _sender
	, String _title
	, String _content
	, String _keyParams
	, long _createTimeMs
	, long _readTimeMs
	, long _gainTimeMs
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _reward
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public String getSender() { return sender; }
public void setSender(String _sender) { sender = _sender; }
public String getTitle() { return title; }
public void setTitle(String _title) { title = _title; }
public String getContent() { return content; }
public void setContent(String _content) { content = _content; }
public String getKeyParams() { return keyParams; }
public void setKeyParams(String _keyParams) { keyParams = _keyParams; }
public long getCreateTimeMs() { return createTimeMs; }
public void setCreateTimeMs(long _createTimeMs) { createTimeMs = _createTimeMs; }
public long getReadTimeMs() { return readTimeMs; }
public void setReadTimeMs(long _readTimeMs) { readTimeMs = _readTimeMs; }
public long getGainTimeMs() { return gainTimeMs; }
public void setGainTimeMs(long _gainTimeMs) { gainTimeMs = _gainTimeMs; }
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getReward() { return reward; }
public void addReward(NPCommon.NPCommon_ItemInfo _reward) { reward.add(_reward); }


public final int GetBufSize() {
	int _size = 32;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sender);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(keyParams);
	_size += 2;
	for(int _i = 0; _i < reward.size(); _i++) {
	_size += 4 + reward.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sender);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(keyParams);
	_size += 2;
	for(int _i = 0; _i < reward.size(); _i++) {
	_size += 4 + reward.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sender = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) title = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) keyParams = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) readTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rewardCount = _buf.getShort();
	for(int _i = 0; _i < _rewardCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _reward = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __rewardCustLen = _buf.getInt();
	int __rewardCurPos = _buf.position();
	_reward.ReadUnzipBuf(_buf, __rewardCurPos + __rewardCustLen);
	_buf.position(__rewardCurPos + __rewardCustLen);

		reward.add(_reward);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, sender);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, title);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, keyParams);
	_buf.putLong(createTimeMs);
	_buf.putLong(readTimeMs);
	_buf.putLong(gainTimeMs);
	_buf.putShort((short)reward.size());
	for(int _i = 0; _i < reward.size(); _i++) { 
		_buf.putInt(reward.get(_i).GetBufSize());
	reward.get(_i).PutUnzipBuf(_buf);
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

