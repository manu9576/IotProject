namespace Storage
{
    public interface ISensorsStorage
    {
        void Start(int intervalInSeconds);
        void Stop();
    }
}