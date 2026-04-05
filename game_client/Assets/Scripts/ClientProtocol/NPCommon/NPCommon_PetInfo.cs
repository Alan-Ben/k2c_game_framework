using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 宠物通用数据
/// </summary>
public class NPCommon_PetInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宠物配置ID
/// </summary>
private long petRefId;
/// <summary>
/// 宠物等级
/// </summary>
private int petLevel;
/// <summary>
/// 宠物皮肤
/// </summary>
private long petSkin;


public NPCommon_PetInfo() {
	petRefId = (long)0;
	petLevel = 0;
	petSkin = (long)0;
}

public NPCommon_PetInfo(
	long _petRefId
	, int _petLevel
	, long _petSkin
) {	petRefId = _petRefId;
	petLevel = _petLevel;
	petSkin = _petSkin;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 宠物配置ID
/// </summary>
public long getPetRefId() { return petRefId; }
/// <summary>
/// 宠物配置ID
/// </summary>
public void setPetRefId(long _petRefId) { petRefId = _petRefId; }
/// <summary>
/// 宠物等级
/// </summary>
public int getPetLevel() { return petLevel; }
/// <summary>
/// 宠物等级
/// </summary>
public void setPetLevel(int _petLevel) { petLevel = _petLevel; }
/// <summary>
/// 宠物皮肤
/// </summary>
public long getPetSkin() { return petSkin; }
/// <summary>
/// 宠物皮肤
/// </summary>
public void setPetSkin(long _petSkin) { petSkin = _petSkin; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	petRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	petLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	petSkin = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(petRefId);
	_buf.putInt(petLevel);
	_buf.putLong(petSkin);
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
	builder.Append("petRefId").Append(":").Append(petRefId.ToString()).Append(", ");
	builder.Append("petLevel").Append(":").Append(petLevel.ToString()).Append(", ");
	builder.Append("petSkin").Append(":").Append(petSkin.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

