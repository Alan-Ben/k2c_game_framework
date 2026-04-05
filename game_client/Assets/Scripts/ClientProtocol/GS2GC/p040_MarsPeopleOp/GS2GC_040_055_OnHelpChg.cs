using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p040_MarsPeopleOp
{

/// <summary>
/// 求助变更
/// </summary>
public class GS2GC_040_055_OnHelpChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.MarsObj.Mars_Help help;


public GS2GC_040_055_OnHelpChg() {
	help = new Common.MarsObj.Mars_Help();
}

public GS2GC_040_055_OnHelpChg(
	Common.MarsObj.Mars_Help _help
) {	help = _help;
}

public byte getMainOrder() { return (byte)40; }

public byte getSubOrder() { return (byte)55; }

public Common.MarsObj.Mars_Help getHelp() { return help; }
public void setHelp(Common.MarsObj.Mars_Help _help) { help = _help; }


public int GetBufSize() {
	int _size = 33;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 35;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _helpCustLen = _buf.getInt();
	int _helpCurPos = _buf.getCurPos();
	help.ReadUnzipBuf(_buf, _helpCurPos + _helpCustLen);
	_buf.setPosition(_helpCurPos + _helpCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(help.GetBufSize());
	help.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)55);
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
	builder.Append("help").Append(":").Append(help == null ? "null" : help.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

