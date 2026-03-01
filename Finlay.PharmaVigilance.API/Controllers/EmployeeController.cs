using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EmployeeController : ControllerBase
{
     private readonly IEmployeeQueryServices _employeeQueryService;
     private readonly IEmployeeCommandServices _employeeCommandService;

    public EmployeeController(IEmployeeQueryServices employeeQueryServices,
                                 IEmployeeCommandServices employeeCommandServices)

    {
        _employeeQueryService = employeeQueryServices;
        _employeeCommandService = employeeCommandServices;
    }

    #region  Query
    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<GetEmployeeDto>> GetEmployeeById(int employeeId)
    {
        var result = await _employeeQueryService.GetByIdAsync(employeeId);

        if (result == null)
            return NotFound($"Employee with ID {employeeId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GetByUsername")]
    public async Task<ActionResult<GetEmployeeDto>> GetEmployeeByUserName(string employeeUsername)
    {
        var result = await _employeeQueryService.GetByUserNameAsync(employeeUsername);

        if (result == null)
            return NotFound($"Employee with ID {employeeUsername} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<GetEmployeeDto>>> GetAllEmployee ()
    {
        var result = await _employeeQueryService.ListAsync();

        return Ok(result);
    }   

    #endregion

    #region Command
    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteEmployee(int employeeId)
    {
        await _employeeCommandService.DeleteAsync(employeeId);

        return Ok("Employee deleted successfully");
    }

    
    #endregion

}