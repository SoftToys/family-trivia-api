using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FamilyTrivia.Contracts.Models;
using FamilyTrivia.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace FamilyTrivia.Api.Host.Controllers
{
    [Authorize(policy: "HasId")]
    [EnableCors("Light")]
    [Route("api/[controller]")]
    public class GameManageController : Controller
    {
        private IGamesRepositoryService _gamesService;

        public GameManageController(IGamesRepositoryService gamesService)
        {
            _gamesService = gamesService;

        }
        // GET api/values
        [HttpGet]
        public Task<IEnumerable<TriviaGame>> Get()
        {
            //Request.HttpContext.User.Identity.Name - need to have autorization..
            return _gamesService.GetByOwner(User.Identity.Name);            
        }

        // GET api/values
        [HttpGet("rating")]
        public Task<IEnumerable<UserRating>> GetUserRating()
        {
            //Request.HttpContext.User.Identity.Name - need to have autorization..
            return _gamesService.GetUserRating();            
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<TriviaGame> Get(Guid id) 
        {
            return await _gamesService.GetById(id);
        }

        // GET api/values/5
        [HttpGet("getbyquestion/{id}")]
        public ContentResult GetByQuestionId(Guid id)
        {
            //ContentResult a = new ContentResult();
            //return new ContentResult(_gamesService.GetByQuestionId(id));
            //Response.AppendHeader("Content-Disposition", cd.ToString());
            //return File(document.Data, document.ContentType);
            return null;

        }

        // POST api/values
        [HttpPost]
        public async Task<Guid> Post([FromBody]TriviaGame game)
        {
            var theUser = User.Identity.Name;
            return await _gamesService.AddUpdate(game);
        }

        // PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
