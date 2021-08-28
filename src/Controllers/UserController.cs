using Dapper;
using Microsoft.AspNetCore.Mvc;
using Orion.Models;
using Orion.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IDataAccessService _dataAccesService;
        IDbConnection _sqliteConnection;

        public UserController(IDataAccessService dataAccessService)
        {
            _dataAccesService = dataAccessService;
            _sqliteConnection = _dataAccesService.LoadConnectionString();
        }

        // GET: api/<UserController>
        [HttpGet]
        public List<UserModel> Get()
        {
            using (_sqliteConnection)
            {
                var output = _sqliteConnection.Query<UserModel>("SELECT * FROM users", new DynamicParameters());
                return output.ToList();
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public IEnumerable<UserModel> Get(int id)
        {
            using (_sqliteConnection)
            {
                var output = _sqliteConnection.Query<UserModel>("SELECT * FROM users WHERE Id = {id}", new DynamicParameters());
                return output.ToList().Where(s => s.Id == id);
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
