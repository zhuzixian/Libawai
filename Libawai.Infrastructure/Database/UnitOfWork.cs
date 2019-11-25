using System.Threading.Tasks;
using Libawai.Core.Interfaces;

namespace Libawai.Infrastructure.Database
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly LibawaiDbContext _context;

        public UnitOfWork(LibawaiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
