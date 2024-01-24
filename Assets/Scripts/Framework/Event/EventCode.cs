namespace BEBE.Framework.Event
{
    public enum EventCode
    {
        ON_GAME_START,
        GET_CHANNEL_ID,
        PING_RPC,
        CALL_JOIN_IN_REQUEST_METHOD,
        RCP_FROM_CLIENT,
        ON_CLIENT_DISCONNECTING,
        PING,
        ON_SYNC_CMD,
        ON_RECV_INPUT,
        JOIN_IN,
        DONT_FIND_ROOM_RPC,
        CALL_CREATE_ROOM_REQUEST_METHOD,
        CREATE_ROOM,
        CREATE_ROOM_RPC,
        JOIN_IN_RPC,
        HAS_JOINED_ROOM_RPC,
        CALL_EXIT_ROOM_METHOD,
        EXIT_ROOM,
        EXIT_ROOM_RPC,
        UPDATE_ROOM_RPC,
        CALL_GET_READY_METHOD,
        GET_READY,
        CALL_CANCEL_READY_METHOD,
        CANCEL_READY,
        CALL_PLAY_METHOD,
        PLAY,
        ROOM_IS_NOT_FULL_RPC,
        ROOM_NOT_ALL_ARE_READY_RPC,
        PLAY_RPC,
        CALL_SYNC_LOAD_PROGRESS_METHOD,
        SYNC_LOAD_PROGRESS,
        SYNC_LOAD_PROGRESS_RPC,
        CALL_LOADING_COMPLETED_METHOD,
        LOADING_COMPLETED,
        LOADING_COMPLETED_RPC,
        CALL_PUSH_CMD_METHOD,
        PUSH_CMD,
        CALL_PULL_CMD_METHOD,
        PULL_CMD,
        ENUM_COUNT,
    }
}