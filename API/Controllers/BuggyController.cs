using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class BuggyController : BaseApiController
  {
    private readonly StoreContext _context;
    public BuggyController(StoreContext context)
    {
      _context = context;
    }

    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
        var nonExistent = _context.Products.Find(42);
        if (nonExistent == null)
        {
            return NotFound(new ApiResponse(404));
        }

        return Ok();
    }

    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
        var nonExistent = _context.Products.Find(42);
        var toReturn = nonExistent.ToString();

        return Ok();
    }

    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult GetNotFoundRequest(int id)
    {
        return Ok();
    }
  }
}