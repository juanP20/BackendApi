using BackendAPI.Models;
using BackendAPI.Service.Contrato;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Service.Implementacion
{
   
    public class EmpleadoService : IEmpleadoService
    {
        private DbempleadoContext _dbContext;
        public EmpleadoService(DbempleadoContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Empleado>> GetList()
        {
            try
            {
                List<Empleado>  lista = new List<Empleado>();
                lista = await _dbContext.Empleados.Include(d => d.IdDepartamentoNavigation).ToListAsync();

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Empleado> Get(int idEmpleado)
        {
            try
            {
                Empleado? empleado = new Empleado();
                empleado = await _dbContext.Empleados.Include(d => d.IdDepartamentoNavigation)
                    .Where(e => e.IdEmpleado == idEmpleado).FirstOrDefaultAsync();

                return empleado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Empleado> add(Empleado modelo)
        {
            try
            {
                _dbContext.Empleados.Add(modelo);
                await _dbContext.SaveChangesAsync();
                return modelo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Update(Empleado modelo)
        {
            try
            {
                _dbContext.Empleados.Update(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Delete(Empleado modelo)
        {
            try
            {
                _dbContext.Empleados.Remove(modelo);
                await _dbContext.SaveChangesAsync();
                return true;    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
