package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 当前穿戴称号
 **/
public class PlayerInfo_CurTitle implements ALBasicProtocolPack._IALProtocolStructure {
/** 称号类型 */
private NPEnum.ENPPlayerTitleType type;
/** 称号信息 */
private byte[] info;


public PlayerInfo_CurTitle() {
	type = NPEnum.ENPPlayerTitleType.values()[0];
	info = null;
}

public PlayerInfo_CurTitle(
	 NPEnum.ENPPlayerTitleType _type
	, byte[] _info
) {	type = _type;
	info = _info;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 称号类型 */
public NPEnum.ENPPlayerTitleType getType() { return type; }
/** 称号类型 */
public void setType(NPEnum.ENPPlayerTitleType _type) { type = _type; }
/** 称号信息 */
public byte[] getInfo() { return info; }
public java.nio.ByteBuffer get_buffer_Info() { if(null == info)return null; else return ByteBuffer.wrap(info); }

/** 称号信息 */
public void setInfo(byte[] _info) { info = _info; }
public void setInfo(java.nio.ByteBuffer _info) 
{
	if(null == _info){return;}
	int _oldPos = _info.position();
	int _bufLength = _info.remaining();
	info = new byte[_bufLength];
	_info.get(info);
	_info.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 4;
	_size += 4 + (info == null ? 0 : info.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + (info == null ? 0 : info.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = NPEnum.ENPPlayerTitleType.ENPPlayerTitleType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCount = _buf.getInt();
	if(0 < _infoCount){
		info = new byte[_infoCount];
		_buf.get(info);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putInt((info == null ? 0 : info.length));
	if(null != info){_buf.put(info);}

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

