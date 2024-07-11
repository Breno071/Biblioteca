using System.Reflection;

namespace Infraestructure.Helpers
{
    public static class DirectoryHelper
    {
        public static DirectoryInfo? FindSolutionDirectory()
        {
            var currentyDirectory = Directory.GetCurrentDirectory();
            var directoryInfo = new DirectoryInfo(currentyDirectory);

            while(directoryInfo != null && !directoryInfo.GetFiles("*.sln").Any())
            {
                directoryInfo = directoryInfo.Parent;
            }

            return directoryInfo;
        }
    }
}
