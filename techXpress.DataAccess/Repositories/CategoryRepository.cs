using techXpress.DataAccess.Abstraction;
using techXpress.DataAccess.Data;
using techXpress.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace techXpress.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db)
            : base(db)
        {
            _db = db;
        }

    }
}