using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace RemarkData
{

public class PlayerUpgradeData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已经展示过升级弹窗的等级
/// </summary>
private long alreadyShowUpgradeWndLevel;


public PlayerUpgradeData() {
	alreadyShowUpgradeWndLevel = (long)0;
}

public PlayerUpgradeData(
	long _alreadyShowUpgradeWndLevel
) {	alreadyShowUpgradeWndLevel = _alreadyShowUpgradeWndLevel;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 已经展示过升级弹窗的等级
/// </summary>
public long getAlreadyShowUpgradeWndLevel() { return alreadyShowUpgradeWndLevel; }
/// <summary>
/// 已经展示过升级弹窗的等级
/// </summary>
public void setAlreadyShowUpgradeWndLevel(long _alreadyShowUpgradeWndLevel) { alreadyShowUpgradeWndLevel = _alreadyShowUpgradeWndLevel; }


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
	alreadyShowUpgradeWndLevel = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(alreadyShowUpgradeWndLevel);
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
	builder.Append("alreadyShowUpgradeWndLevel").Append(":").Append(alreadyShowUpgradeWndLevel.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

