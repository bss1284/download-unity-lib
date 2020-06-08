public enum DownloadStatus {
    Invalid=-1,
    Pending=1<<0,
    Running = 1 << 1,
    Paused=1<<2,
    Successful=1<<3,
    Failed=1<<4
}