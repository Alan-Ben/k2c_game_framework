using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace RemarkData
{

public class TutorialData : ALBasicProtocolPack._IALProtocolStructure {
private long lastFinishForceTutorial;
private List<long> finishTutorialId;


public TutorialData() {
	lastFinishForceTutorial = (long)0;
	finishTutorialId = new List<long>();
}

public TutorialData(
	long _lastFinishForceTutorial
	, List<long> _finishTutorialId
) {	lastFinishForceTutorial = _lastFinishForceTutorial;
	finishTutorialId = _finishTutorialId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getLastFinishForceTutorial() { return lastFinishForceTutorial; }
public void setLastFinishForceTutorial(long _lastFinishForceTutorial) { lastFinishForceTutorial = _lastFinishForceTutorial; }
public List<long> getFinishTutorialId() { return finishTutorialId; }
public void addFinishTutorialId(long _finishTutorialId) { finishTutorialId.Add(_finishTutorialId); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (finishTutorialId.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (finishTutorialId.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastFinishForceTutorial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _finishTutorialIdCount = _buf.getShort();
	for(int _i = 0; _i < _finishTutorialIdCount; _i++) { 
		long _finishTutorialId = (long)0;
		_finishTutorialId = _buf.getLong();
		finishTutorialId.Add(_finishTutorialId);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(lastFinishForceTutorial);
	_buf.putShort((short)finishTutorialId.Count);
	for(int _i = 0; _i < finishTutorialId.Count; _i++) { 
		_buf.putLong(finishTutorialId[_i]);
	}
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
	builder.Append("lastFinishForceTutorial").Append(":").Append(lastFinishForceTutorial.ToString()).Append(", ");
	builder.Append("finishTutorialId").Append(":").Append(finishTutorialId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

