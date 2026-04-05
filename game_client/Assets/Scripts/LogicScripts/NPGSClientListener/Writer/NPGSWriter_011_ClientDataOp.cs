

namespace GOE
{
    public class NPGSWriter_011_ClientDataOp
    {
        public static GC2GS.p011_ClientDataOp.GC2GS_011_001_ReqQueryClientData make_001_ReqQueryClientData(ENPClientDataType _dataType)
        {
            return make_001_ReqQueryClientData((int)_dataType);
        }
        public static GC2GS.p011_ClientDataOp.GC2GS_011_001_ReqQueryClientData make_001_ReqQueryClientData(int _dataIndex)
        {
            GC2GS.p011_ClientDataOp.GC2GS_011_001_ReqQueryClientData protocol = new GC2GS.p011_ClientDataOp.GC2GS_011_001_ReqQueryClientData();
            protocol.setIndex(_dataIndex);

            return protocol;
        }

        public static GC2GS.p011_ClientDataOp.GC2GS_011_002_ReqSaveClientData make_002_ReqSaveClientData(int _dataIndex, byte[] _data)
        {
            GC2GS.p011_ClientDataOp.GC2GS_011_002_ReqSaveClientData protocol = new GC2GS.p011_ClientDataOp.GC2GS_011_002_ReqSaveClientData();
            protocol.setIndex(_dataIndex);
            protocol.setData(_data);

            return protocol;
        }
    }
}
