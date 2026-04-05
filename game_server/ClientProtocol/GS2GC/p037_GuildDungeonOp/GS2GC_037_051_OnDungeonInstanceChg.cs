using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p037_GuildDungeonOp
{

public class GS2GC_037_051_OnDungeonInstanceChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildDungeonObj.GuildDungeon_InstanceInfo instanceInfo;


public GS2GC_037_051_OnDungeonInstanceChg() {
	instanceInfo = new Common.GuildDungeonObj.GuildDungeon_InstanceInfo();
}

public GS2GC_037_051_OnDungeonInstanceChg(
	Common.GuildDungeonObj.GuildDungeon_InstanceInfo _instanceInfo
) {	instanceInfo = _instanceInfo;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)51; }

public Common.GuildDungeonObj.GuildDungeon_InstanceInfo getInstanceInfo() { return instanceInfo; }
public void setInstanceInfo(Common.GuildDungeonObj.GuildDungeon_InstanceInfo _instanceInfo) { instanceInfo = _instanceInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + instanceInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + instanceInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _instanceInfoCustLen = _buf.getInt();
	int _instanceInfoCurPos = _buf.getCurPos();
	instanceInfo.ReadUnzipBuf(_buf, _instanceInfoCurPos + _instanceInfoCustLen);
	_buf.setPosition(_instanceInfoCurPos + _instanceInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(instanceInfo.GetBufSize());
	instanceInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)51);
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
	builder.Append("instanceInfo").Append(":").Append(instanceInfo == null ? "null" : instanceInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

