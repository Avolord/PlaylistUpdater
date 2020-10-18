
namespace PlaylistUpdater
{
    static class Constants
    {
        public const string DefaultPathPrefix = @".\resources";
    }

    public abstract class ExternalConfig<T>
    {
        public T Data { get; set; }
        public abstract void Load(string path);
        public abstract void Save(string path);
        protected abstract void Generate(string destination);
    }
}
