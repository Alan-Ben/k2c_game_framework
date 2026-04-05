using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_SummonInfo : ALBasicProtocolPack._IALProtocolStructure {
private int pos;
private long summonId;
private int pauseLeftSec;
private int beginUnlockTimeS;


public WCGGS2GC_SummonInfo() {
	pos = 0;
	summonId = (long)0;
	pauseLeftSec = 0;
	beginUnlockTimeS = 0;
}

public WCGGS2GC_SummonInfo(
	int _pos
	, long _summonId
	, int _pauseLeftSec
	, int _beginUnlockTimeS
) {	pos = _pos;
	summonId = _summonId;
	pauseLeftSec = _pauseLeftSec;
	beginUnlockTimeS = _beginUnlockTimeS;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getPos() { return pos; }
public void setPos(int _pos) { pos = _pos; }
public long getSummonId() { return summonId; }
public void setSummonId(long _summonId) { summonId = _summonId; }
public int getPauseLeftSec() { return pauseLeftSec; }
public void setPauseLeftSec(int _pauseLeftSec) { pauseLeftSec = _pauseLeftSec; }
public int getBeginUnlockTimeS() { return beginUnlockTimeS; }
public void setBeginUnlockTimeS(int _beginUnlockTimeS) { beginUnlockTimeS = _beginUnlockTimeS; }


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
	pos = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	summonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pauseLeftSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	beginUnlockTimeS = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(pos);
	_buf.putLong(summonId);
	_buf.putInt(pauseLeftSec);
	_buf.putInt(beginUnlockTimeS);
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
	builder.Append("pos").Append(":").Append(pos.ToString()).Append(", ");
	builder.Append("summonId").Append(":").Append(summonId.ToString()).Append(", ");
	builder.Append("pauseLeftSec").Append(":").Append(pauseLeftSec.ToString()).Append(", ");
	builder.Append("beginUnlockTimeS").Append(":").Append(beginUnlockTimeS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

