using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.PlayerShowObj
{

/// <summary>
/// 玩家捏脸展示信息
/// </summary>
public class PlayerShow_MakeFace : ALBasicProtocolPack._IALProtocolStructure {
private long colorId;
private List<long> faceList;
private List<long> avatarList;


public PlayerShow_MakeFace() {
	colorId = (long)0;
	faceList = new List<long>();
	avatarList = new List<long>();
}

public PlayerShow_MakeFace(
	long _colorId
	, List<long> _faceList
	, List<long> _avatarList
) {	colorId = _colorId;
	faceList = _faceList;
	avatarList = _avatarList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getColorId() { return colorId; }
public void setColorId(long _colorId) { colorId = _colorId; }
public List<long> getFaceList() { return faceList; }
public void addFaceList(long _faceList) { faceList.Add(_faceList); }
public List<long> getAvatarList() { return avatarList; }
public void addAvatarList(long _avatarList) { avatarList.Add(_avatarList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (faceList.Count * 8);
	_size += 2 + (avatarList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (faceList.Count * 8);
	_size += 2 + (avatarList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	colorId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _faceListCount = _buf.getShort();
	for(int _i = 0; _i < _faceListCount; _i++) { 
		long _faceList = (long)0;
		_faceList = _buf.getLong();
		faceList.Add(_faceList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _avatarListCount = _buf.getShort();
	for(int _i = 0; _i < _avatarListCount; _i++) { 
		long _avatarList = (long)0;
		_avatarList = _buf.getLong();
		avatarList.Add(_avatarList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(colorId);
	_buf.putShort((short)faceList.Count);
	for(int _i = 0; _i < faceList.Count; _i++) { 
		_buf.putLong(faceList[_i]);
	}
	_buf.putShort((short)avatarList.Count);
	for(int _i = 0; _i < avatarList.Count; _i++) { 
		_buf.putLong(avatarList[_i]);
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
	builder.Append("colorId").Append(":").Append(colorId.ToString()).Append(", ");
	builder.Append("faceList").Append(":").Append(faceList.ToString()).Append(", ");
	builder.Append("avatarList").Append(":").Append(avatarList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

