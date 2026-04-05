using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_EliteInfo : ALBasicProtocolPack._IALProtocolStructure {
private long id;
private long refId;
private int passtime;
private int battleTask;
private int dailyPassCount;


public NPCommon_EliteInfo() {
	id = (long)0;
	refId = (long)0;
	passtime = 0;
	battleTask = 0;
	dailyPassCount = 0;
}

public NPCommon_EliteInfo(
	long _id
	, long _refId
	, int _passtime
	, int _battleTask
	, int _dailyPassCount
) {	id = _id;
	refId = _refId;
	passtime = _passtime;
	battleTask = _battleTask;
	dailyPassCount = _dailyPassCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
public int getPasstime() { return passtime; }
public void setPasstime(int _passtime) { passtime = _passtime; }
public int getBattleTask() { return battleTask; }
public void setBattleTask(int _battleTask) { battleTask = _battleTask; }
public int getDailyPassCount() { return dailyPassCount; }
public void setDailyPassCount(int _dailyPassCount) { dailyPassCount = _dailyPassCount; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	passtime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	battleTask = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dailyPassCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(refId);
	_buf.putInt(passtime);
	_buf.putInt(battleTask);
	_buf.putInt(dailyPassCount);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("passtime").Append(":").Append(passtime.ToString()).Append(", ");
	builder.Append("battleTask").Append(":").Append(battleTask.ToString()).Append(", ");
	builder.Append("dailyPassCount").Append(":").Append(dailyPassCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

