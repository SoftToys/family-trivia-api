﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FamilyTrivia.Contracts.Models;
using FamilyTrivia.Contracts;

namespace FamilyTrivia.Api.Host.Controllers
{
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
        public IEnumerable<TriviaGame> Get()
        {            
            //Request.HttpContext.User.Identity.Name - need to have autorization..
            return _gamesService.GetByOwner(Request.HttpContext.User.Identity.Name);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public TriviaGame Get(Guid id) 
        {
            return _gamesService.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public Guid Post([FromBody]TriviaGame game)
        {
            return _gamesService.AddUpdate(game);
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