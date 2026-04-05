using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_VoiceSetting : ALBasicProtocolPack._IALProtocolStructure {
private bool isSpeakerOn;
private bool isMicroOn;


public Common_VoiceSetting() {
	isSpeakerOn = false;
	isMicroOn = false;
}

public Common_VoiceSetting(
	bool _isSpeakerOn
	, bool _isMicroOn
) {	isSpeakerOn = _isSpeakerOn;
	isMicroOn = _isMicroOn;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public bool getIsSpeakerOn() { return isSpeakerOn; }
public void setIsSpeakerOn(bool _isSpeakerOn) { isSpeakerOn = _isSpeakerOn; }
public bool getIsMicroOn() { return isMicroOn; }
public void setIsMicroOn(bool _isMicroOn) { isMicroOn = _isMicroOn; }


public int GetBufSize() {
	int _size = 2;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 4;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isSpeakerOn = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isMicroOn = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isSpeakerOn?(byte)1:(byte)0);
	_buf.put(isMicroOn?(byte)1:(byte)0);
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
	builder.Append("isSpeakerOn").Append(":").Append(isSpeakerOn.ToString()).Append(", ");
	builder.Append("isMicroOn").Append(":").Append(isMicroOn.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

