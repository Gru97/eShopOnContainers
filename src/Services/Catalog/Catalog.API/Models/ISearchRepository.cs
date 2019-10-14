using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models
{
    public interface ISearchRepository<TEntity>
    {
        Task<bool>  SaveAsync(TEntity entity);
        Task<bool> SaveManyAsync(IEnumerable<TEntity> Entites);
        Task<bool> DeleteAsync(int Id);
        Task<IEnumerable<TEntity>> SearchAsync(string Phrase);
        Task<IEnumerable<TEntity>> SearchAsync(SearchModel model);

        Task<IEnumerable<TEntity>> SearchByCatalogNameAsync(string Phrase);

    }
}
