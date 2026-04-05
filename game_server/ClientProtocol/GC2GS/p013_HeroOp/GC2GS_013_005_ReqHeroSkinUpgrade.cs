using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p013_HeroOp
{

/// <summary>
/// 提升大臣皮肤等级
/// </summary>
public class GC2GS_013_005_ReqHeroSkinUpgrade : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 皮肤id
/// </summary>
private long skinId;


public GC2GS_013_005_ReqHeroSkinUpgrade() {
	skinId = (long)0;
}

public GC2GS_013_005_ReqHeroSkinUpgrade(
	long _skinId
) {	skinId = _skinId;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 皮肤id
/// </summary>
public long getSkinId() { return skinId; }
/// <summary>
/// 皮肤id
/// </summary>
public void setSkinId(long _skinId) { skinId = _skinId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skinId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(skinId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)5);
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
	builder.Append("skinId").Append(":").Append(skinId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

