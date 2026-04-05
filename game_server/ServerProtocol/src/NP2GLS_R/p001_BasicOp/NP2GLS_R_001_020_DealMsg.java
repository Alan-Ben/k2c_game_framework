package NP2GLS_R.p001_BasicOp;

import java.nio.ByteBuffer;
/*********
 * GLS服务器转发消息
 **/
public class NP2GLS_R_001_020_DealMsg implements ALBasicProtocolPack._IALProtocolStructure {
/** 游戏逻辑主体实例ID */
private long instanceId;
private int usId;
private long cid;
private long playerGroupId;
/** 具体的消息内容 */
private byte[] msg;
/** 附加的信息 */
private byte[] addInfo;


public NP2GLS_R_001_020_DealMsg() {
	instanceId = (long)0;
	usId = 0;
	cid = (long)0;
	playerGroupId = (long)0;
	msg = null;
	addInfo = null;
}

public NP2GLS_R_001_020_DealMsg(
	 long _instanceId
	, int _usId
	, long _cid
	, long _playerGroupId
	, byte[] _msg
	, byte[] _addInfo
) {	instanceId = _instanceId;
	usId = _usId;
	cid = _cid;
	playerGroupId = _playerGroupId;
	msg = _msg;
	addInfo = _addInfo;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)20; }

/** 游戏逻辑主体实例ID */
public long getInstanceId() { return instanceId; }
/** 游戏逻辑主体实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
public int getUsId() { return usId; }
public void setUsId(int _usId) { usId = _usId; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getPlayerGroupId() { return playerGroupId; }
public void setPlayerGroupId(long _playerGroupId) { playerGroupId = _playerGroupId; }
/** 具体的消息内容 */
public byte[] getMsg() { return msg; }
public java.nio.ByteBuffer get_buffer_Msg() { if(null == msg)return null; else return ByteBuffer.wrap(msg); }

/** 具体的消息内容 */
public void setMsg(byte[] _msg) { msg = _msg; }
public void setMsg(java.nio.ByteBuffer _msg) 
{
	if(null == _msg){return;}
	int _oldPos = _msg.position();
	int _bufLength = _msg.remaining();
	msg = new byte[_bufLength];
	_msg.get(msg);
	_msg.position(_oldPos);
}

/** 附加的信息 */
public byte[] getAddInfo() { return addInfo; }
public java.nio.ByteBuffer get_buffer_AddInfo() { if(null == addInfo)return null; else return ByteBuffer.wrap(addInfo); }

/** 附加的信息 */
public void setAddInfo(byte[] _addInfo) { addInfo = _addInfo; }
public void setAddInfo(java.nio.ByteBuffer _addInfo) 
{
	if(null == _addInfo){return;}
	int _oldPos = _addInfo.position();
	int _bufLength = _addInfo.remaining();
	addInfo = new byte[_bufLength];
	_addInfo.get(addInfo);
	_addInfo.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 28;
	_size += 4 + (msg == null ? 0 : msg.length);
	_size += 4 + (addInfo == null ? 0 : addInfo.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += 4 + (msg == null ? 0 : msg.length);
	_size += 4 + (addInfo == null ? 0 : addInfo.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerGroupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _msgCount = _buf.getInt();
	if(0 < _msgCount){
		msg = new byte[_msgCount];
		_buf.get(msg);
	}

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _addInfoCount = _buf.getInt();
	if(0 < _addInfoCount){
		addInfo = new byte[_addInfoCount];
		_buf.get(addInfo);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(usId);
	_buf.putLong(cid);
	_buf.putLong(playerGroupId);
	_buf.putInt((msg == null ? 0 : msg.length));
	if(null != msg){_buf.put(msg);}

	_buf.putInt((addInfo == null ? 0 : addInfo.length));
	if(null != addInfo){_buf.put(addInfo);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)20);
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

