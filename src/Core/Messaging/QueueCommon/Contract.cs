namespace Core.Messaging.QueueCommon
{
    public class Contract
    {
        public string QueueName { get; set; }
        public string HostName { get; set; }
        public string Identifier { get; set; }
        public string Number { get; set; }
        public string Finished { get; set; }
        public string IsChunk { get; set; }
        public int ChunkSize { get; set; }
    }
}
