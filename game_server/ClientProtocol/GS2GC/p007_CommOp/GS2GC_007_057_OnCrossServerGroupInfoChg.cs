using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_057_OnCrossServerGroupInfoChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.Common_CrossServerGroupInfo groupInfo;


public GS2GC_007_057_OnCrossServerGroupInfoChg() {
	groupInfo = new Common.Common_CrossServerGroupInfo();
}

public GS2GC_007_057_OnCrossServerGroupInfoChg(
	Common.Common_CrossServerGroupInfo _groupInfo
) {	groupInfo = _groupInfo;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)57; }

public Common.Common_CrossServerGroupInfo getGroupInfo() { return groupInfo; }
public void setGroupInfo(Common.Common_CrossServerGroupInfo _groupInfo) { groupInfo = _groupInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + groupInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + groupInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _groupInfoCustLen = _buf.getInt();
	int _groupInfoCurPos = _buf.getCurPos();
	groupInfo.ReadUnzipBuf(_buf, _groupInfoCurPos + _groupInfoCustLen);
	_buf.setPosition(_groupInfoCurPos + _groupInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(groupInfo.GetBufSize());
	groupInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)57);
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
	builder.Append("groupInfo").Append(":").Append(groupInfo == null ? "null" : groupInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

