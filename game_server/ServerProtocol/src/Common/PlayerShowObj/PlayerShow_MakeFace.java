package Common.PlayerShowObj;

import java.nio.ByteBuffer;
/*********
 * 玩家捏脸展示信息
 **/
public class PlayerShow_MakeFace implements ALBasicProtocolPack._IALProtocolStructure {
private long colorId;
private java.util.ArrayList<Long> faceList;
private java.util.ArrayList<Long> avatarList;


public PlayerShow_MakeFace() {
	colorId = (long)0;
	faceList = new java.util.ArrayList<Long>();
	avatarList = new java.util.ArrayList<Long>();
}

public PlayerShow_MakeFace(
	 long _colorId
	, java.util.ArrayList<Long> _faceList
	, java.util.ArrayList<Long> _avatarList
) {	colorId = _colorId;
	faceList = _faceList;
	avatarList = _avatarList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getColorId() { return colorId; }
public void setColorId(long _colorId) { colorId = _colorId; }
public java.util.ArrayList<Long> getFaceList() { return faceList; }
public void addFaceList(long _faceList) { faceList.add(_faceList); }
public java.util.ArrayList<Long> getAvatarList() { return avatarList; }
public void addAvatarList(long _avatarList) { avatarList.add(_avatarList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (faceList.size() * 8);
	_size += 2 + (avatarList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (faceList.size() * 8);
	_size += 2 + (avatarList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) colorId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _faceListCount = _buf.getShort();
	for(int _i = 0; _i < _faceListCount; _i++) { 
		long _faceList = (long)0;
		if(_buf.remaining() > 0) _faceList = _buf.getLong();
		faceList.add(_faceList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _avatarListCount = _buf.getShort();
	for(int _i = 0; _i < _avatarListCount; _i++) { 
		long _avatarList = (long)0;
		if(_buf.remaining() > 0) _avatarList = _buf.getLong();
		avatarList.add(_avatarList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(colorId);
	_buf.putShort((short)faceList.size());
	for(int _i = 0; _i < faceList.size(); _i++) { 
		_buf.putLong(faceList.get(_i));
	}
	_buf.putShort((short)avatarList.size());
	for(int _i = 0; _i < avatarList.size(); _i++) { 
		_buf.putLong(avatarList.get(_i));
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

