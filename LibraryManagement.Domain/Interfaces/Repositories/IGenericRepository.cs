using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Repositories;

public interface IGenericRepository<T> where T : AuditBase
{
    //Consultar todos
    Task<IEnumerable<T>> GetAllAsync();
    //Consultar por Id
    Task<T?> GetByIdAsync(int id);
    //Crear
    Task<T> CreateAsync(T entity);
    //Actualizar
    Task<T> UpdateAsync(T entity);
    //Eliminar
    Task<bool> DeleteAsync(int id);
    //Validar existencia
    Task<bool> ExistsAsync(int id);
}