using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using FamilyTrivia.Contracts.Models;
using FamilyTrivia.Contracts;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FamilyTrivia.Api.Host.Controllers
{
    [Authorize(policy: "HasId")]
    [EnableCors("Light")]
    [Route("api/rating")]
    public class RatingController : Controller
    {
        private IGamesRepositoryService _gamesService;

        public RatingController(IGamesRepositoryService gamesService)
        {
            _gamesService = gamesService;

        }

        // GET api/values

        public Task<IEnumerable<UserRating>> GetUserRating()
        {        
            return _gamesService.GetUserRating();
        }
    }
}
