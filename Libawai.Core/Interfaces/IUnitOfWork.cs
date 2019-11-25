using System.Threading.Tasks;

namespace Libawai.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveAsync();
    }
}
