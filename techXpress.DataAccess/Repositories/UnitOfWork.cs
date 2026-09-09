using techXpress.DataAccess.Abstraction;
using techXpress.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public ICategoryRepository CategoryRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public IOrderRepository OrderRepository { get; private set; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            CategoryRepository = new CategoryRepository(db);
            ProductRepository = new ProductRepository(db);
            OrderRepository = new OrderRepository(db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
