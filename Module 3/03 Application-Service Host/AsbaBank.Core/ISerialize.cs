using System.IO;

namespace AsbaBank.Core
{
    public interface ISerialize
    {
        void Serialize<T>(Stream output, T graph);
        T Deserialize<T>(Stream input);
    }
}
