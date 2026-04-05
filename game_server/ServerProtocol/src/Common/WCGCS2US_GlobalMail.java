package Common;

import java.nio.ByteBuffer;
public class WCGCS2US_GlobalMail implements ALBasicProtocolPack._IALProtocolStructure {
private long gMailSID;
private String senderName;
private String title;
private String content;
private String reward;
private int createTime;
private int existTime;
private int sendType;
private java.util.ArrayList<String> channelList;
private boolean isSend;


public WCGCS2US_GlobalMail() {
	gMailSID = (long)0;
	senderName = "";
	title = "";
	content = "";
	reward = "";
	createTime = 0;
	existTime = 0;
	sendType = 0;
	channelList = new java.util.ArrayList<String>();
	isSend = false;
}

public WCGCS2US_GlobalMail(
	 long _gMailSID
	, String _senderName
	, String _title
	, String _content
	, String _reward
	, int _createTime
	, int _existTime
	, int _sendType
	, java.util.ArrayList<String> _channelList
	, boolean _isSend
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getGMailSID() { return gMailSID; }
public void setGMailSID(long _gMailSID) { gMailSID = _gMailSID; }
public String getSenderName() { return senderName; }
public void setSenderName(String _senderName) { senderName = _senderName; }
public String getTitle() { return title; }
public void setTitle(String _title) { title = _title; }
public String getContent() { return content; }
public void setContent(String _content) { content = _content; }
public String getReward() { return reward; }
public void setReward(String _reward) { reward = _reward; }
public int getCreateTime() { return createTime; }
public void setCreateTime(int _createTime) { createTime = _createTime; }
public int getExistTime() { return existTime; }
public void setExistTime(int _existTime) { existTime = _existTime; }
public int getSendType() { return sendType; }
public void setSendType(int _sendType) { sendType = _sendType; }
public java.util.ArrayList<String> getChannelList() { return channelList; }
public void addChannelList(String _channelList) { channelList.add(_channelList); }
public boolean getIsSend() { return isSend; }
public void setIsSend(boolean _isSend) { isSend = _isSend; }


public final int GetBufSize() {
	int _size = 21;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward);
	_size += 2;
	for(int _i = 0; _i < channelList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelList.get(_i));
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward);
	_size += 2;
	for(int _i = 0; _i < channelList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelList.get(_i));
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gMailSID = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) senderName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) title = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) reward = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) existTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sendType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _channelListCount = _buf.getShort();
	for(int _i = 0; _i < _channelListCount; _i++) { 
		String _channelList = "";
		if(_buf.remaining() > 0) _channelList = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		channelList.add(_channelList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSend = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(gMailSID);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, senderName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, title);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, reward);
	_buf.putInt(createTime);
	_buf.putInt(existTime);
	_buf.putInt(sendType);
	_buf.putShort((short)channelList.size());
	for(int _i = 0; _i < channelList.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, channelList.get(_i));
	}
	_buf.put(isSend?(byte)1:(byte)0);
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

