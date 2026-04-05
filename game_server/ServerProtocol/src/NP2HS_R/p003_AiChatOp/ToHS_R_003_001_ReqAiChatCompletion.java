package NP2HS_R.p003_AiChatOp;

import java.nio.ByteBuffer;
public class ToHS_R_003_001_ReqAiChatCompletion implements ALBasicProtocolPack._IALProtocolStructure {
private String uid;
private long cid;
private String role;
private int level;
private int vipLevel;
/** us请求序列号 */
private long usRequestSerial;
private int usId;
/** 角色代码 */
private String charCode;
/** 消息列表 */
private java.util.ArrayList<Common.Common_AiChatMessage> msgList;


public ToHS_R_003_001_ReqAiChatCompletion() {
	uid = "";
	cid = (long)0;
	role = "";
	level = 0;
	vipLevel = 0;
	usRequestSerial = (long)0;
	usId = 0;
	charCode = "";
	msgList = new java.util.ArrayList<Common.Common_AiChatMessage>();
}

public ToHS_R_003_001_ReqAiChatCompletion(
	 String _uid
	, long _cid
	, String _role
	, int _level
	, int _vipLevel
	, long _usRequestSerial
	, int _usId
	, String _charCode
	, java.util.ArrayList<Common.Common_AiChatMessage> _msgList
) {	uid = _uid;
	cid = _cid;
	role = _role;
	level = _level;
	vipLevel = _vipLevel;
	usRequestSerial = _usRequestSerial;
	usId = _usId;
	charCode = _charCode;
	msgList = _msgList;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)1; }

public String getUid() { return uid; }
public void setUid(String _uid) { uid = _uid; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public String getRole() { return role; }
public void setRole(String _role) { role = _role; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public int getVipLevel() { return vipLevel; }
public void setVipLevel(int _vipLevel) { vipLevel = _vipLevel; }
/** us请求序列号 */
public long getUsRequestSerial() { return usRequestSerial; }
/** us请求序列号 */
public void setUsRequestSerial(long _usRequestSerial) { usRequestSerial = _usRequestSerial; }
public int getUsId() { return usId; }
public void setUsId(int _usId) { usId = _usId; }
/** 角色代码 */
public String getCharCode() { return charCode; }
/** 角色代码 */
public void setCharCode(String _charCode) { charCode = _charCode; }
/** 消息列表 */
public java.util.ArrayList<Common.Common_AiChatMessage> getMsgList() { return msgList; }
/** 消息列表 */
public void addMsgList(Common.Common_AiChatMessage _msgList) { msgList.add(_msgList); }


public final int GetBufSize() {
	int _size = 28;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(role);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(charCode);
	_size += 2;
	for(int _i = 0; _i < msgList.size(); _i++) {
	_size += 4 + msgList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(role);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(charCode);
	_size += 2;
	for(int _i = 0; _i < msgList.size(); _i++) {
	_size += 4 + msgList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) role = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) vipLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usRequestSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) charCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _msgListCount = _buf.getShort();
	for(int _i = 0; _i < _msgListCount; _i++) { 
		Common.Common_AiChatMessage _msgList = new Common.Common_AiChatMessage();
		if(_buf.remaining() <= 0) return;
	int __msgListCustLen = _buf.getInt();
	int __msgListCurPos = _buf.position();
	_msgList.ReadUnzipBuf(_buf, __msgListCurPos + __msgListCustLen);
	_buf.position(__msgListCurPos + __msgListCustLen);

		msgList.add(_msgList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, uid);
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, role);
	_buf.putInt(level);
	_buf.putInt(vipLevel);
	_buf.putLong(usRequestSerial);
	_buf.putInt(usId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, charCode);
	_buf.putShort((short)msgList.size());
	for(int _i = 0; _i < msgList.size(); _i++) { 
		_buf.putInt(msgList.get(_i).GetBufSize());
	msgList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)1);
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

