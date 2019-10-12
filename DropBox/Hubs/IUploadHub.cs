using System.Threading.Tasks;

namespace DropBox.Hubs
{
    public interface IUploadHub
    {
        Task SendProgress(string fileName,int progress);
    }
}