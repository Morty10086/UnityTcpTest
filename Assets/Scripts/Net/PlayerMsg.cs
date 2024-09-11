using System.Collections;
using System.Collections.Generic;


public class PlayerMsg : BaseMsg
{
    public int playerID;
    public PlayerData playerData;

    public override int GetBytesNum()
    {
        return 4+4+playerData.GetBytesNum();
    }

    public override int Reading(byte[] bytes, int index)
    {
        playerID= ReadInt(bytes, ref index);
        playerData=ReadData<PlayerData>(bytes, ref index);
        return index;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, GetMsgID(), ref index);
        WriteInt(bytes, playerID, ref index);
        WriteData(bytes, playerData,ref index);
        return bytes;
    }

    protected override int GetMsgID()
    {
        return 1001;
    }
}
