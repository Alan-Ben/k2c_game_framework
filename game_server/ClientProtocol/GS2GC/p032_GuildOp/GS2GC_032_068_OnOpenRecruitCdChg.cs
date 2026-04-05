using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_068_OnOpenRecruitCdChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 下次可招募时间
/// </summary>
private long nextCanRecruitTimeMs;


public GS2GC_032_068_OnOpenRecruitCdChg() {
	nextCanRecruitTimeMs = (long)0;
}

public GS2GC_032_068_OnOpenRecruitCdChg(
	long _nextCanRecruitTimeMs
) {	nextCanRecruitTimeMs = _nextCanRecruitTimeMs;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)68; }

/// <summary>
/// 下次可招募时间
/// </summary>
public long getNextCanRecruitTimeMs() { return nextCanRecruitTimeMs; }
/// <summary>
/// 下次可招募时间
/// </summary>
public void setNextCanRecruitTimeMs(long _nextCanRecruitTimeMs) { nextCanRecruitTimeMs = _nextCanRecruitTimeMs; }


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
	nextCanRecruitTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(nextCanRecruitTimeMs);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)68);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)68);
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
	builder.Append("nextCanRecruitTimeMs").Append(":").Append(nextCanRecruitTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

