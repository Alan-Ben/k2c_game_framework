using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p019_DinnerOp
{

/// <summary>
/// 检查宴会邀请信息
/// </summary>
public class GS2GC_019_014_RetCheckDinnerInvite : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 参与宴会玩家数量
/// </summary>
private int joinerCount;


public GS2GC_019_014_RetCheckDinnerInvite() {
	joinerCount = 0;
}

public GS2GC_019_014_RetCheckDinnerInvite(
	int _joinerCount
) {	joinerCount = _joinerCount;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)14; }

/// <summary>
/// 参与宴会玩家数量
/// </summary>
public int getJoinerCount() { return joinerCount; }
/// <summary>
/// 参与宴会玩家数量
/// </summary>
public void setJoinerCount(int _joinerCount) { joinerCount = _joinerCount; }


public int GetBufSize() {
	int _size = 4;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinerCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(joinerCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)14);
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
	builder.Append("joinerCount").Append(":").Append(joinerCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

