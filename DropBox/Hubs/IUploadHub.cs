using System.Threading.Tasks;

namespace DropBox.Hubs
{
    public interface IUploadHub
    {
        Task SendProgress(int progress);
    }
}