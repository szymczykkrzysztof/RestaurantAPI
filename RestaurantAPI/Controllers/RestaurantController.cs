using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers;

[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IMapper _mapper;

    public RestaurantController(RestaurantDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        var restaurants = _dbContext
            .Restaurants
            .Include(r=>r.Address)
            .Include(r=>r.Dishes)
            .ToList();
        var restaurantDtos = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        return Ok(restaurantDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<RestaurantDto> Get([FromRoute] int id)
    {
        var restaurant = _dbContext
            .Restaurants
            .Include(r=>r.Address)
            .Include(r=>r.Dishes)
            .FirstOrDefault(r => r.Id == id);
        if (restaurant is null) return NotFound();
        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
        return Ok(restaurantDto);
    }
}