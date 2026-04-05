using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p040_MarsPeopleOp
{

/// <summary>
/// 火星居民信件变更
/// </summary>
public class GS2GC_040_053_OnLetterChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.MarsObj.Mars_Letter letter;


public GS2GC_040_053_OnLetterChg() {
	letter = new Common.MarsObj.Mars_Letter();
}

public GS2GC_040_053_OnLetterChg(
	Common.MarsObj.Mars_Letter _letter
) {	letter = _letter;
}

public byte getMainOrder() { return (byte)40; }

public byte getSubOrder() { return (byte)53; }

public Common.MarsObj.Mars_Letter getLetter() { return letter; }
public void setLetter(Common.MarsObj.Mars_Letter _letter) { letter = _letter; }


public int GetBufSize() {
	int _size = 29;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 31;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _letterCustLen = _buf.getInt();
	int _letterCurPos = _buf.getCurPos();
	letter.ReadUnzipBuf(_buf, _letterCurPos + _letterCustLen);
	_buf.setPosition(_letterCurPos + _letterCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(letter.GetBufSize());
	letter.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)53);
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
	builder.Append("letter").Append(":").Append(letter == null ? "null" : letter.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

