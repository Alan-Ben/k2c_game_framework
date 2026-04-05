using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人朋友圈AI对话新增消息
/// </summary>
public class GS2GC_015_076_OnConsortAiChatMomentMsgAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例id
/// </summary>
private long instanceId;
/// <summary>
/// 消息内容
/// </summary>
private string msg;
/// <summary>
/// 错误码
/// </summary>
private int errCode;
/// <summary>
/// 家人ID
/// </summary>
private long consortId;


public GS2GC_015_076_OnConsortAiChatMomentMsgAdd() {
	instanceId = (long)0;
	msg = "";
	errCode = 0;
	consortId = (long)0;
}

public GS2GC_015_076_OnConsortAiChatMomentMsgAdd(
	long _instanceId
	, string _msg
	, int _errCode
	, long _consortId
) {	instanceId = _instanceId;
	msg = _msg;
	errCode = _errCode;
	consortId = _consortId;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)76; }

/// <summary>
/// 实例id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 实例id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 消息内容
/// </summary>
public string getMsg() { return msg; }
/// <summary>
/// 消息内容
/// </summary>
public void setMsg(string _msg) { msg = _msg; }
/// <summary>
/// 错误码
/// </summary>
public int getErrCode() { return errCode; }
/// <summary>
/// 错误码
/// </summary>
public void setErrCode(int _errCode) { errCode = _errCode; }
/// <summary>
/// 家人ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 家人ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }


public int GetBufSize() {
	int _size = 20;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(msg);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(msg);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msg = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putString(msg);
	_buf.putInt(errCode);
	_buf.putLong(consortId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)76);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)76);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("msg").Append(":").Append(msg.ToString()).Append(", ");
	builder.Append("errCode").Append(":").Append(errCode.ToString()).Append(", ");
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

