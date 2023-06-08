using ShowRoomAPI.DataAccess.Interface;

namespace ShowRoomAPI.DataAccess.Implementation
{
    public class ScopedService : IScopedService
    {
        private Guid guid;
        public ScopedService()
        {
            guid = Guid.NewGuid();
            GuidString = guid.ToString();
        }
        public string GuidString { get; set; }
    }

    public class SingletonService : ISIngletonService
    {
        private Guid guid;
        public SingletonService()
        {
            guid = Guid.NewGuid();
            GuidString = guid.ToString();
        }
        public string GuidString { get; set; }
    }

    public class TransientService : ITransientService
    {
        private Guid guid;
        public TransientService()
        {
            guid = Guid.NewGuid();
            GuidString = guid.ToString();
        }
        public string GuidString { get; set; }
    }
}
