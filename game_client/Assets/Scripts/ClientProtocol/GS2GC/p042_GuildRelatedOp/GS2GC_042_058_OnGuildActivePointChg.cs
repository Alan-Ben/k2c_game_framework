using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

/// <summary>
/// 联盟活跃点变化数据
/// </summary>
public class GS2GC_042_058_OnGuildActivePointChg : ALBasicProtocolPack._IALProtocolStructure {
private long activePoint;
private int targetLvl;


public GS2GC_042_058_OnGuildActivePointChg() {
	activePoint = (long)0;
	targetLvl = 0;
}

public GS2GC_042_058_OnGuildActivePointChg(
	long _activePoint
	, int _targetLvl
) {	activePoint = _activePoint;
	targetLvl = _targetLvl;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)58; }

public long getActivePoint() { return activePoint; }
public void setActivePoint(long _activePoint) { activePoint = _activePoint; }
public int getTargetLvl() { return targetLvl; }
public void setTargetLvl(int _targetLvl) { targetLvl = _targetLvl; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activePoint = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	targetLvl = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activePoint);
	_buf.putInt(targetLvl);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)58);
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
	builder.Append("activePoint").Append(":").Append(activePoint.ToString()).Append(", ");
	builder.Append("targetLvl").Append(":").Append(targetLvl.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

