using System.Collections.Generic;

namespace Kiwiland
{
    public interface IFileProvider
    {
        List<string> GetFileContents();
    }
}
