using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动新增推送
/// </summary>
public class GS2GC_017_050_OnActivityAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动信息
/// </summary>
private Common.ActivityObj.Activity_Info activity;


public GS2GC_017_050_OnActivityAdd() {
	activity = new Common.ActivityObj.Activity_Info();
}

public GS2GC_017_050_OnActivityAdd(
	Common.ActivityObj.Activity_Info _activity
) {	activity = _activity;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 活动信息
/// </summary>
public Common.ActivityObj.Activity_Info getActivity() { return activity; }
/// <summary>
/// 活动信息
/// </summary>
public void setActivity(Common.ActivityObj.Activity_Info _activity) { activity = _activity; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + activity.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + activity.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _activityCustLen = _buf.getInt();
	int _activityCurPos = _buf.getCurPos();
	activity.ReadUnzipBuf(_buf, _activityCurPos + _activityCustLen);
	_buf.setPosition(_activityCurPos + _activityCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(activity.GetBufSize());
	activity.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)50);
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
	builder.Append("activity").Append(":").Append(activity == null ? "null" : activity.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

