namespace _Project.Scripts
{
    public interface IConsumer
    {
        public void PerformOnFilled();
        public void PerformOnReceiveResource();
    }
}