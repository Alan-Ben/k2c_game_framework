using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p041_MarsExploreOp
{

public class GS2GC_041_024_RetGuildShareMineHadAttackByOthersTag : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否被其他人攻击标识
/// </summary>
private bool hadAttackByOthersTag;


public GS2GC_041_024_RetGuildShareMineHadAttackByOthersTag() {
	hadAttackByOthersTag = false;
}

public GS2GC_041_024_RetGuildShareMineHadAttackByOthersTag(
	bool _hadAttackByOthersTag
) {	hadAttackByOthersTag = _hadAttackByOthersTag;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)24; }

/// <summary>
/// 是否被其他人攻击标识
/// </summary>
public bool getHadAttackByOthersTag() { return hadAttackByOthersTag; }
/// <summary>
/// 是否被其他人攻击标识
/// </summary>
public void setHadAttackByOthersTag(bool _hadAttackByOthersTag) { hadAttackByOthersTag = _hadAttackByOthersTag; }


public int GetBufSize() {
	int _size = 1;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadAttackByOthersTag = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(hadAttackByOthersTag?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)24);
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
	builder.Append("hadAttackByOthersTag").Append(":").Append(hadAttackByOthersTag.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

