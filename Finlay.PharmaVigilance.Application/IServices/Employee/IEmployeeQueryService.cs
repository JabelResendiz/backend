
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IEmployeeQueryServices : IGenericQueryService<Employee,GetEmployeeDto>
{
    // Task<IEnumerable<GetEmployeeDto>> ListAllByRole(UserRole role);

    Task<GetEmployeeDto> GetByUserNameAsync(string employeeUserName);



}