using System;
using System.Collections.Generic;
using System.Text;
// ESTE ROPOSITORIO GEN�RICO SE UTILIZA PARA NO TENER QUE REPLICAR
//TODAS LAS ENTIDADES CON EL MISMO C�DIGO DE ACCESO A DATOS.
// CON ESTO APLICAMOS:
// SOLID: Open/Closed Principle - esta interfaz permanece cerrada a modificaci�n;
// el comportamiento espec�fico de cada entidad se agrega extendi�ndola en
// interfaces, sin necesidad de tocar este archivo.

using System.Linq.Expressions;

namespace PizzeriaApp.Repository.Interfaces;

public interface IGenericRepository<T> where T : class //ESTO FUNCIONA PARA CUALQUIER ENTIDAD,
                                                       //NO SOLO PARA LAS QUE HEREDAN DE UNA CLASE BASE
{
    Task<IEnumerable<T>> GetAllAsync(); // EL GetAllAsync(); LO UTILIZAMOS PARA OBTENER TODOS LOS REGISTROS
                                        // DE LA ENTIDAD

    Task<T?> GetByIdAsync(int id);  // EL GetByIdAsync(int id); LO UTILIZAMOS PARA OBTENER UN REGISTRO POR SU ID

    Task<T> AddAsync(T entity); // EL AddAsync(T entity); LO UTILIZAMOS PARA AGREGAR UN NUEVO REGISTRO A LA ENTIDAD

    Task UpdateAsync(T entity);    // EL UpdateAsync(T entity); LO UTILIZAMOS PARA ACTUALIZAR UN REGISTRO EXISTENTE DE LA ENTIDAD

    Task DeleteAsync(int id);

    Task<bool> ExistsAsync(int id); // EL ExistsAsync(int id); LO UTILIZAMOS PARA VER SI UN REGISTRO EXISTE EN LA ENTIDAD POR SU ID

    Task<IEnumerable<T>> FindAsync( // EL FindAsync LO UTILIZAMOS PARA FILTRAR LOS REGISTROS DE LA ENTIDAD 
        Expression<Func<T, bool>> predicate);
}