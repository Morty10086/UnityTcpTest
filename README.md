基于C#Socket的Unity TCP 步传输方案尝试
服务端由C#编写，主要负责接收并转发客户端消息(游戏数据、角色实时信息等)，用以同步两个客户端的游戏进程；
客户端由Unity实现，负责游戏逻辑的执行和服务器连接；
基本思路是通过C#Socket提供的API建立客户端与服务端之间的TCP协议通讯，由服务端接收客户端游戏数据再转发给另一个客户端，以实现游戏信息的同步；
目前延迟较高，待优化。
